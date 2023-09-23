using Cooquoi.Application.Interfaces;
using Cooquoi.Core.Functional;
using FluentValidation;
using MediatR;

namespace Cooquoi.Application.UseCases.Storage.Commands;

public static class CreateStorage
{
    public record Command(string StorageName) : IRequest<Result<Guid>>;
    
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.StorageName).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Command, Result<Guid>>
    {
        private readonly IStorageRepository _storageRepository;

        public Handler(IStorageRepository storageRepository)
        {
            _storageRepository = storageRepository;
        }

        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            var storageName = request.StorageName;
            var storage = new Domain.Entities.Storage(Guid.NewGuid(), storageName);
            var res = await _storageRepository.AddNewStorage(storage);
            return !res.IsSuccess 
                ? Result<Guid>.Fail(res.Failures) 
                : Result<Guid>.Success(storage.Id);
        }
    }
    
}

