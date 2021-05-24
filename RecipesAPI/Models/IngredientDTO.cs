#nullable enable
namespace RecipesAPI.Models
{
  public class CreatedIngredientDTO
  {
    public CreatedIngredientDTO(string name, int amount, string unit)
    {
      Name = name;
      Amount = amount;
      Unit = unit;
    }
    
    public string Name { get; set; }
    public int Amount { get; set; }
    public string Unit { get; set; }
  }
  
   public class UpdatedIngredientDTO
    {
      public long Id { get; set; }
      
      public string? Name { get; set; }
      public int? Amount { get; set; }
      public string? Unit { get; set; }
    }
}