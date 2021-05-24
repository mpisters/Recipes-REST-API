using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        
        public async Task<int> UpdateRecipe(Recipe recipe)
        {
             _context.Recipes.Update(recipe);
            return await _context.SaveChangesAsync();
        }

        public async Task<ActionResult<Recipe>> GetRecipe(long id)
        {
            return await _context.Recipes.FindAsync(id);
        }
        
        public async Task<ActionResult> SaveIngredients(ICollection<Ingredient> ingredients)
        {
            await _context.Ingredients.AddRangeAsync(ingredients);
            await _context.SaveChangesAsync();
        }
    }
}