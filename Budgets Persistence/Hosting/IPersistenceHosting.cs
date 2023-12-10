namespace Krypton.Budgets.Persistence.Hosting;

public interface IPersistenceHosting
{
    void ResetDB(string master, string dbname, string dbowner);
    string GetDDL();
    void RunMigrations();
    void DropDB(string master, string dbname, string dbowner);
}
