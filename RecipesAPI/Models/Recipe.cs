using System;
using System.Collections.Generic;

namespace RecipesAPI.Models
{
  public class Recipe
  {
    public Recipe(string name, string description, ICollection<Ingredient> ingredients)
    {
      Name = name;
      Description = description;
      Ingredients = ingredients;
    }

    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public ICollection<Ingredient> Ingredients { get; set; }
  }
  
}