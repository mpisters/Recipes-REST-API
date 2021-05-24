using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecipesAPI.Models;

namespace RecipesAPI.Domain
{
    public class RecipeDomain
    {
        private readonly DatabaseActions _databaseActions;
        private  readonly IngredientDomain _ingredientDomain;

        public RecipeDomain(DatabaseActions databaseActions, IngredientDomain ingredientDomain)
        {
            _databaseActions = databaseActions;
            _ingredientDomain = ingredientDomain;
        }

        public async Task<ActionResult<Recipe>> updateRecipe(Recipe currentRecipe, UpdateRecipeDTO updatedRecipe)
        {
            if (updatedRecipe.Name != null)
            {
                currentRecipe.Name = updatedRecipe.Name;
            }

            if (updatedRecipe.Description != null)
            {
                currentRecipe.Description = updatedRecipe.Description;
            }
            
            // ToDo update Ingredients
             if (updatedRecipe.Ingredients != null)
            {
               
            }

            await _databaseActions.UpdateRecipe(currentRecipe);
            return currentRecipe;

        }
        {
            
        }

        public async Task<ActionResult<Recipe>> GetRecipe(long id)
        {
            return await _databaseActions.GetRecipe(id);
        }

        public async Task<ActionResult<Recipe>> createRecipe(CreatedRecipeDTO newCreatedRecipe)
        {
            var ingredients = _ingredientDomain.createIngredientList(newCreatedRecipe.Ingredients);
            var recipe = new Recipe(newCreatedRecipe.Name, newCreatedRecipe.Description, ingredients);
            await _databaseActions.CreateRecipe(recipe);
            return recipe;
        }
    }
}