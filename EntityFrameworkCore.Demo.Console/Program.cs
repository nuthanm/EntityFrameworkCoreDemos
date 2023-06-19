namespace EntityFrameworkCore.Demo.Console;

using EntityFrameworkCore.Data;
using EntityFrameworkCore.Domain;
using System;
public class Program
{
    // Instance the object of DBContext file.
    private static readonly FootBallDBContext context = new FootBallDBContext();

    static void Main(string[] args)
    {
        // Approach 1: With out creating a league but it creates through Foriegn Key table: Teams
        ////////AskThisInInterview();

        // Approach 2: Create league record
        //context.<TableName>.Add
        //Here Add() == Database Insert command
        ////////CreateLeauge();

        // If you are using await then your method should add async
        // When you want to use the below code then replace our main with async in static async Task Main
        ////////await CreateLeagueAsync();

        //Add Teams information using League record
        ////////AddTeamsWithLeagueId(league);
        //footBallDBContext.SaveChanges();

        //Simple Select to get all records from League
        GetListOfLeagues();

        Console.ReadLine();
    }

    private static void GetListOfLeagues()
    {
        /*
                 * info: 19-06-2023 13:42:55.841 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
                   Executed DbCommand (39ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                   SELECT [l].[Id], [l].[Name]
                   FROM [Leagues] AS [l]

                   // Output
                    1 - Potti Perimier League            
                    2 - Nani Perimier League            
                    3 - Nani vs potti Perimier League            
                    4 - NP Perimier League            
                    5 - NP Perimier League            
                    6 - NP Perimier League            
                    7 - League information
                 */

        // The below statement connects to db and get all data and store it in leageus inmemory
        // so for each here get the data from inmemory
        //var leagues = context.Leagues.ToList();
        //foreach (var league in leagues)
        //{
        //    Console.WriteLine($"{league.Id} - {league.Name}");
        //}

        // Connected architecture
        // Until all records from the table you read this should connect with DB and locked the table.
        // Even this works but inefficient
        var leagues = context.Leagues;
        foreach (var league in leagues)
        {
            Console.WriteLine($"{league.Id} - {league.Name}");
        }
    }

    private static async Task CreateLeagueAsync()
    {
        await context.Leagues.AddAsync(new Domain.League { Name = "Nani Perimier League" });
        await context.SaveChangesAsync();
    }

    private static void CreateLeauge()
    {
        var league = new Domain.League { Name = "NP Perimier League" };
        context.Leagues.Add(league);
        context.SaveChanges();

        // Database query

        // In Log, the above code generates the follwing one,
        /*
         * info: 19-06-2023 00:17:22.105 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
           Executed DbCommand (61ms) [Parameters=[@p0='Nani Perimier League' (Nullable = false) (Size = 4000)], CommandType='Text', CommandTimeout='30']
           
            SET IMPLICIT_TRANSACTIONS OFF;
            SET NOCOUNT ON;
            INSERT INTO [Leagues] ([Name])
            OUTPUT INSERTED.[Id]
            VALUES (@p0);
         */

    }


    static void AddTeamsWithLeagueId(League league)
    {
        // Single Result
        CreateTeam(league);

        // List of Teams
        CreateListOfTeamsWithOutNavigationProperty(league);

        CreateListOfTeamsWithNavigationProperty(league);
    }

