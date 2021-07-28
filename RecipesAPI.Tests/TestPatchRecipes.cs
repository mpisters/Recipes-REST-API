using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecipesAPI.Controllers;
using RecipesAPI.Domain;
using RecipesAPI.Models;
using Xunit;

namespace RecipesAPI.Tests
{
  [Collection("Database collection")]
  public class TestPatchRecipes
  {
    private readonly DatabaseFixture _fixture;

    public TestPatchRecipes(DatabaseFixture fixture)
    {
      _fixture = fixture;
    }
    
    [Fact]
    public async Task TestReturnsBadRequestWhenRecipeIdIsUnknown()
    {
      var options = _fixture.Options;
      var recipesContext = new RecipesContext(options);
      var controller = CreateRecipesController(recipesContext);
      var ingredientList = new List<UpdatedIngredientDto>();
      var unknownRecipe = new UpdatedRecipeDto("Updated recipe", "Updated description", ingredientList, 8888);
      
      var result = await controller.PatchRecipe(9999, unknownRecipe);
      
      Assert.IsType<BadRequestResult>(result.Result);
    }

    [Fact]
    public async Task TestReturnsNotFoundWhenRecipeIdIsNotFound()
    {
      var options = _fixture.Options;
      var recipesContext = new RecipesContext(options);
      var controller = CreateRecipesController(recipesContext);
      var ingredientList = new List<UpdatedIngredientDto>();
      var newRecipe = new UpdatedRecipeDto("Updated recipe", "Updated description", ingredientList, 9999);
      
      var result = await controller.PatchRecipe(9999, newRecipe);
      var error = result.Result;
      
      Assert.IsType<NotFoundResult>(error);
    }
    
    [Fact]
    public async Task TestUpdatesRecipeWithoutIngredients()
    {
      var options = _fixture.Options;
      var recipesContext = new RecipesContext(options);
      var controller = CreateRecipesController(recipesContext);
      var emptyIngredientList = new List<Ingredient>();
      var existingRecipe = new Recipe
      {
        Name = "existing title",
        Description = "existing description",
        Ingredients = emptyIngredientList,
        Id = 9
      };
      await recipesContext.Recipes.AddAsync(existingRecipe);
      await recipesContext.SaveChangesAsync();
    
      var emptyList = new List<UpdatedIngredientDto>();
      var newRecipe = new UpdatedRecipeDto("Updated recipe", "Updated description", emptyList, 9);
      
      var result = await controller.PatchRecipe(9, newRecipe);

      Assert.IsType<ActionResult<Recipe>>(result);
      Assert.Equal("Updated recipe", result.Value.Name);
      Assert.Equal("Updated description", result.Value.Description);
      Assert.Empty(result.Value.Ingredients);
    }
    
    [Fact]
    public async Task TestUpdatesRecipeWithNewIngredients()
    {
      var options = _fixture.Options;
      var recipesContext = new RecipesContext(options);
      var controller = CreateRecipesController(recipesContext);
      // create existing recipe without ingredients
    
      var emptyIngredientList = new List<Ingredient>();
      var existingRecipe = new Recipe
      {
        Name = "existing title",
        Description = "existing description",
        Ingredients = emptyIngredientList,
        Id = 8
      };
      await recipesContext.Recipes.AddAsync(existingRecipe);
      await recipesContext.SaveChangesAsync();
    
      var ingredientList = new List<UpdatedIngredientDto>();
      var ingredient1 = new UpdatedIngredientDto("Ingredient1", 100, "gr");
      var ingredient2 = new UpdatedIngredientDto("Ingredient2", 200, "kg");
      ingredientList.Add(ingredient1);
      ingredientList.Add(ingredient2);
      var newRecipe = new UpdatedRecipeDto("Updated recipe", "Updated description", ingredientList, 8);
      
      var result = await controller.PatchRecipe(8, newRecipe);
      Assert.IsType<ActionResult<Recipe>>(result);
      Assert.Equal("Updated recipe", result.Value.Name);
      Assert.Equal("Updated description", result.Value.Description);
      Assert.NotNull(result.Value.Ingredients);
      Assert.Equal(2, result.Value.Ingredients.Count);
      Assert.Equal("Ingredient1", result.Value.Ingredients.ToList()[0].Name);
      Assert.Equal(100, result.Value.Ingredients.ToList()[0].Amount);
      Assert.Equal("gr", result.Value.Ingredients.ToList()[0].Unit);
      Assert.Equal("Ingredient2", result.Value.Ingredients.ToList()[1].Name);
      Assert.Equal(200, result.Value.Ingredients.ToList()[1].Amount);
      Assert.Equal("kg", result.Value.Ingredients.ToList()[1].Unit);
    }
    
    [Fact]
    public async Task TestUpdatesRecipeWithExistingIngredients()
    {
      var options = _fixture.Options;
      var recipesContext = new RecipesContext(options);
      var controller = CreateRecipesController(recipesContext);
    
      // add existing ingredients to database
      var ingredientList = new List<Ingredient>();
      var existingIngredient1 = new Ingredient("Ingredient1",100,  "gr") {Id = 6};
      var existingIngredient2 = new Ingredient("Ingredient2",  200, "kg"){ Id = 7};
      ingredientList.Add(existingIngredient1);
      ingredientList.Add(existingIngredient2);
      await recipesContext.Ingredients.AddAsync(existingIngredient1);
      await recipesContext.Ingredients.AddAsync(existingIngredient2);
    
      // have existing recipe with ingredients
      var existingRecipe = new Recipe
      {
        Name = "existing title",
        Description = "existing description",
        Ingredients = ingredientList,
        Id = 12
      };
      await recipesContext.Recipes.AddAsync(existingRecipe);
      await recipesContext.SaveChangesAsync();
    
    
      var updatedIngredientList = new List<UpdatedIngredientDto>();
      var ingredient1 = new UpdatedIngredientDto("Ingredient1 updated", 333, "gr") {Id = 6};
      var ingredient2 = new UpdatedIngredientDto("Ingredient2 updated", 555, "kg") {Id = 7};

      updatedIngredientList.Add(ingredient1);
      updatedIngredientList.Add(ingredient2);
      var newRecipe = new UpdatedRecipeDto("Updated recipe", "Updated description", updatedIngredientList, 12);
      
      var result = await controller.PatchRecipe(12, newRecipe);
      Assert.IsType<ActionResult<Recipe>>(result);
      Assert.Equal("Updated recipe", result.Value.Name);
      Assert.Equal("Updated description", result.Value.Description);
      Assert.Equal(2, result.Value.Ingredients.Count);
      Assert.Equal("Ingredient1 updated", result.Value.Ingredients.ToList()[0].Name);
      Assert.Equal(333, result.Value.Ingredients.ToList()[0].Amount);
      Assert.Equal("gr", result.Value.Ingredients.ToList()[0].Unit);
      Assert.Equal("Ingredient2 updated", result.Value.Ingredients.ToList()[1].Name);
      Assert.Equal(555, result.Value.Ingredients.ToList()[1].Amount);
      Assert.Equal("kg", result.Value.Ingredients.ToList()[1].Unit);
    }
    
    private RecipesController CreateRecipesController(RecipesContext recipesContext)
    {
      var databaseActions = new DatabaseActions(recipesContext);
      var ingredientDomain = new IngredientDomain(databaseActions);
      var recipesDomain = new RecipesDomain(databaseActions, ingredientDomain);
      return new RecipesController(recipesDomain);
    }
  }
}