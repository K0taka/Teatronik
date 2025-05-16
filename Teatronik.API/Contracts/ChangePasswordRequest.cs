namespace Teatronik.API.Contracts
{
    public record ChangePasswordRequest(
        string OldPassword,
        string NewPassword);
}