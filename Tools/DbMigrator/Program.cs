
using DbMigrator;

Console.WriteLine("Starting migration...");

// Create the database
DatabaseUtility.CreateDatabase();

// Execute the SQL script to create the table
DatabaseUtility.ExecuteSqlScript(Path.Combine(AppContext.BaseDirectory, "Migrations", "CreateMessagesTable.sql"));

Console.WriteLine("Migration completed.");