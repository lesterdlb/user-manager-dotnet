using Moq;

using UserManager.Application.Common.DTOs.Role;
using UserManager.Application.Common.Interfaces.Repositories;

namespace UserManager.Application.UnitTests.Mocks;

public class RepositoryMocks
{
    private const string AdminRoleId = "00000000-0000-0000-0000-000000000001";
    private const string UserRoleId = "00000000-0000-0000-0000-000000000002";
    private const string GuestRoleId = "00000000-0000-0000-0000-000000000003";
    private const string NewRoleId = "00000000-0000-0000-0000-000000000004";

    public static string GetUserRoleId() => AdminRoleId;
    public static string GetNewRoleId() => NewRoleId;

    public static Mock<IRoleRepository> GetRoleRepository()
    {
        var roles = new List<RoleDto>
        {
            new() { Id = AdminRoleId, Name = "Admin" },
            new() { Id = UserRoleId, Name = "User" },
            new() { Id = GuestRoleId, Name = "Guest" },
        };

        var mockRoleRepository = new Mock<IRoleRepository>();
        mockRoleRepository.Setup(repo => repo.GetRolesAsync())
            .ReturnsAsync(roles);

        mockRoleRepository.Setup(repo => repo.GetRoleAsync(It.IsAny<Guid>()))
            .ReturnsAsync(
                (Guid roleId) =>
                {
                    return roles.FirstOrDefault(role => Guid.Parse(role.Id) == roleId);
                });

        mockRoleRepository.Setup(
                repo => repo.CreateRoleAsync(It.IsAny<CreateRoleDto>()))
            .ReturnsAsync(
                (CreateRoleDto role) =>
                {
                    var newRole = new RoleDto { Id = NewRoleId, Name = role.Name };
                    roles.Add(newRole);
                    return newRole;
                });

        mockRoleRepository.Setup(
                repo => repo.UpdateRoleAsync(It.IsAny<RoleDto>()))
            .ReturnsAsync(
                (RoleDto role) =>
                {
                    var existingRole = roles.FirstOrDefault(r => r.Id == role.Id);
                    if (existingRole is null)
                    {
                        throw new ApplicationException();
                    }

                    existingRole.Name = role.Name;
                    return existingRole;
                });

        mockRoleRepository.Setup(
                repo => repo.DeleteRoleAsync(It.IsAny<Guid>()))
            .Callback(
                (Guid roleId) =>
                {
                    var existingRole = roles.FirstOrDefault(role => Guid.Parse(role.Id) == roleId);
                    if (existingRole != null) roles.Remove(existingRole);
                });

        return mockRoleRepository;
    }
}