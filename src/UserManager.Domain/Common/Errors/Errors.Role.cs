using ErrorOr;

namespace UserManager.Domain.Common.Errors;

public static partial class Errors
{
    public static class Role
    {
        public static Error RoleNotFound => Error.NotFound(
            "role.not.found", "Role not found");
    }
}