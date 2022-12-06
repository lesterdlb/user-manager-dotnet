using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManager.Infrastructure.Identity;
using MapsterMapper;
using UserManager.Application.Common.Contracts.Authentication;
using UserManager.Application.Common.DTOs.Authentication;
using UserManager.Application.Common.Interfaces.Authentication;

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

    public async Task<bool> RoleExistsAsync(string name)
        => await _roleManager.RoleExistsAsync(name);


    public async Task<UserDto?> CreateUserAsync(RegisterRequest registerRequest, string password, string role)
    {
        var user = _mapper.Map<ApplicationUser>(registerRequest);
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            return null;

        await _userManager.AddToRoleAsync(user, role);

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> LoginUserAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        var result = await _signInManager.PasswordSignInAsync(
            user, request.Password, false, false);

        if (!result.Succeeded)
            return null;

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.UserName, user.Email);

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName ?? string.Empty,
            LastName = user.LastName ?? string.Empty,
            Token = token
        };
    }

    public async Task<List<string>> GetRoles()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return roles.Select(x => x.Name).ToList();
    }
}