using Cooquoi.Core.Domain;
using Cooquoi.Core.Functional;

namespace Cooquoi.Domain.Entities;

public class Storage : EntityBase
{
    private readonly HashSet<StorageItem> _items = new ();

    public Storage(Guid id, string name) : base(id)
    {
        Name = name;
    }

    public string Name { get;  }
    public IReadOnlyCollection<StorageItem> Items => _items;

    public Result AddItem(Ingredient ingredient, decimal quantity)
    {
        if (_items.Any(i => i.Ingredient.Id != ingredient.Id))
        {
            var failure = new Failure
            {
                Code = "DomainError",
                Message = $"Cannot add ingredient ({ingredient.Name}) because it already exist in this storage",
                Data = new
                {
                    Entity = nameof(Storage),
                    EntityId = Id,
                    IngredientId = ingredient.Id
                }
            };
            return Result.Fail(new []{failure});
        }

        var item = new StorageItem(ingredient, quantity);
        _items.Add(item);
        return Result.Success();
    }
}

public record StorageItem
{
    public StorageItem(Ingredient ingredient, decimal quantity)
    {
        Ingredient = ingredient;
        Quantity = quantity;
    }

    public Ingredient Ingredient { get; }
    public decimal Quantity { get; }
}