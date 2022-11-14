﻿using ErrorOr;
using UserManager.Contracts.Authentication;

namespace UserManager.Application.Common.Interfaces.Services;

public interface IIdentityService
{
    Task<ErrorOr<RegisterResponse>> RegisterUserAsync(RegisterRequest request);
    Task<List<string>> GetRoles();
}