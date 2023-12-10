using EvolveDb.Configuration;
using Krypton.Budgets.Domain._Impl.Attributes;
using Krypton.Budgets.Domain._Ports.Persistence;
using Krypton.Budgets.Persistence.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Type;
using System.Globalization;
using System.Reflection;
using Configuration = NHibernate.Cfg.Configuration;

namespace Krypton.Budgets.Persistence.Implementation;

internal class NHibernateBridge : IPersistenceHosting
{
    public static Configuration? GLOBAL_CONFIG { get; private set; }
    private static readonly object _gfLock = new();

    private static Func<MemberInfo, bool, bool> BASE_ISPERSISTENTPROPERTY_IMPL = (_, _) => false;

    private class Domain
    {
        internal Func<Type, string> GetEntitySubdomain { get; set; }
        internal MemberInfo Identifier { get; set; }
        internal MemberInfo WhenCreated { get; set; }
        internal MemberInfo WhenUpdated { get; set; }
        internal List<TypeInfo> Abstract { get; set; }
        internal List<TypeInfo> Entities { get; set; }
        internal List<TypeInfo> Components { get; set; }
        internal List<TypeInfo> Complete { get; set; }

        internal Domain(IDomainExplorer port)
        {
            GetEntitySubdomain = type => "budgets_" + port.GetEntitySubdomain(type);

            Identifier = port.DomainIdentifier;
            WhenCreated = port.WhenCreated;
            WhenUpdated = port.WhenUpdated;

            var unordered = port.DomainEntites.ToList();

            Abstract = unordered.Where(t => t.IsAbstract && !t.IsInterface).ToList();
            Entities = unordered.Where(t => !t.IsAbstract && !t.IsInterface && Abstract.Any(r => r.IsAssignableFrom(t))).ToList();
            Components = unordered.Where(t => !t.IsAbstract && !t.IsInterface && !Entities.Contains(t)).ToList();

            Complete = (new List<TypeInfo>[] { Abstract, Components, Entities }).SelectMany(t => t).ToList();
        }
    }

    private readonly Domain domain;
    private readonly string connectionString;
    private readonly Configuration dbConfig;
    private readonly ILogger<NHibernateBridge> logger;

    private ISessionFactory? factory;

    public NHibernateBridge(IDomainExplorer port, IConfiguration config, ILogger<NHibernateBridge> logger)
    {
        domain = new Domain(port);

        connectionString = config["Persistence:ConnectionString"] ?? "";

        lock (_gfLock)
        {
            GLOBAL_CONFIG ??= BuildConfig(connectionString);
        }
        dbConfig = GLOBAL_CONFIG;

        this.logger = logger;
    }

    void IPersistenceHosting.ResetDB(string master, string dbname, string dbowner)
    {
        var server = new Server();
        server.ConnectionContext.AutoDisconnectMode = AutoDisconnectMode.NoAutoDisconnect;
        server.ConnectionContext.Connect();

        if (server.Databases.Contains(dbname))
        {
            server.Databases[dbname].Drop();
        }

        var db = new Database(server, dbname)
        {
            Collation = "Latin1_General_100_CI_AI"
        };
        db.Create();
        db.SetOwner(dbowner);

        server.ConnectionContext.Disconnect();
    }

    string IPersistenceHosting.GetDDL()
    {
        string result = "";
        new SchemaUpdate(dbConfig).Execute(x => result += x + '\n', false);
        return result;
    }

    void IPersistenceHosting.RunMigrations()
    {
        using (var cnx = new SqlConnection(connectionString))
        {
            var evolve = new EvolveDb.Evolve(cnx, msg => logger.LogInformation("{Msg}", msg))
            {
                TransactionMode = TransactionKind.CommitAll,
                IsEraseDisabled = true,
                EmbeddedResourceAssemblies = new[] { GetType().Assembly },
                EmbeddedResourceFilters = new[] { "Krypton.Budgets.Persistence.Migrations" }
            };
            evolve.Migrate();
        }
    }

