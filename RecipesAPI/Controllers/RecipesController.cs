using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipesAPI.Domain;
using RecipesAPI.Models;

namespace RecipesAPI.Controllers
{
    [Route("api/Recipes")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private RecipeDomain _recipeDomain;
        public RecipesController(RecipeDomain domain)
        {
            _recipeDomain = domain;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
        {
            return await _context.Recipes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipeById(long id)
        {
            var recipe = await _recipeDomain.GetRecipe(id);
            if (recipe == null)
            {
                return NotFound();
            }
            return recipe;
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Recipe>> PatchRecipe(long id, UpdatedIngredientDto updatedRecipe)
        {
            if (id != updatedRecipe.Id)
            {
                return BadRequest();
            }

            var currentRecipe = await _recipeDomain.GetRecipe(id);
            if (currentRecipe == null)
            {
                return NotFound();
            }

            var recipe = await _recipeDomain.UpdateRecipe(currentRecipe, updatedRecipe);
            return CreatedAtAction(nameof(GetRecipeById), recipe.Id, recipe);
        }

        [HttpPost]
        public async Task<ActionResult<Recipe>> PostRecipe(CreatedRecipeDto newCreatedRecipe)
        {
            var recipe = _recipeDomain.CreateRecipe(newCreatedRecipe);
            return CreatedAtAction(nameof(GetRecipeById), recipe.Id, recipe);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteRecipe(long id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            if (recipe.Ingredients.Count > 0)
            {
                foreach (var ingredient in recipe.Ingredients)
                {
                    _context.Ingredients.Remove(ingredient);
                }
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool RecipeExists(long id)
        {
            return _context.Recipes.Any(item => item.Id == id);
        }
        

        private async Task<Ingredient> updateIngredient(Ingredient ingredient, CreatedIngredientDto updatedCreatedIngredient)
        {
            ingredient.Name = updatedCreatedIngredient.Name;
            ingredient.Amount = updatedCreatedIngredient.Amount;
            ingredient.Unit = updatedCreatedIngredient.Unit;
            _context.Ingredients.Update(ingredient);
            return ingredient;
        }
    }
    

}