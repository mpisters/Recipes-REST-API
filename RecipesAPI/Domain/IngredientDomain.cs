using System.Collections;
using System.Collections.Generic;
using RecipesAPI.Models;

namespace RecipesAPI.Domain
{
    public class IngredientDomain
    {
        private readonly DatabaseActions _databaseActions;

        public IngredientDomain(DatabaseActions databaseActions)
        {
            _databaseActions = databaseActions;
        }

        public Ingredient mapIngredientToDomain(CreatedIngredientDTO newIngredient)
        {
            return new Ingredient(newIngredient.Name, newIngredient.Amount, newIngredient.Unit);
        }

        public ICollection<Ingredient> createIngredientList(ICollection<CreatedIngredientDTO> newIngredients)
        {
            ICollection<Ingredient> ingredients = new List<Ingredient>();
            foreach (var ingredient in newIngredients)
            {
                var mappedIngredient = mapIngredientToDomain(ingredient);
                ingredients.Add(mappedIngredient);
            }
            _databaseActions.SaveIngredients(ingredients);
            return ingredients;
        }
        
    }
}