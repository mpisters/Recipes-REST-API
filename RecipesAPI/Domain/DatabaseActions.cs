using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipesAPI.Models;

namespace RecipesAPI.Domain
{
    public class DatabaseActions
    {
        private RecipesContext _context;

        public DatabaseActions(RecipesContext context)
        {
            _context = context;
        }

        public async Task<int> CreateRecipe(Recipe recipe)
        {
            await _context.Recipes.AddAsync(recipe);
            return await _context.SaveChangesAsync();
        }

        public async Task<ActionResult<ICollection<Ingredient>>> getIngredients()
        {
            return await _context.Recipes.ToListAsync();
        }

        public async Task<int> UpdateRecipe(Recipe recipe)
        {
            _context.Recipes.Update(recipe);
            return await _context.SaveChangesAsync();
        }

        public async Task<ActionResult<Recipe>> GetRecipe(long id)
        {
            return await _context.Recipes.FindAsync(id);
        }

        public async void SaveIngredients(ICollection<Ingredient> ingredients)
        {
            await _context.Ingredients.AddRangeAsync(ingredients);
            await _context.SaveChangesAsync();
            return;
        }
        
        public async Task<int> updateIngredient(Ingredient ingredient)
        {
            _context.Ingredients.Update(ingredient);
            return await _context.SaveChangesAsync();
        }

        public async  Task<ActionResult<Ingredient>> GetIngredient(long id)
        {
            return await _context.Ingredients.FindAsync(id);
        }
    }
}