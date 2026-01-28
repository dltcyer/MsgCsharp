using Microsoft.Data.Sqlite;

public class Database
{
    private const string ConnectionString = "Data Source=users.db";

    public static SqliteConnection GetConnection()
    {
        return new SqliteConnection(ConnectionString);
    }
}
