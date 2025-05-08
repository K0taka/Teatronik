using Teatronik.Core.Common;
using Teatronik.Core.Enums;

namespace Teatronik.Core.Models
{
    public partial class User
    {
        public const int MAX_NAME_LENGTH = 150;
        public const int MAX_EMAIL_LENGTH = 100;
        public const int MAX_PASSWORD_HASH = 60;
        public const int MIN_PASSWORD_LENGTH = 8;

        public Guid Id { get; }
        public string FullName { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime RegistrationDate { get; }
        public List<RoleType> Roles { get; private set; } = [];

        private User(Guid id, string fullName, string email, string passwordHash, DateTime registrationDate)
        {
            Id = id;
            FullName = fullName;
            Email = email;
            PasswordHash = passwordHash;
            RegistrationDate = registrationDate;
        }

        public static Result<User> Create(string fullName, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < MIN_PASSWORD_LENGTH)
                return Result<User>.Fail("Password must be at least 8 characters");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            return Initialize(Guid.NewGuid(), fullName, email, passwordHash, DateTime.UtcNow);
        }

        public static Result<User> Initialize(Guid id, string fullName, string email, string passwordHash, DateTime dateTime)
        {
            if (Guid.Empty.Equals(id))
                return Result<User>.Fail("id must be inited");

            if (string.IsNullOrWhiteSpace(fullName))
                return Result<User>.Fail("Full name is required");

            if (fullName.Length > MAX_NAME_LENGTH)
                return Result<User>.Fail($"Name must be less than {MAX_NAME_LENGTH}");

            if (email.Length > MAX_EMAIL_LENGTH)
                return Result<User>.Fail($"Email must be less than {MAX_EMAIL_LENGTH}");

            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
                return Result<User>.Fail("Invalid email");

            return Result<User>.Ok(new(
                id,
                fullName.Trim(),
                email.Trim().ToLower(),
                passwordHash,
                dateTime));
        }

        public Result ChangePassword(string oldPassword, string newPassword)
        {
            if (!VerifyPassword(oldPassword))
                return Result.Fail("Old password didn't match");


            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < MIN_PASSWORD_LENGTH)
                return Result.Fail("New password must be at least 8 characters");
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            return Result.Ok();
        }

        public Result UpdateDetails(string newFullName, string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newFullName))
                return Result.Fail("Full name is required");

            if (string.IsNullOrWhiteSpace(newEmail) || !IsValidEmail(newEmail))
                return Result.Fail("Invalid email");

            if (newFullName.Length > MAX_NAME_LENGTH)
                return Result<User>.Fail($"Name must be less than {MAX_NAME_LENGTH}");

            if (newEmail.Length > MAX_EMAIL_LENGTH)
                return Result<User>.Fail($"Email must be less than {MAX_EMAIL_LENGTH}");


            FullName = newFullName;
            Email = newEmail;
            return Result.Ok();
        }

        public Result UpdateName(string newName) => UpdateDetails(newName, Email);
        public Result UpdateEmail(string newEmail) => UpdateDetails(FullName, newEmail);

        public bool VerifyPassword(string password) =>
            !PasswordHash.Equals(string.Empty) && BCrypt.Net.BCrypt.Verify(password, PasswordHash);

        public Result AssignRole(RoleType role)
        {
            if (!Roles.Contains(role))
            {
                Roles.Add(role);
                return Result.Ok();
            }
            return Result.Fail("Role already assigned");
        }

        public Result RemoveRole(RoleType role)
        {
            var result = Roles.Remove(role);
            return result ? Result.Ok() : Result.Fail("Role not found");
        }

        private static bool IsValidEmail(string email) =>
            EmailRegex().IsMatch(email);
        
        
        [System.Text.RegularExpressions.GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
        private static partial System.Text.RegularExpressions.Regex EmailRegex();
    }
}
