using Cooquoi.Core.Domain;

namespace Cooquoi.Domain.Business;

public class Recipe : EntityBase
{
    private readonly HashSet<RecipeIngredient> _ingredients = new();
    public Recipe(Guid id, string name) : base(id)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException($"Recipe name cannot be empty", nameof(name));
        }
        Name = name;
    }
    
    public string Name { get; }
    
    public IReadOnlyCollection<RecipeIngredient> Ingredients => _ingredients;

    public void ClearIngredients()
    {
        _ingredients.Clear();
    }
    
    public void AddIngredients(Ingredient ingredient, decimal quantity, bool required)
    {
        if (_ingredients.Any(i => i.Ingredient.Id == ingredient.Id))
        {
            throw new ArgumentException($"Ingredient with id {ingredient.Id} is already exist in this {nameof(Recipe)}", nameof(ingredient));
        }

        _ingredients.Add(new RecipeIngredient(ingredient, quantity, required));
    }
}

public class RecipeIngredient : ValueObject
{
    public Ingredient Ingredient { get;  }
    public decimal Quantity { get;  }
    public bool Required { get;  }

    public RecipeIngredient(Ingredient ingredient, decimal quantity, bool required)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException($"{nameof(RecipeIngredient)} must have positive {nameof(quantity)}", nameof(quantity));
        }
        Ingredient = ingredient;
        Quantity = quantity;
        Required = required;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Ingredient.Id;
        yield return Quantity;
        yield return Required;
    }
}