using ErrorOr;

namespace UserManager.Domain.Common.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error DuplicateEmail => Error.Conflict(
            code: "user.duplicate.email",
            description: "User with this email already exists");
    }
}