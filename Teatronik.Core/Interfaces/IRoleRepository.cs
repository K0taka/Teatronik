namespace Teatronik.Core.Interfaces.Identity
{
    public interface IRoleRepository
    {
        Task<IRole?> FindByNameAsync(string roleName);
        Task CreateAsync(IRole role);
    }
}