    void IPersistenceHosting.DropDB(string master, string dbname, string dbowner)
    {
        var server = new Server();
        server.ConnectionContext.AutoDisconnectMode = AutoDisconnectMode.NoAutoDisconnect;
        server.ConnectionContext.Connect();

        var db = server.Databases[dbname];
        db.DropIfExists();

        server.ConnectionContext.Disconnect();
    }

    internal ISessionFactory GetFactory()
    {
        return factory ?? BuildFactory();
    }

    private ISessionFactory BuildFactory()
    {
        factory = dbConfig.BuildSessionFactory();
        return factory;
    }

    private Configuration BuildConfig(string connectionString)
    {
        var result = new Configuration();
        result.DataBaseIntegration(c =>
        {
            c.Dialect<MsSql2012Dialect>();
            c.ConnectionString = connectionString;
            c.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
            c.SchemaAction = SchemaAutoAction.Validate;
            c.LogFormattedSql = true;
            c.LogSqlInConsole = true;
            c.ExceptionConverter<ExceptionConverter>();
        });
        result.AddMapping(BuildMapping());

        return result;
    }

    private HbmMapping BuildMapping()
    {
        var mapper = new ConventionModelMapper();

        BASE_ISPERSISTENTPROPERTY_IMPL = (Func<MemberInfo, bool, bool>?)mapper.ModelInspector.GetType().
            GetField("isPersistentProperty", BindingFlags.NonPublic | BindingFlags.Instance)?.
            GetValue(mapper.ModelInspector) ?? BASE_ISPERSISTENTPROPERTY_IMPL;

        mapper.IsEntity(IsEntity);
        mapper.IsComponent(IsComponent);
        mapper.IsRootEntity(IsRootEntity);
        mapper.IsPersistentId(IsPersistentId);
        mapper.IsPersistentProperty(IsPersistentProperty);
        mapper.IsSet(IsSetFieldType);

        mapper.BeforeMapClass += BeforeMapClass;
        mapper.BeforeMapJoinedSubclass += BeforeMapJoinedSubclass;
        mapper.BeforeMapProperty += BeforeMapProperty;
        mapper.BeforeMapManyToOne += BeforeMapManyToOne;
        mapper.BeforeMapSet += BeforeMapSet;

        AddAllManyToManyRelations(mapper, domain.Entities);
        return mapper.CompileMappingFor(domain.Complete);
    }

    private bool IsEntity(Type type, bool declared)
    {
        return domain.Entities.Contains(type);
    }

    private bool IsComponent(Type type, bool declared)
    {
        return domain.Components.Contains(type) && !type.IsEnum;
    }

    private bool IsRootEntity(Type type, bool declared)
    {
        return domain.Abstract.Contains(type.BaseType);
    }

    private bool IsPersistentId(MemberInfo member, bool declared)
    {
        return member.DeclaringType == domain.Identifier.DeclaringType && member.Name == domain.Identifier.Name;
    }

    private bool IsPersistentProperty(MemberInfo member, bool declared)
    {
        var tmp = BASE_ISPERSISTENTPROPERTY_IMPL(member, declared);
        return tmp &&
            (member as PropertyInfo)?.GetGetMethod(false) is not null &&
            !Attribute.IsDefined(member, typeof(ExternalAttribute));
    }

    private bool IsSetFieldType(MemberInfo member, bool declared)
    {
        var typeDefs = member.GetPropertyOrFieldType().GetGenericInterfaceTypeDefinitions();
        foreach (Type t in typeDefs)
        {
            if (t == typeof(IEnumerable<>) &&
                member.GetPropertyOrFieldType().IsGenericCollection() &&
                domain.Complete.Contains(member.GetPropertyOrFieldType().GetGenericArguments()[0]))
            {
                return true;
            }
        }

        var fieldInfo = PropertyToField.GetBackFieldInfo((PropertyInfo)member);
        if (fieldInfo is null)
        {
            return false;
        }

        typeDefs = fieldInfo.FieldType.GetGenericInterfaceTypeDefinitions();
        foreach (Type t in typeDefs)
        {
            if (t == typeof(IEnumerable<>))
            {
                return true;
            }
        }

        return false;
    }

