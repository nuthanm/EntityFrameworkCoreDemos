using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EntityFrameworkCore.Data;
public class FootBallDBContext : DbContext
{

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // No change in base configure
        // base.OnConfiguring(optionsBuilder);
        // Add sql server database connection string 
        // (localdb)\\MSSQLLocalDB => Local Visual Studio SQL Server Object Explore
        // Initial Catalog = Name of the database   
        optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FootballLeague_EfCore;Encrypt=False")
            .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
            .EnableSensitiveDataLogging();

        /*Note:  Add Encrypt to Connection String
        Due to a change in Entity Framework Core 7, you will get the following error when attempting to scaffold.
        A connection was successfully established with the server, but then an error occurred during the login process. (provider: SSL Provider, error: 0 - The certificate chain was issued by an authority that is not trusted.)
        
        Solution
        Add Encrypt = False to the connection string.
        */
    }

    // DBContext is the main class we should inherit from EFCore library to connect and configure database
    // This is nothing but databse connection class

    // Teams = TableName
    // Serialize to Team class from Tables: Teams
    public DbSet<Team> Teams { get; set; }

    // Leagues = TableName
    // Serialize to League class from Tables: Leagues
    public DbSet<League> Leagues { get; set; }
}