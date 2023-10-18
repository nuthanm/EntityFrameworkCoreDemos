# Entity Framework Core for .NET Core API

This README provides a piece of essential information for developers looking to work with Entity Framework Core (EF Core) in a .NET Core API project. 
Whether you're just starting or experienced in knowing how to debug locally, this guide will help you get started with EF Core and streamline your development process.

## Table of Contents

- [Introduction](#introduction)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Configuration](#configuration)
- [Usage](#usage)
  - [CRUD Operations](#crud-operations)
  - [Transactions](#transactions)
  - [Efficient Record Updates](#efficient-record-updates)

## Introduction

Entity Framework Core is a powerful Object-Relational Mapping (ORM) framework for .NET Core applications. It simplifies database interactions and accelerates development, making it an ideal choice for building APIs.

## Getting Started
I am taking this readme as an example using .net core API, you can use the same process for any project in a .NET ecosystem like a console project.

### Prerequisites

Before you start working with EF Core, make sure you have the following prerequisites:

1. A .NET Core API project.
2. Entity Framework Core installed in your project.
   - Namespaces for efcore and database provider.
4. Choose your database provider: SQL Server, SQLite, or PostgreSQL.

### Configuration

To configure EF Core in your project, please follow these steps:

1. First, Create a DBContext file that will serve as the bridge between your application and the database.
   Define your entities and configure relationships, indexes, and primary keys.
2. In your `Startup.cs` or `Program.cs` file, configure this DBContext using the following steps but before there are other configurations we should set first:

   - **Step 1:** Define the necessary information in your `appsettings.json` file:

     ```json
     "DBContext": {
       "TimeoutInSeconds": 30,
       "EnableDetailedError": true,
       "EnableSensitiveDataLogging": true
     }
     ```
     **Purpose of this section** :
     - If we don't have this section if there is an error in the environment then we have to rebuild and deploy after the code changes.
     - If we have this configuration section we just change it here then no need to redeploy/rebuild the application
     - Flexible and easy to maintain
       
   - **Step 2:** Create a model class to hold these settings:

     ```csharp
     public class DBContextConfiguration
     {
        public int TimeoutInSeconds { get; set; }
        public bool EnableDetailedError { get; set; }
        public bool EnableSensitiveDataLogging { get; set; }
     }
     ```

   - **Step 3:** Access these `appsettings.json` file sections in your `Program.cs` or `Startup.cs`, initialize and configure the DBContext:

     ```csharp
     // This is one way of configuring the appsettings.json sections.
     var dbConfig = new DBContextConfiguration();
     builder.Configuration.GetSection("DbContext").Bind(dbConfig);

     builder.Services.AddDbContext<DBContext>(options => {
         options.UseSqlite(connectionString, action => {
              action.CommandTimeout(dbConfig.TimeoutInSeconds);
         });

         // The following line is used when there is an error while you working with entity framework core, this gives more detailed error information for tracing makes it easier.
         options.EnableDetailedErrors(dbConfig.EnableDetailedError);

         // The following line is used to know what values we are passing when there is a parameter to that query.
         options.EnableSensitiveDataLogging(dbConfig.EnableSensitiveDataLogging);
     });
     ```
     **Important point to remember:** All these settings are not good when you deploy these in a production environment.
     **Reason:** Information is exposed to the persons so with that information they can do anything in the system

3. In some situations there should be some pre-populated data into entities in that case, we create an object and configure this using the `HasData` method.
   - It always creates this pre-populated data when we perform migrations.
   **Example:**
   ```csharp
     public class Users
     {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
     }

     // In the DatabaseContext file,
     var user = new Users()
     {
       Id = 1,
       FirstName = "Nani",
       LastName = "Nuthan",
     };
     modelEntity.HasData(user);
     ```
   

## Usage

### CRUD Operations

With EF Core, you can easily perform CRUD (Create, Read, Update, Delete) operations on your database. 
Your DBContext will provide the necessary methods to interact with your entities.

### Transactions

You can use transactions in EF Core to ensure the integrity of your data. Use the following code to start a transaction and commit it:

```csharp
_dbContext.Database.BeginTransactionAsync();
// Your CRUD statements
_dbContext.Database.CommitTransactionAsync();
```
### Efficient Record Updates - Optimization
If you need to update a large number of records, EF Core provides a more efficient way than using a foreach loop. 
You can use the `ExecuteSqlInterpolatedAsync` method:

```
_dbContext.Database.ExecuteSqlInterpolatedAsync("<Write update/insert/delete>");
```

By following these steps and guidelines, you'll be able to work efficiently with Entity Framework Core in your .NET Core API project.
