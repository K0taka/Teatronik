using Teatronik.Core.Common;
using Teatronik.Core.Models;

namespace Teatronik.Core.Interfaces
{
    public interface IEventService
    {
        Task<Result> AddAsync(Event ev);
        Task<Result> AddProp(Event ev, Prop prop);
        Task<Result> DeleteAsync(Guid id);
        Task<Result<List<Event>>> GetAllAsync();
        Task<Result<List<Event>>> GetByFilterAsync(string? name = null, DateTime? fromDate = null, DateTime? toDate = null);
        Task<Result<Event?>> GetByIdAsync(Guid id);
        Task<Result> RemoveProp(Event ev, Prop prop);
        Task<Result> UpdateAsync(Event ev);
    }
}