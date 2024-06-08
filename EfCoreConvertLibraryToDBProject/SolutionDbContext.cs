using Microsoft.EntityFrameworkCore;

namespace EfCoreConvertLibraryToDBProject
{
    // Step 1: Identify what are the Pre-Requisites for EF Core project
    // Nuget Packages: 2 packages required: Provider: Sqlserver/Inmemory/postgress and another one is Tools pacakge.
    // Step 2: <WhichClassToExtend> : DbContext
    public class SolutionDbContext : DbContext
    {

        // Step 3: How to map tables to entities, Here what is entities?8
        public DbSet<Sample> Samples { get; set; }

        // Step 4: How to configure db Connection string
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("sample"); // Here i used inmemorydatabase 
            // UseSQlServer("<ConnectionString");
        }


        // From the following classe(s), we set [Key] for both the classes to tell EF Core this column is primary key for this class.
        // Q) Which one is optional?
        // Q) If i want to achieve the same thing using Fluent Approach, how to do that?
        // Q) How to add proprety and it's maximum length to 255 using FLuent approach?
        // Q) How to map this class with actual table, which method is required to map?
        // Q) Which method we used to add Index()?
        // Q) Which method we used to add Constraint()?

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SampleVersion>(tablebuilder =>
            {
                tablebuilder.HasKey("SampleId"); // Alternative approach

                tablebuilder.Property("Name").HasMaxLength(255);

                tablebuilder.HasIndex("Name").IsUnique();

                // Multiple properties
                tablebuilder.HasIndex(i => new
                {
                    i.Name,
                    i.Description,
                }).IsUnique();

            });
            base.OnModelCreating(modelBuilder);
        }
    }
}

// For Sample Class/Entity: As per EFCore convention : ClassNameId should treat as Primary Key.

// For SampleVersion : // Here classNameSampleId is not matched with EF Core convention so we explictly configured either annotation or fluent approach

/*
 * Difference between IQueryable vs IEnumerable?
    * Query: dbContext.Sales; => What is the return tyep here? => IEnumerable
    * Query: dbContext.Sales.Count(); => What is the return type here? => IQueryable
    * Which option is very slow? => IEnumerble because we fetch all the data from DB and execute in memory.
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
// Ans: return type should be IQueryable<Sales>
//      return dbContext.Sales.Count;


// CodeSnippet(s):

// return dbContext.Sales.AsNoTracking().ToList();
// vs
// return dbContext.Sales.ToList();

// Here, what is the purpose of AsNoTracking();

// Q) What is the purpose of TagWith()
// return dbContext.Sales.TagWith("GetSales data").ToList();
// Where it helps?