    private void BeforeMapClass(IModelInspector modelInspector, Type type, IClassAttributesMapper mapper)
    {
        mapper.Schema(domain.GetEntitySubdomain(type).ToLower());
        mapper.Table("tbl" + new Inflector.Inflector(new CultureInfo("en")).Pluralize(type.Name).ToLower());
        mapper.Id(k =>
        {
            k.Generator(Generators.GuidComb);
            k.Column("pk");
            k.UnsavedValue(Guid.Empty);
        });
        mapper.Lazy(false);
    }

    private void BeforeMapJoinedSubclass(IModelInspector modelInspector, Type type, IJoinedSubclassAttributesMapper mapper)
    {
        mapper.Schema(domain.GetEntitySubdomain(type).ToLower());
        mapper.Table("tbl" + new Inflector.Inflector(new CultureInfo("en")).Pluralize(type.Name).ToLower());
        mapper.Key(k =>
        {
            var prefix = type.BaseType?.Name.ToLower() ?? "";
            k.Column($"pk_{prefix}");
            k.ForeignKey($"pk_{prefix}_{type.Name.ToLower()}_{prefix}");
        });
        mapper.Lazy(false);
    }

    private void BeforeMapProperty(IModelInspector modelInspector, PropertyPath member, IPropertyMapper mapper)
    {
        if (member.LocalMember is PropertyInfo propInfo)
        {
            if (Attribute.IsDefined(propInfo, typeof(UniqueAttribute)))
            {
                var attr = (UniqueAttribute)Attribute.GetCustomAttribute(propInfo, typeof(UniqueAttribute))!;

                if (attr.Combinations == null)
                {
                    mapper.Unique(true);
                }
                else
                {
                    mapper.UniqueKey(attr.Combinations);
                }
            }

            var colName = modelInspector.IsComponent(propInfo.DeclaringType) ?
                member.PreviousPath.LocalMember.Name.ToLower() + "_" + propInfo.Name.ToLower() :
                propInfo.Name.ToLower();

            mapper.Column(c =>
            {
                c.Name(colName);
                if (Attribute.IsDefined(propInfo, typeof(RequiredAttribute)))
                {
                    c.NotNullable(true);
                    if (propInfo.PropertyType == typeof(string))
                    {
                        c.Check(propInfo.Name.ToLower() + " <> ''");
                    }
                    if (propInfo.PropertyType == typeof(Guid))
                    {
                        c.Check(propInfo.Name.ToLower() + $" <> '{Guid.Empty}'");
                    }
                }
            });

            if (propInfo.PropertyType == typeof(DateOnly) || propInfo.PropertyType == typeof(DateOnly?))
            {
                mapper.Type<DateOnlyMapper>();
            }

            if (propInfo.IsEnum())
            {
                foreach (MethodInfo m in mapper.GetType().GetMethods())
                    if (m.Name == "Type" && m.GetParameters().Length == 0)
                        m.MakeGenericMethod(new Type[] { typeof(EnumStringType<>).MakeGenericType(propInfo.PropertyType) }).Invoke(mapper, null);
            }
        }
    }

