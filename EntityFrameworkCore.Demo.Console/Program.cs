namespace EntityFrameworkCore.Demo.Console;

using EntityFrameworkCore.Data;
using System;
public class Program
{
    private static readonly FootBallDBContext footBallDBContext = new FootBallDBContext();

    static void Main(string[] args)
    {
        footBallDBContext.Leagues.Add(new Domain.League { Name = "Potti Perimier League" });
        footBallDBContext.SaveChangesAsync();

        Console.ReadLine();
    }
}