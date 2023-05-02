using MapsterMapper;

using UserManager.MVC.Contracts;
using UserManager.MVC.Models.Users;
using UserManager.MVC.Services.Base;

namespace UserManager.MVC.Services;

public class UserService : BaseHttpService, IUserService
{
    private readonly IMapper _mapper;

    public UserService(
        ILocalStorageService localStorageService,
        IClient httpClient,
        IMapper mapper) : base(httpClient, localStorageService)
    {
        _mapper = mapper;
    }

    public async Task<List<UserViewModel>> GetUsers()
    {
        AddBearerToken();
        var users = await Client.UsersAllAsync();
        return _mapper.Map<List<UserViewModel>>(users);
    }

    public async Task<UserViewModel> GetUser(Guid id)
    {
        AddBearerToken();
        var user = await Client.UsersGETAsync(id);
        return _mapper.Map<UserViewModel>(user);
    }

    public async Task<Response<Guid>> CreateUser(CreateUserViewModel user)
    {
        try
        {
            var createUserCommand = _mapper.Map<CreateUserCommand>(user);
            AddBearerToken();
            var result = await Client.UsersPOSTAsync(createUserCommand);

            return new Response<Guid> { Success = true, Data = result };
        }
        catch (ApiException ex)
        {
            return ConvertApiException<Guid>(ex);
        }
    }

    public Task<Response<Guid>> UpdateUser(UserViewModel user)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<Guid>> DeleteUser(Guid id)
    {
        try
        {
            AddBearerToken();
            await Client.UsersDELETEAsync(id);
            return new Response<Guid> { Success = true };
        }
        catch (ApiException ex)
        {
            return ConvertApiException<Guid>(ex);
        }
    }
}