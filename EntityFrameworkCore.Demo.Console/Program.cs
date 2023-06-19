namespace EntityFrameworkCore.Demo.Console;

using EntityFrameworkCore.Data;
using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;

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
        ////////GetListOfLeagues();

        // Get leagueNames
        ////////GetFilteredLeagueNames();

        // Get leagueNames based out of user entered
        //Console.Write("Enter League Information which you are looking for? : ");
        //var leguageNameToFilter = Console.ReadLine();
        ////////GetFilteredLeagueNames(leguageNameToFilter);


        //Get first record from the result set
        ////////GetSingleResultSet();

        // Checks whether record exist or not and it returns bool
        ////////RecordExistsUsingAny();

        // Check whether data is there in the table
        ////////GetDataIfThereUsingFindAsync();

        // Acheive the same thing using Method syntax
        ////////GetListOfLeagueNamesUsingMethodSyntax();

        // Traditional way of updting the record
        ////////UpdateTheRecordUsingTraditionalWay();


        // If we pass Id then if record exists then it updates or else insert
        // If Id itself not pass then it inserts the record even if we use Update method
        ////////TeamUpdate_else_Insert();

        // Delete the record from an entity
        // Delete data when there is a relationship
        // Same statement if we delete the record from main table then it deletes that record in child table too.
        SimpleDeleteEntryFromEntity();

        

        Console.ReadLine();
    }

    private static void SimpleDeleteEntryFromEntity()
    {
        var deleteDuplicateRecord = context.Leagues.Find(7);
        context.Leagues.Remove(deleteDuplicateRecord);
        context.SaveChanges();

        /*
         * info: 19-06-2023 20:05:55.490 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
           Executed DbCommand (56ms) [Parameters=[@__p_0='4'], CommandType='Text', CommandTimeout='30']
           SELECT TOP(1) [l].[Id], [l].[Name]
           FROM [Leagues] AS [l]
           WHERE [l].[Id] = @__p_0

           info: 19-06-2023 20:05:55.680 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
           Executed DbCommand (9ms) [Parameters=[@p0='4'], CommandType='Text', CommandTimeout='30']
           SET IMPLICIT_TRANSACTIONS OFF;
           SET NOCOUNT ON;
           DELETE FROM [Leagues]
           OUTPUT 1
           WHERE [Id] = @p0;
         */
    }

    private static void TeamUpdate_else_Insert()
    {
        // Scenario 1: Passed valid ID in teams
        var team = new Team
        {
            Id = 1,
            Name = "Test",
            LeagueId = 7
        };
        context.Teams.Update(team);
        context.SaveChanges();

        var updatedTeam = context.Teams.Find(1);
        Console.WriteLine($"Updated team name: {updatedTeam.Name}");

        /*
         * info: 19-06-2023 19:52:53.482 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
           Executed DbCommand (60ms) [Parameters=[@p2='1', @p0='7', @p1='Test' (Nullable = false) (Size = 4000)], CommandType='Text', CommandTimeout='30']
           SET IMPLICIT_TRANSACTIONS OFF;
           SET NOCOUNT ON;
           UPDATE [Teams] SET [LeagueId] = @p0, [Name] = @p1
           OUTPUT 1
           WHERE [Id] = @p2;

        Output:
        Updated team name: Test
         */

        // Scenario 2: Passed invalid ID in teams
        // Exception: The database operation was expected to affect 1 row(s), but actually affected 0 row(s); data may have been modified or deleted since entities were loaded. See http://go.microsoft.com/fwlink/?LinkId=527962 for information on understanding and handling optimistic concurrency exceptions.
        ////team = new Team
        ////{
        ////    Id = 11,
        ////    Name = "Test",
        ////    LeagueId = 7
        ////};
        ////context.Teams.Update(team);
        ////context.SaveChanges();

        ////var teamInfo = context.Teams.Find(1);
        ////Console.WriteLine($"Updated team name: {teamInfo.Name}");


        // Scenario 3: No Id in team object
        team = new Team
        {
            Name = "Test insertion 1",
            LeagueId = 7
        };
        context.Teams.Update(team);
        context.SaveChanges();

        var teamInfo = context.Teams.Find(team.Id);
        Console.WriteLine($"Updated team name: {teamInfo.Name}");

        /*
         * info: 19-06-2023 19:56:48.127 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
           Executed DbCommand (7ms) [Parameters=[@p0='7', @p1='Test insertion' (Nullable = false) (Size = 4000)], CommandType='Text', CommandTimeout='30']
            SET IMPLICIT_TRANSACTIONS OFF;
            SET NOCOUNT ON;
            INSERT INTO [Teams] ([LeagueId], [Name])
            OUTPUT INSERTED.[Id]
            VALUES (@p0, @p1);

            Output:
            Updated team name: Test
         */
    }

    private static void UpdateTheRecordUsingTraditionalWay()
    {
        // Get the record
        var league = context.Leagues.Find(7);

        // Modify the column value
        if (league is not null)
        {
            Console.WriteLine($"Current League Name (Before Update):{league.Name}");
            league.Name = "First ever league";

            // Update it
            context.SaveChanges();
        }

        var updatedRecord = context.Leagues.Find(7);
        Console.Write($"Updated League Name:{updatedRecord.Name}");


        /*
         * info: 19-06-2023 19:41:24.228 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
           Executed DbCommand (93ms) [Parameters=[@__p_0='7'], CommandType='Text', CommandTimeout='30']
           SELECT TOP(1) [l].[Id], [l].[Name]
           FROM [Leagues] AS [l]
           WHERE [l].[Id] = @__p_0
      
      Output:
      Current League Name (Before Update):League information
        
          info: 19-06-2023 19:41:24.477 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
          Executed DbCommand (7ms) [Parameters=[@p1='7', @p0='First ever league' (Nullable = false) (Size = 4000)], CommandType='Text', CommandTimeout='30']
          SET IMPLICIT_TRANSACTIONS OFF;
          SET NOCOUNT ON;
          UPDATE [Leagues] SET [Name] = @p0
          OUTPUT 1
          WHERE [Id] = @p1;

      Output:
      Updated League Name:First ever league

         * 
         */
    }

    private static void GetListOfLeagueNamesUsingMethodSyntax()
    {
        var filter = "NP";
        var listOfLeagueNamesUsingMethodSyntax = (from league in context.Leagues
                                                  where EF.Functions.Like(league.Name, $"{filter}%")
                                                  select league).ToList();

        foreach (var league in listOfLeagueNamesUsingMethodSyntax)
        {
            Console.WriteLine(league.Name);
        }

        /*
         * info: 19-06-2023 18:34:37.125 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
           Executed DbCommand (47ms) [Parameters=[@__Format_1='NP%' (Size = 4000)], CommandType='Text', CommandTimeout='30']
            SELECT [l].[Id], [l].[Name]
            FROM [Leagues] AS [l]
            WHERE [l].[Name] LIKE @__Format_1

            Output:
            NP Perimier League
            NP Perimier League
            NP Perimier League
         */
    }

    private static void GetDataIfThereUsingFindAsync()
    {
        var leagueData = context.Leagues.FindAsync(7);
        Console.WriteLine(leagueData.Result?.Name);

        /*
         * info: 19-06-2023 18:25:46.730 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
            Executed DbCommand (56ms) [Parameters=[@__p_0='7'], CommandType='Text', CommandTimeout='30']
            SELECT TOP(1) [l].[Id], [l].[Name]
            FROM [Leagues] AS [l]
            WHERE [l].[Id] = @__p_0

            Output:
            League information
         * 
         */
    }

    private static void RecordExistsUsingAny()
    {
        var isRecordExists = context.Leagues.Any(l => l.Name.Contains("NPL"));
        Console.WriteLine($"Requested items are available? : {isRecordExists}");

        /*
         * info: 19-06-2023 18:17:03.357 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
           Executed DbCommand (43ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
            SELECT CASE
                WHEN EXISTS (
                    SELECT 1
                    FROM [Leagues] AS [l]
                    WHERE [l].[Name] LIKE N'%NP%') THEN CAST(1 AS bit)
                ELSE CAST(0 AS bit)
            END
            
            Output:
            Requested items are available? : True
         */

        // For False record as Input:NPL
        /*
         * info: 19-06-2023 18:18:04.339 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
           Executed DbCommand (30ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
            SELECT CASE
                WHEN EXISTS (
                    SELECT 1
                    FROM [Leagues] AS [l]
                    WHERE [l].[Name] LIKE N'%NPL%') THEN CAST(1 AS bit)
                ELSE CAST(0 AS bit)
            END
            
            Output:
            Requested items are available? : False
         */
    }

    private static void GetSingleResultSet()
    {
        var record = context.Leagues.Where(x => x.Name.Contains("NP")).FirstOrDefault();
        Console.WriteLine("This record is with Where: " + record?.Name);
        /*
         * info: 19-06-2023 18:07:08.650 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
           Executed DbCommand (32ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
            SELECT TOP(1) [l].[Id], [l].[Name]
            FROM [Leagues] AS [l]
            WHERE [l].[Name] LIKE N'%NP%'
            
            Output:
            This record is with Where: NP Perimier League
        * 
         */
        var recordWithOutWhere = context.Leagues.FirstOrDefault(x => x.Name.Contains("NP"));
        Console.WriteLine("This record is without Where: " + recordWithOutWhere?.Name);

        /*
         * info: 19-06-2023 18:07:08.819 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
            Executed DbCommand (2ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
                SELECT TOP(1) [l].[Id], [l].[Name]
                FROM [Leagues] AS [l]
                WHERE [l].[Name] LIKE N'%NP%'
            
            Output:
            This record is without Where: NP Perimier League

         */

        // First vs Single => First record from multiple records but Single by default we get one record in resultset
    }

    private static void GetFilteredLeagueNames()
    {
        var leagueNames = context.Leagues.Where(league => league.Name == "NP Perimier League").ToList();
        foreach (var league in leagueNames)
        {
            Console.WriteLine(league.Name);
        }

        /*
         * info: 19-06-2023 14:45:57.586 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
           Executed DbCommand (34ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
            SELECT [l].[Id], [l].[Name]
            FROM [Leagues] AS [l]
            WHERE [l].[Name] = N'NP Perimier League'
            
            Output:
            NP Perimier League
            NP Perimier League
            NP Perimier League
         */
    }

    private static void GetFilteredLeagueNames(string? filter)
    {
        var exactLeagueNames = context.Leagues.Where(league => league.Name == filter).ToList();
        foreach (var league in exactLeagueNames)
        {
            Console.WriteLine(league.Name);
        }

        /*
         * Enter League Information which you are looking for? : League Information
           info: 19-06-2023 14:48:29.287 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
           Executed DbCommand (48ms) [Parameters=[@__filter_0='League Information' (Size = 4000)], CommandType='Text', CommandTimeout='30']
            SELECT [l].[Id], [l].[Name]
            FROM [Leagues] AS [l]
            WHERE [l].[Name] = @__filter_0

            Output:
            League information
         */

        // Override value
        filter = "NP";
        var partialLeagueNames = context.Leagues.Where(league => league.Name.Contains(filter)).ToList();
        foreach (var league in partialLeagueNames)
        {
            Console.WriteLine(league.Name);
        }

        /*
         * info: 19-06-2023 17:53:06.794 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
           Executed DbCommand (3ms) [Parameters=[@__filter_0='NP' (Size = 4000)], CommandType='Text', CommandTimeout='30']
            SELECT [l].[Id], [l].[Name]
            FROM [Leagues] AS [l]
            WHERE (@__filter_0 LIKE N'') OR CHARINDEX(@__filter_0, [l].[Name]) > 0
            
            Output:
            NP Perimier League            
            NP Perimier League            
            NP Perimier League

         */

        // Acheive the above result using EF Functions
        filter = "NP";
        var partialLeagueNamesUsingEFFUnctions = context.Leagues.Where(league => EF.Functions.Like(league.Name, $"{filter}%")).ToList();
        foreach (var league in partialLeagueNamesUsingEFFUnctions)
        {
            Console.WriteLine(league.Name);
        }

        /*
         * info: 19-06-2023 17:58:04.795 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
            Executed DbCommand (1ms) [Parameters=[@__Format_1='NP%' (Size = 4000)], CommandType='Text', CommandTimeout='30']
                SELECT [l].[Id], [l].[Name]
                FROM [Leagues] AS [l]
                WHERE [l].[Name] LIKE @__Format_1
            Output:
            NP Perimier League
            NP Perimier League
            NP Perimier League
         */
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
