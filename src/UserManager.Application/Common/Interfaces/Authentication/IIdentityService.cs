﻿using ErrorOr;
using UserManager.Application.Common.Contracts.Authentication;
using UserManager.Application.Common.DTOs.Authentication;

namespace UserManager.Application.Common.Interfaces.Authentication;

public interface IIdentityService
{
    Task<bool> UserByEmailExistsAsync(string email);
    Task<bool> RoleExistsAsync(string name);
    Task<ErrorOr<RegisterResponse>> CreateUserAsync(RegisterRequest registerRequest, string password, string role);
    Task<ErrorOr<LoginResponse>> LoginUserAsync(LoginRequest request);
    Task<List<string>> GetRoles();
}