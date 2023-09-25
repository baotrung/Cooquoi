using Cooquoi.Application.Interfaces;
using Cooquoi.Domain.Business;
using Cooquoi.Domain.Enums;

namespace Cooquoi.Infrastructure.Services;

public class InMemoryIngredientRepository : IIngredientRepository
{
    private readonly List<Ingredient> _ingredients = new()
    {
        new Ingredient(Guid.NewGuid(), "Carrot", IngredientUnitType.Weight),
        new Ingredient(Guid.NewGuid(), "Entrecote", IngredientUnitType.Weight),
        new Ingredient(Guid.NewGuid(), "Whole Milk", IngredientUnitType.Volume),
        new Ingredient(Guid.NewGuid(), "Egg", IngredientUnitType.Piece),
    };
    
    public Task<Ingredient> GetIngredientById(Guid id)
    {
        var result = _ingredients.Single(i => i.Id == id);
        return Task.FromResult(result);
    }
}