    private static void CreateListOfTeamsWithNavigationProperty(League league)
    {
        var listOfTeamsUsingNavigationProperty = new List<Domain.Team>
        {
            new Domain.Team
            {
                Name ="NLP1",
                LeagueId = league.Id,
            },
            new Domain.Team
            {
                Name="PLN_NavigationProperty",
                League=league,
            }

        };

        context.AddRange(listOfTeamsUsingNavigationProperty);

        // Database Query

        // Option 3: Add using Navigation Property
        // List of Teams
        /*
         * info: 19-06-2023 00:37:39.641 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
           Executed DbCommand (41ms) [Parameters=[@p0='NP Perimier League' (Nullable = false) (Size = 4000)], CommandType='Text', CommandTimeout='30']
                SET IMPLICIT_TRANSACTIONS OFF;
                SET NOCOUNT ON;
                INSERT INTO [Leagues] ([Name])
                OUTPUT INSERTED.[Id]
                VALUES (@p0);
           info: 19-06-2023 00:37:39.727 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
           Executed DbCommand (6ms) [Parameters=[@p0='6', @p1='NLP1' (Nullable = false) (Size = 4000), @p2='6', @p3='PLN_NavigationProperty' (Nullable = false) (Size = 4000)], CommandType='Text', CommandTimeout='30']
                SET IMPLICIT_TRANSACTIONS OFF;
                SET NOCOUNT ON;
                MERGE [Teams] USING (
                VALUES (@p0, @p1, 0),
                (@p2, @p3, 1)) AS i ([LeagueId], [Name], _Position) ON 1=0
                WHEN NOT MATCHED THEN
                INSERT ([LeagueId], [Name])
                VALUES (i.[LeagueId], i.[Name])
                OUTPUT INSERTED.[Id], i._Position;

         * 
         */
    }

    private static void CreateListOfTeamsWithOutNavigationProperty(League league)
    {
        var listOfTeams = new List<Domain.Team>
        {
            new Domain.Team
            {
                Name ="NLP",
                LeagueId = league.Id,
            },
            new Domain.Team
            {
                Name="PLN",
                LeagueId=league.Id,
            }

        };
        context.AddRange(listOfTeams);
    }

    private static void CreateTeam(League league)
    {
        var team = new Domain.Team { LeagueId = league.Id, Name = "NR" };
        context.Teams.Add(team);

        //Database Query

        /*
         * List of Teams Query generation
         * info: 19-06-2023 00:33:51.764 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
            Executed DbCommand (43ms) [Parameters=[@p0='NP Perimier League' (Nullable = false) (Size = 4000)], CommandType='Text', CommandTimeout='30']
                SET IMPLICIT_TRANSACTIONS OFF;
                SET NOCOUNT ON;
                INSERT INTO [Leagues] ([Name])
                OUTPUT INSERTED.[Id]
                VALUES (@p0);

          info: 19-06-2023 00:33:51.862 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
            Executed DbCommand (20ms) [Parameters=[@p0='5', @p1='NR' (Nullable = false) (Size = 4000), @p2='5', @p3='NLP' (Nullable = false) (Size = 4000), @p4='5', @p5='PLN' (Nullable = false) (Size = 4000)], CommandType='Text', CommandTimeout='30']
                SET IMPLICIT_TRANSACTIONS OFF;
                SET NOCOUNT ON;
                MERGE [Teams] USING (
                VALUES (@p0, @p1, 0),
                (@p2, @p3, 1),
                (@p4, @p5, 2)) AS i ([LeagueId], [Name], _Position) ON 1=0
                WHEN NOT MATCHED THEN
                INSERT ([LeagueId], [Name])
                VALUES (i.[LeagueId], i.[Name])
                OUTPUT INSERTED.[Id], i._Position;
         */

    }

    static void AskThisInInterview()
    {
        var league = new League { Name = "League information" };
        var team = new Domain.Team { Name = "Team", League = league };
        context.Add(team); // context decide it's Teams entity based out of object we are passing
        context.SaveChanges();

        // First creates league and it uses in Teams object
        // No error it throws
        /*
         * info: 19-06-2023 00:45:51.907 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
      Executed DbCommand (39ms) [Parameters=[@p0='League information' (Nullable = false) (Size = 4000)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [Leagues] ([Name])
      OUTPUT INSERTED.[Id]
      VALUES (@p0);
info: 19-06-2023 00:45:51.938 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
      Executed DbCommand (3ms) [Parameters=[@p1='7', @p2='Team' (Nullable = false) (Size = 4000)], CommandType='Text', CommandTimeout='30']
      SET IMPLICIT_TRANSACTIONS OFF;
      SET NOCOUNT ON;
      INSERT INTO [Teams] ([LeagueId], [Name])
      OUTPUT INSERTED.[Id]
      VALUES (@p1, @p2);

         */
    }
}
