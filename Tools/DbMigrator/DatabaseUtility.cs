using Npgsql;

namespace DbMigrator;
static class DatabaseUtility
{
    static string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
        ?? "Host=localhost;Username=postgres;Password=postgres;Port=5432;";
    private static readonly string databaseName = "chatdb";

    public static void CreateDatabase()
    {
        using var conn = new NpgsqlConnection(connectionString);
        conn.Open();

        using var cmd = new NpgsqlCommand
        {
            Connection = conn,
            CommandText = $"CREATE DATABASE {databaseName}"
        };

        try
        {
            cmd.ExecuteNonQuery();
            Console.WriteLine($"Database '{databaseName}' created successfully.");
        }
        catch (PostgresException ex) when (ex.SqlState == "42P04")
        {
            Console.WriteLine($"Database '{databaseName}' already exists.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred creating the database: {ex.Message}");
        }
    }

    public static void ExecuteSqlScript(string scriptFilePath)
    {
        var connectionStringWithDb = $"{connectionString}Database={databaseName};";

        string script = File.ReadAllText(scriptFilePath);

        using var conn = new NpgsqlConnection(connectionStringWithDb);
        conn.Open();

        using var cmd = new NpgsqlCommand
        {
            Connection = conn,
            CommandText = script
        };

        try
        {
            cmd.ExecuteNonQuery();
            Console.WriteLine($"Script '{scriptFilePath}' executed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred executing the script: {ex.Message}");
        }
    }
}