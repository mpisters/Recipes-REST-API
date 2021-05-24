using System.Collections.Generic;

#nullable enable
namespace RecipesAPI.Models
{
  public class CreatedRecipeDTO
  {
    public CreatedRecipeDTO(string name, string description, CreatedIngredientDTO[] ingredients)
    {
      Name = name;
      Description = description;
      Ingredients = ingredients;
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<CreatedIngredientDTO> Ingredients { get; set; }
  }

  public class UpdateRecipeDTO
  {
    public UpdatedRecipeDTO()
    {
    public long Id { get; set; }
    public string? Name{ get; set; }
    public string? Description { get; set; }
    public ICollection<UpdatedIngredientDTO>? Ingredients { get; set; }
    }
  }
}