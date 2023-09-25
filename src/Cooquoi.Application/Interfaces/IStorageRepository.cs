using Cooquoi.Domain.Business;

namespace Cooquoi.Application.Interfaces;

public interface IStorageRepository
{
    // getters
    Task<Storage> GetStorageById(Guid id);
    
    // actions
    Task AddNewStorage(Storage storage);
    Task SaveStorage(Storage storage);
}