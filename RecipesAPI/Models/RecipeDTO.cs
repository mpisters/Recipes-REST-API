using System.Collections.Generic;

#nullable enable
namespace RecipesAPI.Models
{
    public class CreatedRecipeDto
    {
        public CreatedRecipeDto(string name, string description, CreatedIngredientDto[] ingredients)
        {
            Name = name;
            Description = description;
            Ingredients = ingredients;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<CreatedIngredientDto> Ingredients { get; set; }
    }

    public class UpdateRecipeDTO
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ICollection<UpdatedIngredientDto>? Ingredients { get; set; }
    }
}
