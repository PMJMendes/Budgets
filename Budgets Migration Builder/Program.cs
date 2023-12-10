using Krypton.Budgets.Domain._Ports.Hosting;
using Krypton.Budgets.Logging.Hosting;
using Krypton.Budgets.Persistence.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var config = new ConfigurationBuilder().
    AddJsonFile("appsettings.json", true, true).
    Build();

var srv = new ServiceCollection().AddSingleton<IConfiguration>(config).
    AddLogging(conf => conf.ClearProviders()).
    AddBudgetsDomain().
    AddBudgetsPersistence().
    AddBudgetsLogging(config.GetSection("Logging")).
    BuildServiceProvider();

var logger = srv.GetService<ILogger<Program>>() ?? throw new Exception("Could not find logger for output");

if (srv.GetService<IPersistenceHosting>() is IPersistenceHosting persist)
{
    var master = config["Persistence:ConnectionStringNoDb"] ?? "";
    var dbname = config["Persistence:EmptyDBName"] ?? "";
    var dbowner = config["Persistence:EmptyDBOwner"] ?? "";

    persist.ResetDB(master, dbname, dbowner);
    persist.RunMigrations();

    var ddl = persist.GetDDL();
    logger.LogInformation("{DDL}", !string.IsNullOrWhiteSpace(ddl) ? ddl : "Migration Builder: Nothing to do...");

    persist.DropDB(master, dbname, dbowner);
}
else
{
    logger.LogCritical("Migration Builder: Could not get persistence service");
}