    private void BeforeMapManyToOne(IModelInspector modelInspector, PropertyPath member, IManyToOneMapper mapper)
    {
        if (member.LocalMember is PropertyInfo propInfo)
        {
            if (Attribute.IsDefined(propInfo, typeof(UniqueAttribute)))
            {
                var attr = (UniqueAttribute)Attribute.GetCustomAttribute(propInfo, typeof(UniqueAttribute))!;

                if (attr.Combinations == null)
                {
                    mapper.Unique(true);
                }
                else
                {
                    mapper.UniqueKey(attr.Combinations);
                }
            }

            mapper.Column(c =>
            {
                c.Name("fk_" + member.LocalMember.Name.ToLower());
                if (Attribute.IsDefined(propInfo, typeof(RequiredAttribute)))
                {
                    c.NotNullable(true);
                    if (propInfo.PropertyType == typeof(string))
                    {
                        c.Check(propInfo.Name.ToLower() + " <> ''");
                    }
                    if (propInfo.PropertyType == typeof(Guid))
                    {
                        c.Check(propInfo.Name.ToLower() + $" <> '{Guid.Empty}'");
                    }
                }
            });

            mapper.ForeignKey(string.Format("fk_{0}_{1}_{2}",
                member.LocalMember.Name.ToLower(),
                member.LocalMember.DeclaringType?.Name.ToLower() ?? "",
                member.LocalMember.GetPropertyOrFieldType().Name.ToLower()));

            mapper.Cascade(Cascade.Persist);

            mapper.Fetch(FetchKind.Join);
        }
    }

    private void BeforeMapSet(IModelInspector modelInspector, PropertyPath member, ISetPropertiesMapper map)
    {
        var inverse = member.LocalMember.GetInverseReferenceProperty();
        if (inverse is not null)
        {
            map.Key(x => x.Column("fk_" + inverse.Name.ToLower()));
            map.Cascade(Cascade.All | Cascade.DeleteOrphans);
            map.Inverse(true);
        }
    }

