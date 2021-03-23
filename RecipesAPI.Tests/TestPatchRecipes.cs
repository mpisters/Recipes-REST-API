using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecipesAPI.Controllers;
using RecipesAPI.Models;
using Xunit;

namespace RecipesAPI.Tests
{
  public class TestPatchRecipes
  {
    
    [Fact]
    public async Task TestReturnsBadRequestWhenRecipeIdIsUnknown()
    {
      DatabaseFixture fixture = new DatabaseFixture();
      var options = fixture.options;
      var recipesContext = new RecipesContext(options);

      var controller = new RecipesController(recipesContext);
      var ingredientList = Array.Empty<IngredientDto>();

      var newRecipe = new RecipeDto("Updated recipe", "Updated description", ingredientList);
      var result = await controller.PatchRecipe(9999, newRecipe);
      Assert.IsType<BadRequestResult>(result.Result);
    }

    [Fact]
    public async Task TestReturnsNotFoundWhenRecipeIdIsNotFound()
    {
      DatabaseFixture fixture = new DatabaseFixture();
      var options = fixture.options;
      var recipesContext = new RecipesContext(options);

      var controller = new RecipesController(recipesContext);
      var ingredientList = Array.Empty<IngredientDto>();

      var newRecipe = new RecipeDto("Updated recipe", "Updated description", ingredientList) {Id = 9999};
      var result = await controller.PatchRecipe(9999, newRecipe);
      Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task TestUpdatesRecipeWithoutIngredients()
    {
      DatabaseFixture fixture = new DatabaseFixture();
      var options = fixture.options;
      var recipesContext = new RecipesContext(options);
      var emptyIngredientList = new List<Ingredient>();
      var existingIngredient = new Recipe
          {Description = "existing description", Name = "existing title", Ingredients = emptyIngredientList};
      await recipesContext.Recipes.AddAsync(existingIngredient);
      await recipesContext.SaveChangesAsync();
      var controller = new RecipesController(recipesContext);
      var ingredientList = Array.Empty<IngredientDto>();

      var newRecipe = new RecipeDto("Updated recipe", "Updated description", ingredientList) {Id = 1};
      newRecipe.Id = 1;
      await controller.PatchRecipe(1, newRecipe);
      var updatedRecipe = await recipesContext.Recipes.FindAsync((long) 1);
      Assert.Equal("Updated recipe", updatedRecipe.Name);
      Assert.Equal("Updated description", updatedRecipe.Description);
      Assert.Null(updatedRecipe.Ingredients);
    }

    [Fact]
    public async Task TestUpdatesRecipeWithNewIngredients()
    {
      DatabaseFixture fixture = new DatabaseFixture();
      var options = fixture.options;
      var recipesContext = new RecipesContext(options);

      // create existing recipe without ingredients
      var ingredientList = new IngredientDto[2];
      var ingredient1 = new IngredientDto("Ingredient1", 100, "gr");
      var ingredient2 = new IngredientDto("Ingredient2", 200, "kg");
      ingredientList[0] = ingredient1;
      ingredientList[1] = ingredient2;
      var emptyIngredientList = new List<Ingredient>();
      var existingRecipe = new Recipe
          {Description = "existing description", Name = "existing title", Ingredients = emptyIngredientList};
      await recipesContext.Recipes.AddAsync(existingRecipe);
      await recipesContext.SaveChangesAsync();
      var controller = new RecipesController(recipesContext);

      var newRecipe = new RecipeDto("Updated recipe", "Updated description", ingredientList) {Id = 2};
      await controller.PatchRecipe(2, newRecipe);
      var updatedRecipe = await  recipesContext.Recipes.FindAsync((long) 2);
      Assert.Equal("Updated recipe", updatedRecipe.Name);
      Assert.Equal("Updated description", updatedRecipe.Description);
      Assert.Equal(2, updatedRecipe.Ingredients.Count);
      Assert.Equal("Ingredient1", updatedRecipe.Ingredients[0].Name);
      Assert.Equal(100, updatedRecipe.Ingredients[0].Amount);
      Assert.Equal("gr", updatedRecipe.Ingredients[0].Unit);
      Assert.Equal("Ingredient2", updatedRecipe.Ingredients[1].Name);
      Assert.Equal(200, updatedRecipe.Ingredients[1].Amount);
      Assert.Equal("kg", updatedRecipe.Ingredients[1].Unit);
    }

    [Fact]
    public async Task TestUpdatesRecipeWithExistingIngredients()
    {
      DatabaseFixture fixture = new DatabaseFixture();
      var options = fixture.options;
      var recipesContext = new RecipesContext(options);

      // add existing ingredients to database
      var ingredientList = new List<Ingredient>();
      var existingIngredient1 = new Ingredient {Name = "Ingredient1", Amount = 100, Unit = "gr"};
      var existingIngredient2 = new Ingredient {Name = "Ingredient2", Amount = 200, Unit = "kg"};
      ingredientList.Add(existingIngredient1);
      ingredientList.Add(existingIngredient2);
      await recipesContext.Ingredients.AddAsync(existingIngredient1);
      await recipesContext.Ingredients.AddAsync(existingIngredient2);

      // have existing recipe with ingredients
      var existingRecipe = new Recipe
          {Description = "existing description", Name = "existing title", Ingredients = ingredientList};
      await recipesContext.Recipes.AddAsync(existingRecipe);
      await recipesContext.SaveChangesAsync();
      var controller = new RecipesController(recipesContext);


      var updatedIngredientList = new IngredientDto[2];
      var ingredient1 = new IngredientDto("Ingredient1 updated", 333, "gr") {Id = 1};
      var ingredient2 = new IngredientDto("Ingredient2 updated", 555, "kg") {Id = 2};
      updatedIngredientList[0] = ingredient1;
      updatedIngredientList[1] = ingredient2;
      var newRecipe = new RecipeDto("Updated recipe", "Updated description", updatedIngredientList) {Id = 1};
      await controller.PatchRecipe(1, newRecipe);
      var updatedRecipe = await recipesContext.Recipes.FindAsync((long) 1);
      Assert.Equal("Updated recipe", updatedRecipe.Name);
      Assert.Equal("Updated description", updatedRecipe.Description);
      Assert.Equal(2, updatedRecipe.Ingredients.Count);
      Assert.Equal("Ingredient1 updated", updatedRecipe.Ingredients[0].Name);
      Assert.Equal(333, updatedRecipe.Ingredients[0].Amount);
      Assert.Equal("gr", updatedRecipe.Ingredients[0].Unit);
      Assert.Equal("Ingredient2 updated", updatedRecipe.Ingredients[1].Name);
      Assert.Equal(555, updatedRecipe.Ingredients[1].Amount);
      Assert.Equal("kg", updatedRecipe.Ingredients[1].Unit);
    }
  }
}