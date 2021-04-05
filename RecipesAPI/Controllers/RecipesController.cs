using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipesAPI.Models;

namespace RecipesAPI.Controllers
{
    [Route("api/Recipes")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly RecipesContext _context;

        public RecipesController(RecipesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
        {
            return await _context.Recipes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipeById(long id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            return recipe;
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Recipe>> PatchRecipe(long id, RecipeDto updatedRecipe)
        {
            if (id != updatedRecipe.Id)
            {
                return BadRequest();
            }

            var currentRecipe = await _context.Recipes.FindAsync(id);

            if (currentRecipe == null)
            {
                return NotFound();
            }

            currentRecipe.Name = updatedRecipe.Name;
            currentRecipe.Description = updatedRecipe.Description;

            if (updatedRecipe.Ingredients != null && updatedRecipe.Ingredients.Length > 0)
            {
                var newIngredientList = new List<Ingredient>();
                foreach (var ingredient in updatedRecipe.Ingredients)
                {
                    if (ingredient.Id == null)
                    {
                        var newIngredient = await createNewIngredient(ingredient);
                        newIngredientList.Add(newIngredient);
                    }
                    else
                    {
                        var existingIngredient = await _context.Ingredients.FindAsync(ingredient.Id);
                        if (existingIngredient == null)
                        {
                            continue;
                        }

                        existingIngredient = await updateIngredient(existingIngredient, ingredient);
                        newIngredientList.Add(existingIngredient);
                    }
                }

                currentRecipe.Ingredients = newIngredientList;
            }

            _context.Recipes.Update(currentRecipe);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRecipeById), new {id = currentRecipe.Id}, currentRecipe);
        }

        [HttpPost]
        public async Task<ActionResult<Recipe>> PostRecipe(RecipeDto newRecipe)
        {
            var ingredients = newRecipe.Ingredients;
            var newIngredients = new List<Ingredient>();
            foreach (var ingredient in ingredients)
            {
                var newIngredient = await createNewIngredient(ingredient);
                newIngredients.Add(newIngredient);
            }

            var recipe = new Recipe
            {
                Name = newRecipe.Name,
                Description = newRecipe.Description,
                Ingredients = newIngredients
            };

            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRecipeById), new {id = recipe.Id}, recipe);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteRecipe(long id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool RecipeExists(long id)
        {
            return _context.Recipes.Any(item => item.Id == id);
        }
        
        private async Task<Ingredient> createNewIngredient(IngredientDto ingredient){
            var newIngredient = new Ingredient
            {
                Name = ingredient.Name,
                Amount = ingredient.Amount,
                Unit = ingredient.Unit
            };
            await _context.Ingredients.AddAsync(newIngredient);
            await _context.SaveChangesAsync();
            return newIngredient;
        }

        private async Task<Ingredient> updateIngredient(Ingredient ingredient, IngredientDto updatedIngredient)
        {
            ingredient.Name = updatedIngredient.Name;
            ingredient.Amount = updatedIngredient.Amount;
            ingredient.Unit = updatedIngredient.Unit;
            _context.Ingredients.Update(ingredient);
            return ingredient;
        }
    }
    

}