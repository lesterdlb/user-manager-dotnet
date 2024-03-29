﻿using UserManager.Domain.Entities;

namespace UserManager.Application.Common.Interfaces.Repositories;

public interface IUserRepository : IAsyncRepository<User>
{
    Task<User> AddUserWithPasswordAsync(User user, string password);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> UserNameExistsAsync(string userName);
    Task AddUserToRoleAsync(Guid userId, string roleName);
    Task<IEnumerable<string>> GetUserRolesNamesAsync(Guid userId);
}