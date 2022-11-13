namespace UserManager.Application.Common.Interfaces.Services;

public interface ICurrentUserService
{
    string? UserId { get; }
}