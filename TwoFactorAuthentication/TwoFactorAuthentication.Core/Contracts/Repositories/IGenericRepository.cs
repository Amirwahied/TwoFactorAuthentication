
using TwoFactorAuthentication.Core.Models;

namespace TwoFactorAuthentication.Core.Contracts.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        //return immutable list
        Task<IReadOnlyList<T>> GetAsync();
        //return single record with id
        Task<T?> GetByIdAsync(Guid id);
        //create new record
        Task<int> CreateAsync(T entity,string procedureName);
        //update  record
        Task UpdateAsync(T entity);
        //delete record
        Task DeleteAsync(T entity);

    }
}
