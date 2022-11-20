using ErrorOr;

namespace UserManager.Domain.Common.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error UserCouldNotBeCreated => Error.Unexpected(
            code: "user.could.not.be.created",
            description: "User could not be created");

        public static Error UserNotFound => Error.Conflict(
            code: "user.not.found",
            description: "User not found");

        public static Error DuplicateEmail => Error.Conflict(
            code: "user.duplicate.email",
            description: "User with this email already exists");
    }
}