namespace Teatronik.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<IUser?> GetByIdAsync(Guid id);
        Task<IUser?> GetByEmailAsync(string email);
        Task CreateAsync(IUser user, string password);
        Task AddToRoleAsync(IUser user, string roleName);
    }
}
