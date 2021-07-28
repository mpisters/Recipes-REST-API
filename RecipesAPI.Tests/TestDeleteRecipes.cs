using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecipesAPI.Controllers;
using RecipesAPI.Domain;
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
            var options = _fixture.Options;
            var recipesContext = new RecipesContext(options);
            var controller = CreateRecipesController(recipesContext);
            var emptyIngredientList = new List<Ingredient>();

            // add recipe to context
            var existingRecipe = new Recipe
            {
                Name = "existing title",
                Description = "existing description",
                Ingredients = emptyIngredientList,
                Id = 8
            };
            recipesContext.Recipes.Add(existingRecipe);

            var result = await controller.DeleteRecipe(8);

            Assert.IsType<NoContentResult>(result);

            var deletedRecipe = await recipesContext.Recipes.FindAsync((long) 8);
            Assert.Null(deletedRecipe);
        }

        // [Fact]
        // public async Task DeleteRecipeWithIngredients()
        // {
        //     // Todo delete ingredients on cascade
        //     var options = _fixture.Options;
        //     var recipesContext = new RecipesContext(options);
        //     var controller = CreateRecipesController(recipesContext);
        //
        //     // add existing ingredients to database
        //     var ingredientList = new List<Ingredient>();
        //     var existingIngredient1 = new Ingredient("Ingredient1", 100, "gr");
        //     var existingIngredient2 = new Ingredient("Ingredient2", 200, "kg");
        //     ingredientList.Add(existingIngredient1);
        //     ingredientList.Add(existingIngredient2);
        //     await recipesContext.Ingredients.AddAsync(existingIngredient1);
        //     await recipesContext.Ingredients.AddAsync(existingIngredient2);
        //
        //     // add recipe to context
        //     var existingRecipe = new Recipe
        //     {
        //         Name = "existing title",
        //         Description = "existing description",
        //         Ingredients = ingredientList,
        //         Id = 13
        //     };
        //     recipesContext.Recipes.Add(existingRecipe);
        //
        //     var result = await controller.DeleteRecipe(13);
        //     
        //     Assert.IsType<NoContentResult>(result);
        //
        //     
        //     var deletedRecipe = await recipesContext.Recipes.FindAsync((long) 13);
        //     Assert.Null(deletedRecipe);
        //     var ingredient1 = await recipesContext.Ingredients.FindAsync(existingIngredient1.Id);
        //     var ingredient2 = await recipesContext.Ingredients.FindAsync(existingIngredient2.Id);
        //     Assert.Null(ingredient1);
        //     Assert.Null(ingredient2);
        // }

        private RecipesController CreateRecipesController(RecipesContext recipesContext)
        {
            var databaseActions = new DatabaseActions(recipesContext);
            var ingredientDomain = new IngredientDomain(databaseActions);
            var recipesDomain = new RecipesDomain(databaseActions, ingredientDomain);
            return new RecipesController(recipesDomain);
        }
    }
}