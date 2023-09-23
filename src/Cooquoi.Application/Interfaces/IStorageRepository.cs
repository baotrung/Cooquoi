using Cooquoi.Core.Functional;
using Cooquoi.Domain.Entities;

namespace Cooquoi.Application.Interfaces;

public interface IStorageRepository
{
    Task<Result> AddNewStorage(Storage storage);
}