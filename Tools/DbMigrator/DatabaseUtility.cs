using Npgsql;

namespace DbMigrator;
static class DatabaseUtility
{
    public static string ConnectionString = string.Empty;
    private static readonly string databaseName = "chat";

    public static void CreateDatabase()
    {
        using var conn = new NpgsqlConnection(ConnectionString);
        conn.Open();

        using var cmd = new NpgsqlCommand
        {
            Connection = conn,
            CommandText = $"CREATE DATABASE {databaseName} WITH OWNER = postgres ENCODING = 'UTF8' TABLESPACE = pg_default CONNECTION LIMIT = -1;"
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
        var connectionStringWithDb = $"{ConnectionString}Database={databaseName};";

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