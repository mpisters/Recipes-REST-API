using System;
using Microsoft.EntityFrameworkCore;
using RecipesAPI.Models;
using RecipesAPI.Tests;
using Xunit;

namespace RecipesAPI.Tests
{
    public class DatabaseFixture: IDisposable
    {
        public DatabaseFixture()
        {
            options = new DbContextOptionsBuilder<RecipesContext>()
                .UseInMemoryDatabase(databaseName: "TestRecipeDatabase")
                .Options;
        }
        
        public DbContextOptions<RecipesContext> options { get; set; }

        public void Dispose()
        {
            this.options = null;
        }
    }
}

[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}