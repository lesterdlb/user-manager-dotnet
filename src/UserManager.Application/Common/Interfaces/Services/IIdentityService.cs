namespace UserManager.Application.Common.Interfaces.Services;

public interface IIdentityService
{
    Task<List<string>> GetRoles();
}