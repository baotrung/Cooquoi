using Cooquoi.Application.Interfaces;
using Cooquoi.Domain.Business;

namespace Cooquoi.Infrastructure.Services;

public class InMemoryStorageRepository : IStorageRepository
{
    private readonly List<Storage> _storages = new();
    public Task AddNewStorage(Storage storage)
    {
        if (_storages.Any(s => s.Id == storage.Id))
        {
            throw new ArgumentException($"Storage with id {storage.Id} already exists");
        }
        _storages.Add(storage);
        return Task.CompletedTask;
    }

    public Task<Storage> GetStorageById(Guid id)
    {
        var storage = _storages.Single(s => s.Id == id);
        return Task.FromResult(storage);
    }

    public Task SaveStorage(Storage storage)
    {
        _storages.RemoveAll(s => s.Id == storage.Id);
        _storages.Add(storage);
        return Task.CompletedTask;
    }
}