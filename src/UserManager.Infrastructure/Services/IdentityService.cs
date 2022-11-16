using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManager.Infrastructure.Identity;
using ErrorOr;
using MapsterMapper;
using UserManager.Application.Common.Contracts.Authentication;
using UserManager.Application.Common.Interfaces.Authentication;
using UserManager.Domain.Common.Errors;

namespace UserManager.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IMapper _mapper;
    
    private const string UserRole = "User";

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

    public async Task<ErrorOr<RegisterResponse>> RegisterUserAsync(RegisterRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        var role = await _roleManager.FindByNameAsync(UserRole);

        if (user is not null)
            return Errors.User.DuplicateEmail;

        if (role is null)
            return Errors.Role.RoleNotFound;

        var newUser = _mapper.Map<ApplicationUser>(request);
        newUser.UserName = request.Email;

        var result = await _userManager.CreateAsync(newUser, request.Password);

        if (!result.Succeeded)
            return Errors.User.UserCouldNotBeCreated;

        await _userManager.AddToRoleAsync(newUser, role.Name);

        return new RegisterResponse(UserId: newUser.Id);
    }

    public async Task<ErrorOr<LoginResponse>> LoginUserAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
            return Errors.User.UserNotFound;

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);

        if (!result.Succeeded)
            return Errors.Authentication.InvalidCredentials;

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.UserName, user.Email);

        return new LoginResponse(
            Id: user.Id,
            Email: user.Email,
            FirstName: user.FirstName ?? string.Empty,
            LastName: user.LastName ?? string.Empty,
            Token: token);
    }

    public async Task<List<string>> GetRoles()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return roles.Select(x => x.Name).ToList();
    }
}