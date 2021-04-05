using System.Collections.Generic;
using System.Threading.Tasks;
using RecipesAPI.Controllers;
using RecipesAPI.Models;
using Xunit;

namespace RecipesAPI.Tests
{
    [Collection("Database collection")]
    public class TestDeleteRecipes
    {
        private readonly DatabaseFixture _fixture;

        public TestDeleteRecipes(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task DeleteRecipeWithoutIngredients()
        {
            var options = _fixture.options;
            var recipesContext = new RecipesContext(options);

            var controller = new RecipesController(recipesContext);
            var emptyIngredientList = new List<Ingredient>();


            // add recipe to context
            var existingRecipe = new Recipe
            {
                Description = "existing description", Name = "existing title", Ingredients = emptyIngredientList, Id = 3
            };
            recipesContext.Recipes.Add(existingRecipe);

            await controller.DeleteRecipe(3);
            var deletedRecipe = await recipesContext.Recipes.FindAsync((long) 3);
            Assert.Null(deletedRecipe);
        }

        [Fact]
        public async Task DeleteRecipeWithIngredients()
        {
            var options = _fixture.options;
            var recipesContext = new RecipesContext(options);

            var controller = new RecipesController(recipesContext);

            // add existing ingredients to database
            var ingredientList = new List<Ingredient>();
            var existingIngredient1 = new Ingredient {Name = "Ingredient1", Amount = 100, Unit = "gr"};
            var existingIngredient2 = new Ingredient {Name = "Ingredient2", Amount = 200, Unit = "kg"};
            ingredientList.Add(existingIngredient1);
            ingredientList.Add(existingIngredient2);
            await recipesContext.Ingredients.AddAsync(existingIngredient1);
            await recipesContext.Ingredients.AddAsync(existingIngredient2);

            // add recipe to context
            var existingRecipe = new Recipe
            {
                Description = "existing description", Name = "existing title", Ingredients = ingredientList, Id = 3
            };
            recipesContext.Recipes.Add(existingRecipe);

            await controller.DeleteRecipe(3);
            var deletedRecipe = await recipesContext.Recipes.FindAsync((long) 3);
            Assert.Null(deletedRecipe);
            var ingredient1 = await recipesContext.Ingredients.FindAsync(existingIngredient1.Id);
            var ingredient2 = await recipesContext.Ingredients.FindAsync(existingIngredient2.Id);
            Assert.Null(ingredient1);
            Assert.Null(ingredient2);
        }
    }
}