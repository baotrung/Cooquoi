using Cooquoi.Application.Interfaces;
using Cooquoi.Domain.Business;
using FluentValidation;
using MediatR;

namespace Cooquoi.Application.UseCases.Storage.Commands;

public static class UpdateStorageItems
{
    public record Command(
        Guid StorageId, 
        ICollection<StorageItemDto>  StorageItems) 
        : IRequest;
    public record StorageItemDto(
        Guid IngredientId, 
        decimal Quantity);
    
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.StorageId)
                .NotNull()
                .NotEmpty();
            
            RuleFor(c => c.StorageItems)
                .NotNull()
                .Must(items => items.Select(i => i.IngredientId).Distinct().Count() == items.Count)
                .WithMessage("item list must not contain duplicate ingredients");
            
            RuleForEach(c => c.StorageItems)
                .Must(i => i.Quantity > 0)
                .WithMessage("quantity must be greater than 0");
        }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly IStorageRepository _storageRepository;
        private readonly IIngredientRepository _ingredientRepository;

        public Handler(IStorageRepository storageRepository, IIngredientRepository ingredientRepository)
        {
            _storageRepository = storageRepository;
            _ingredientRepository = ingredientRepository;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var (storageId, itemDtos) = request;
            var storage = await _storageRepository.GetStorageById(storageId);
            var items = await ConvertToValueTuples(itemDtos);
            storage.UpdateItems(items);
            await _storageRepository.SaveStorage(storage);
        }

        private async Task<HashSet<(Ingredient, decimal)>> ConvertToValueTuples(IEnumerable<StorageItemDto> itemDtos)
        {
            var items = new HashSet<(Ingredient, decimal)>();
            foreach (var i in itemDtos)
            {
                var ingredient = await _ingredientRepository.GetIngredientById(i.IngredientId);
                items.Add((ingredient, i.Quantity));
            }

            return items;
        }
    }
}