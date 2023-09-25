using Cooquoi.Core.Domain;

namespace Cooquoi.Domain.Business;

public class Storage : EntityBase
{
    private readonly HashSet<StorageItem> _items = new ();

    public Storage(Guid id, string name) : base(id)
    {
        Name = name;
    }

    public string Name { get;  }
    public IReadOnlyCollection<StorageItem> Items => _items;

    public void UpdateItems(ICollection<(Ingredient,decimal)> items)
    {
        if (items.Select(i => i.Item1.Id).Distinct().Count() != items.Count())
        {
            throw new ArgumentException("Storage cannot contain multiple items with same ingredient");
        }
        if(items.Any(i => i.Item2 < 0))
        {
            throw new ArgumentException("Storage cannot contain items with negative quantity");
        }
        
        
        _items.Clear();
        foreach (var (ingredient, quantity) in items)
        {
            if (quantity > 0)
            {
                _items.Add(new StorageItem(ingredient, quantity));
            }
        }
    }
}

public class StorageItem : ValueObject
{
    public Ingredient Ingredient {get;}
    public decimal Quantity {get;}

    public StorageItem(Ingredient ingredient, decimal quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Storage Item cannot contain negative quantity");
        }

        Ingredient = ingredient;
        Quantity = quantity;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Ingredient.Id;
        yield return Quantity;
    }
}