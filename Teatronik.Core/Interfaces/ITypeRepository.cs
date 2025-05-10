namespace Teatronik.Core.Interfaces
{
    public interface ITypeRepository
    {
        public Task<List<Models.Type>> GetAllAsync();
        public Task<Models.Type?> GetByIdAsync(Guid id);
        public Task<List<Models.Type>> GetByNameAsync(string name);

        public Task AddAsync(Models.Type type);
        public Task DeleteAsync(Guid id);
        public Task UpdateAsync(Models.Type type);
    }
}
