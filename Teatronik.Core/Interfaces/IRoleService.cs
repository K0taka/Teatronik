using Teatronik.Core.Common;
using Teatronik.Core.Models;

namespace Teatronik.Core.Interfaces
{
    public interface IRoleService
    {
        Task<Result<List<Role>>> GetAllAsync();
        Task<Result<Role?>> GetById(int id);
    }
}