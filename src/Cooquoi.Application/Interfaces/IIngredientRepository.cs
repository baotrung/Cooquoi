using Cooquoi.Domain.Business;

namespace Cooquoi.Application.Interfaces;

public interface IIngredientRepository
{
    Task<Ingredient> GetIngredientById(Guid id);
}