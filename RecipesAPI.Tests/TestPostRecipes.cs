using System;
using System.Linq;
using System.Threading.Tasks;
using RecipesAPI.Controllers;
using RecipesAPI.Models;
using Xunit;

namespace RecipesAPI.Tests
{
    
    [Collection("Database collection")]
    public class TestPostRecipes
    {
        private readonly DatabaseFixture _fixture;

        public TestPostRecipes(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }
        [Fact]
        public async Task PostRecipeWithoutIngredients()
        {
            var options = _fixture.options;
            var recipesContext = new RecipesContext(options);

            var controller = new RecipesController(recipesContext);
            var ingredientList = Array.Empty<IngredientDto>();

            var newRecipe = new RecipeDto("Test recipe", "Test description", ingredientList);
            await controller.PostRecipe(newRecipe);
            var addedRecipe = recipesContext.Recipes.Last();
            Assert.Equal("Test recipe", addedRecipe.Name);
            Assert.Equal("Test description", addedRecipe.Description);
            Assert.Empty(addedRecipe.Ingredients);
        }

        [Fact]
        public async Task PostRecipeWithIngredient()
        {
            var options = _fixture.options;
            var recipesContext = new RecipesContext(options);

            var controller = new RecipesController(recipesContext);
            var newIngredient = new IngredientDto("ui", 2, "unit");
            var ingredientList = new[] {newIngredient};

            var newRecipe = new RecipeDto("Test recipe", "Test description", ingredientList);
            await controller.PostRecipe(newRecipe);
            var addedRecipe = recipesContext.Recipes.LastOrDefault();
            Assert.Equal("Test recipe", addedRecipe.Name);
            Assert.Equal("Test description", addedRecipe.Description);
            var firstIngredient = addedRecipe.Ingredients[0];
            Assert.Equal("ui", firstIngredient.Name);
            Assert.Equal(2, firstIngredient.Amount);
            Assert.Equal("unit", firstIngredient.Unit);
        }
        [Fact]
        public async Task PostRecipeWithIngredients()
        {
            var options = _fixture.options;
            var recipesContext = new RecipesContext(options);

            var controller = new RecipesController(recipesContext);
            var newIngredient = new IngredientDto("ui", 2, "unit");
            var newIngredient2 = new IngredientDto("knoflook", 3, "unit");
            var ingredientList = new[] {newIngredient, newIngredient2};

            var newRecipe = new RecipeDto("Test recipe", "Test description", ingredientList);
            await controller.PostRecipe(newRecipe);
            var addedRecipe = recipesContext.Recipes.Last();
            Assert.Equal("Test recipe", addedRecipe.Name);
            Assert.Equal("Test description", addedRecipe.Description);
            Assert.Equal(2, addedRecipe.Ingredients.Count);
        }
    }
}