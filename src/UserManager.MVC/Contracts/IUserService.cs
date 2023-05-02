using UserManager.MVC.Models.Users;
using UserManager.MVC.Services.Base;

namespace UserManager.MVC.Contracts;

public interface IUserService
{
    Task<List<UserViewModel>> GetUsers();
    Task<UserViewModel> GetUser(Guid id);
    Task<Response<Guid>> CreateUser(CreateUserViewModel role);
    Task<Response<Guid>> UpdateUser(UserViewModel role);
    Task<Response<Guid>> DeleteUser(Guid id);
}