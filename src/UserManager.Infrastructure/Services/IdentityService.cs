using ErrorOr;

using MapsterMapper;

using Microsoft.AspNetCore.Identity;

using UserManager.Application.Common.Contracts.Authentication;
using UserManager.Application.Common.DTOs;
using UserManager.Application.Common.Interfaces.Authentication;
using UserManager.Domain.Common.Errors;
using UserManager.Infrastructure.Identity;

namespace UserManager.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IMapper _mapper;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtTokenGenerator jwtTokenGenerator,
        IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _mapper = mapper;
    }

    public async Task<bool> UserByEmailExistsAsync(string email)
        => await _userManager.FindByEmailAsync(email) is not null;

    public async Task<ErrorOr<RegisterResponse>> CreateUserAsync(
        RegisterRequest registerRequest, string password, string role)
    {
        var user = _mapper.Map<ApplicationUser>(registerRequest);
        user.UserName = registerRequest.Email;
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded) return Errors.User.UserCouldNotBeCreated;

        await _userManager.AddToRoleAsync(user, role);

        return _mapper.Map<RegisterResponse>(user);
    }

    public async Task<ErrorOr<LoginResponse>> LoginUserAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null) return Errors.Authentication.InvalidCredentials;

        var result = await _signInManager.PasswordSignInAsync(
            user, request.Password, false, false);

        if (!result.Succeeded) return Errors.Authentication.AuthenticationFailed;

        var roles = await _userManager.GetRolesAsync(user);

        var token = _jwtTokenGenerator
            .GenerateToken(new IdentityUserDto(
                user.Id,
                user.UserName!,
                user.Email!,
                roles));

        return new LoginResponse(
            user.Id,
            user.Email!,
            user.FirstName ?? string.Empty,
            user.LastName ?? string.Empty,
            token);
    }
}