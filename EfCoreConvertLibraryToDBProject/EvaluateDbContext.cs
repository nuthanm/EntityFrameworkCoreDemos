﻿namespace EfCoreConvertLibraryToDBProject
{
    // Step 1: Identify what are the Pre-Requisites for EF Core project
    // Step 2: <WhichClassToExtend>
    public class EvaluateDbContext
    {

    }

    // Step 3: How to map tables to entities, Here what is entities?

    // Step 4: How to configure db Connection string


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

