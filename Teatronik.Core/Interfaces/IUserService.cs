using Teatronik.Core.Common;
using Teatronik.Core.Models;

namespace Teatronik.Core.Interfaces
{
    public interface IUserService
    {
        Task<Result> AddAsync(User user);
        Task<Result> AddRole(User user, Role role);
        Task<Result> DeleteAsync(Guid id);
        Task<Result<List<User>>> GetAllAsync();
        Task<Result<List<User>>> GetByFilter(string? name = null, DateTime? fromDate = null, DateTime? toDate = null);
        Task<Result<User?>> GetByIdAsync(Guid id);
        Task<Result> RemoveRole(User user, Role role);
        Task<Result> UpdateAsync(User user);
    }
}
