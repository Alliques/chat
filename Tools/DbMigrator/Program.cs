
using Microsoft.Extensions.Configuration;
using DbMigrator;

var configuration = new ConfigurationBuilder()
                 .AddJsonFile($"appsettings.json", true, true);
var config = configuration.Build();

DatabaseUtility.ConnectionString = config.GetConnectionString("DefaultConnection");

Console.WriteLine("Starting migration...");

DatabaseUtility.CreateDatabase();

DatabaseUtility.ExecuteSqlScript(Path.Combine(AppContext.BaseDirectory, "Migrations", "001_CreateMessagesTable.sql"));

Console.WriteLine("Migration completed.");