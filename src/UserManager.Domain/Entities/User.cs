namespace UserManager.Domain.Entities;

public abstract class User
{
    public Guid Id { get; } = Guid.NewGuid();
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
    public Uri? ProfilePicture { get; init; }
}