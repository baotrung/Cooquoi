using Cooquoi.Application.Interfaces;
using FluentValidation;
using MediatR;

namespace Cooquoi.Application.UseCases.Storage.Commands;

public static class CreateStorage
{
    public record Command(string StorageName) : IRequest<Guid>;
    
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.StorageName)
                .NotNull()
                .NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Command, Guid>
    {
        private readonly IStorageRepository _storageRepository;

        public Handler(IStorageRepository storageRepository)
        {
            _storageRepository = storageRepository;
        }

        public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
        {
            var storageName = request.StorageName;
            var storage = new Domain.Business.Storage(Guid.NewGuid(), storageName);
            await _storageRepository.AddNewStorage(storage);
            return storage.Id;
        }
    }
    
}

