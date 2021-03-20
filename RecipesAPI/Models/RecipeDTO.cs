#nullable enable
namespace RecipesAPI.Models
{
  public class RecipeDto
  {
    public RecipeDto(string name, string description, IngredientDto[] ingredients)
    {
      Name = name;
      Description = description;
      Ingredients = ingredients;
    }

    public long? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public IngredientDto[]? Ingredients { get; set; }
  }
}