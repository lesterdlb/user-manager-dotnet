using ErrorOr;

namespace UserManager.Domain.Common.Errors;

public static partial class Errors
{
    public static class Authentication
    {
        public static Error InvalidCredentials => Error.Validation(
            code: "invalid.credentials",
            description: "Invalid credentials");

        public static Error InvalidToken => Error.Validation(
            code: "invalid.token",
            description: "Invalid token");
    }
}