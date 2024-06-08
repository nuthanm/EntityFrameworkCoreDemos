using System.ComponentModel.DataAnnotations;
// From the following classe(s), we set [Key] for both the classes to tell EF Core this column is primary key for this class.
// Q) Which one is optional?
// Q) If i want to achieve the same thing using Fluent Approach, how to do that?
// Q) How to add proprety and it's maximum length to 255 using FLuent approach?
// Q) How to map this class with actual table, which method is required to map?
// Q) Which method we used to add Index()?
// Q) Which method we used to add Constraint()?

public class Sample
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }
}



/*
 * Difference between IQueryable vs IEnumerable?
    * Query: dbContext.Sales; => What is the return tyep here?
    * Query: dbContext.Sales.Count(); => What is the return type here?
    * Which option is very slow?
 * 
 * 
 */

// Code snippet(s): 

// Option 1: return dbContext.Sales.ToList().Count;
// Option 2: IEnumerable<Sales> sales = dbContext.Sales;
//           return sales.Count();
// Option 3: return GetSales().Count(); 
//           private IEnumerable<Sales> GetSales(){ return dbContext.Sales;}

// Do you see any difference in all options in terms of execution?
// If yes, what could be the issue with the above options and how to resolve it?

// CodeSnippet(s):

// return dbContext.Sales.AsNoTracking().ToList();
// vs
// return dbContext.Sales.ToList();

// Here, what is the purpose of AsNoTracking();

// Q) What is the purpose of TagWith()
// return dbContext.Sales.TagWith("GetSales data").ToList();
// Where it helps?

