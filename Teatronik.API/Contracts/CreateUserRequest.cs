namespace Teatronik.API.Contracts
{
    public record CreateUserRequest(
        string FullName,
        string Email,
        string Password);
}