    private void AddAllManyToManyRelations(ModelMapper mapper, IEnumerable<Type> entities)
    {
        var cache = new List<string?>();

        foreach (var t in entities)
        {
            var props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var p in props)
            {
                if (cache.Contains(p.ToString()))
                {
                    continue;
                }

                if (Attribute.IsDefined(p, typeof(ExternalAttribute)))
                {
                    continue;
                }

                var inverseProp = p.GetInverseCollectionProperty();

                if (inverseProp is not null)
                {
                    if (cache.Contains(inverseProp.ToString()))
                        continue;

                    typeof(NHibernateBridge)
                            .GetMethod("MapBiManyToMany", BindingFlags.NonPublic | BindingFlags.Static)!
                            .MakeGenericMethod(new[] { p.DeclaringType!, inverseProp.DeclaringType! })
                            .Invoke(null, new object[] { mapper, p, inverseProp });

                    cache.Add(inverseProp.ToString());
                    cache.Add(p.ToString());

                    continue;
                }

                if (p.PropertyType.DetermineCollectionElementOrDictionaryValueType() is null)
                {
                    continue;
                }
                if (p.GetInverseReferenceProperty() is not null)
                {
                    continue;
                }

                GetType().
                    GetMethod("MapUniManyToMany", BindingFlags.NonPublic | BindingFlags.Static)!.
                    MakeGenericMethod(new[] { p.DeclaringType!, p.PropertyType.GetGenericArguments()[0] }).
                    Invoke(null, new object[] { mapper, p });

                cache.Add(p.ToString());
            }
        }
    }

    //private static void MapBiManyToMany<TControllingEntity, TInverseEntity>(ModelMapper mapper, PropertyInfo property, PropertyInfo inverseProperty)
    //    where TControllingEntity : class
    //    where TInverseEntity : class
    //{
    //    var controllingColumnName = string.Format("{0}Id", property.DeclaringType.Name);
    //    var inverseColumnName = string.Format("{0}Id", inverseProperty.DeclaringType.Name);
    //    var tableName = string.Format("{0}To{1}", inverseProperty.CName, property.Name);

    //    if (isSelfReferencingObject(property, inverseProperty))
    //    {
    //        MapSelfReferencingObject<TControllingEntity, TInverseEntity>(mapper, property);
    //    }
    //    else
    //    {
    //        mapper.Class<TControllingEntity>(map =>
    //            map.Bag<TInverseEntity>(property.Name, cm =>
    //            {
    //                cm.Table(tableName);
    //                cm.Cascade(Cascade.Persist /*Cascade.All | Cascade.DeleteOrphans*/);
    //                cm.Key(km =>
    //                {
    //                    km.Column(controllingColumnName);
    //                    km.ForeignKey("");
    //                });
    //                cm.Lazy(CollectionLazy.NoLazy);
    //            },
    //                    em =>
    //                        em.ManyToMany(m =>
    //                            m.Column(inverseColumnName))));

    //        mapper.Class<TInverseEntity>(map =>
    //            map.Bag<TControllingEntity>(inverseProperty.Name, cm =>
    //            {
    //                cm.Table(tableName);
    //                cm.Key(km =>
    //                    km.Column(inverseColumnName));
    //                cm.Lazy(CollectionLazy.NoLazy);
    //                cm.Inverse(true);
    //            },
    //                    em =>
    //                        em.ManyToMany(m =>
    //                            m.Column(controllingColumnName))));
    //    }
    //}

    //private static void MapUniManyToMany<TControllingEntity, TInverseEntity>(ModelMapper mapper, PropertyInfo property)
    //    where TControllingEntity : class
    //    where TInverseEntity : class
    //{
    //    string singular;
    //    string[] typeNames;

    //    singular = new Inflector.Inflector(new CultureInfo("en")).Singularize(property.Name);
    //    if (singular == null)
    //        singular = property.Name;
    //    typeNames = property.DeclaringType?.FullName?.Split('.') ?? Array.Empty<string>();

    //    mapper.Class<TControllingEntity>(map =>
    //        map.Set<TInverseEntity>(property.Name, cm =>
    //        {
    //            cm.Schema(string.Format("Aegean{0}", typeNames[typeNames.Length - 2]));
    //            cm.Table(string.Format("tbl{0}{1}", property.DeclaringType?.Name, property.Name));
    //            cm.Key(km =>
    //            {
    //                km.Column("FKOwner");
    //                km.ForeignKey(string.Format("FKOwner_{0}{1}_{2}",
    //                            property.DeclaringType?.Name,
    //                            singular,
    //                            property.DeclaringType?.Name));
    //            });
    //            cm.Cascade(Cascade.Persist /*Cascade.All | Cascade.DeleteOrphans*/);
    //            /*cm.Lazy(CollectionLazy.NoLazy);*/
    //        },
    //            em =>
    //                em.ManyToMany(m =>
    //                {
    //                    m.Column("FKContent");
    //                    m.ForeignKey(string.Format("FKContent_{0}{1}_{2}",
    //                            property.DeclaringType?.Name,
    //                            singular,
    //                            property.PropertyType.GetGenericArguments()[0].Name));
    //                })));
    //}

    //private static bool isSelfReferencingObject(PropertyInfo property, PropertyInfo inverseProperty)
    //{
    //    return property.PropertyType.FullName == inverseProperty.PropertyType.FullName;
    //}

    //private static void MapSelfReferencingObject<TControllingEntity, TInverseEntity>(ModelMapper mapper, PropertyInfo property)
    //    where TControllingEntity : class
    //    where TInverseEntity : class
    //{
    //    var controllingPropertyName = property.DeclaringType.Name;
    //    var controllingColumnName = string.Format("{0}Id", controllingPropertyName);
    //    var inverseColumnName = string.Format("{0}Id", property.Name);
    //    var tableName = string.Format("{0}To{1}", controllingPropertyName, property.Name);

    //    mapper.Class<TControllingEntity>(map =>
    //        map.Bag<TInverseEntity>(property.Name, cm =>
    //    {
    //        cm.Cascade(Cascade.Persist /*Cascade.All | Cascade.DeleteOrphans*/);
    //        cm.Table(tableName);
    //        cm.Key(km =>
    //        {
    //            km.ForeignKey(string.Format("fk_{0}_{1}",
    //                    property.Name,
    //                    controllingColumnName));
    //            km.Column(controllingColumnName);
    //        });
    //        cm.Lazy(CollectionLazy.NoLazy);
    //    },
    //            em =>
    //                em.ManyToMany(m =>
    //                    m.Column(inverseColumnName))));
    //}
}
