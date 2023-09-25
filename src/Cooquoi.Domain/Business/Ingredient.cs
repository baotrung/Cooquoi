using Cooquoi.Core.Domain;
using Cooquoi.Domain.Enums;

namespace Cooquoi.Domain.Business;

public class Ingredient : EntityBase
{
    public Ingredient(Guid id, string name, IngredientUnitType type) : base(id)
    {
        Name = name;
        Type = type;
    }

    public string Name { get; }
    public IngredientUnitType Type { get; }
}