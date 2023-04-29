using MapsterMapper;

using UserManager.MVC.Contracts;
using UserManager.MVC.Models.Roles;
using UserManager.MVC.Services.Base;

namespace UserManager.MVC.Services;

public class RoleService : BaseHttpService, IRoleService
{
    private readonly IMapper _mapper;

    public RoleService(
        ILocalStorageService localStorageService,
        IClient httpClient,
        IMapper mapper) : base(httpClient, localStorageService)
    {
        _mapper = mapper;
    }

    public async Task<List<RoleViewModel>> GetRoles()
    {
        AddBearerToken();
        var roles = await Client.RolesAllAsync();
        return _mapper.Map<List<RoleViewModel>>(roles);
    }

    public async Task<RoleViewModel> GetRole(Guid id)
    {
        AddBearerToken();
        var role = await Client.RolesGETAsync(id);
        return _mapper.Map<RoleViewModel>(role);
    }

    public async Task<Response<Guid>> CreateRole(CreateRoleViewModel role)
    {
        try
        {
            var createRoleCommand = _mapper.Map<CreateRoleCommand>(role);
            AddBearerToken();
            var result = await Client.RolesPOSTAsync(createRoleCommand);

            return new Response<Guid> { Success = true, Data = result };
        }
        catch (ApiException ex)
        {
            return ConvertApiException<Guid>(ex);
        }
    }

    public async Task<Response<Guid>> UpdateRole(RoleViewModel role)
    {
        try
        {
            var roleCommand = _mapper.Map<UpdateRoleCommand>(role);
            AddBearerToken();
            await Client.RolesPUTAsync(roleCommand);
            return new Response<Guid> { Success = true };
        }
        catch (ApiException ex)
        {
            return ConvertApiException<Guid>(ex);
        }
    }

    public async Task<Response<Guid>> DeleteRole(Guid id)
    {
        try
        {
            AddBearerToken();
            await Client.RolesDELETEAsync(id);
            return new Response<Guid> { Success = true };
        }
        catch (ApiException ex)
        {
            return ConvertApiException<Guid>(ex);
        }
    }
}