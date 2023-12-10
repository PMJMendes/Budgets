using System.Reflection;

namespace Krypton.Budgets.Tests.Utils;

public abstract class CommonTestBase
{
    private static readonly Dictionary<(string Namespace, string TypeName), (Type Type, Dictionary<string, PropertyInfo?> Props)> Types = new();
    private static readonly object _lock = new();

    protected static Type FindType(Assembly assembly, string spaceName, string typeName)
    {
        return EnsureType(assembly, spaceName, typeName).Type;
    }

    protected static Type FindType(Assembly assembly, string spaceName, Type intfType)
    {
        return EnsureType(assembly, spaceName, intfType).Type;
    }

    protected static PropertyInfo FindProperty(Assembly assembly, string spaceName, string typeName, string propName)
    {
        var typeInfo = EnsureType(assembly, spaceName, typeName);

        lock (typeInfo.Props)
        {
            if (!typeInfo.Props.ContainsKey(propName))
            {
                typeInfo.Props[propName] = typeInfo.Type.GetProperties(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(p => p.Name == propName);
            }

            return typeInfo.Props[propName] ?? throw new Exception($"Property {propName} not found in {typeName} of module {spaceName}");
        }
    }

    private static (Type Type, Dictionary<string, PropertyInfo?> Props) EnsureType(Assembly assembly, string spaceName, string typeName)
    {
        var key = (spaceName, typeName);

        lock (_lock)
        {
            if (!Types.ContainsKey(key))
            {
                var type = assembly.GetTypes().
                    FirstOrDefault(t => (t.Namespace?.StartsWith(spaceName) ?? false) &&
                        t.Name == typeName) ??
                    throw new Exception($"Type {typeName} not found in {spaceName}");

                Types[key] = (type, new());
            }

            return Types[key];
        }
    }

    private static (Type Type, Dictionary<string, PropertyInfo?> Props) EnsureType(Assembly assembly, string spaceName, Type intfType)
    {
        var key = (spaceName, typeName: intfType.Name);

        lock (_lock)
        {
            if (!Types.ContainsKey(key))
            {
                var type = assembly.GetTypes().
                    FirstOrDefault(t => (t.Namespace?.StartsWith(spaceName) ?? false) &&
                        t.IsAssignableTo(intfType)) ??
                    throw new Exception($"No implementations for {key.typeName} found in {spaceName}");

                Types[key] = (type, new());
            }

            return Types[key];
        }
    }
}
