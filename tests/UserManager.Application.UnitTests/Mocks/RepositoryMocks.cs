using Moq;

using UserManager.Application.Common.Interfaces.Repositories;
using UserManager.Domain.Entities;

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
        var roles = new List<Role>
        {
            new TestRole(Guid.Parse(AdminRoleId)) { Name = "Admin" },
            new TestRole(Guid.Parse(UserRoleId)) { Name = "User" },
            new TestRole(Guid.Parse(GuestRoleId)) { Name = "Guest" }
        };

        var mockRoleRepository = new Mock<IRoleRepository>();
        mockRoleRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(roles);

        mockRoleRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(
                (Guid roleId) =>
                {
                    return roles.FirstOrDefault(role => role.Id == roleId);
                });

        mockRoleRepository.Setup(
                repo => repo.AddAsync(It.IsAny<Role>()))
            .ReturnsAsync(
                (Role role) =>
                {
                    var newRole = new TestRole(Guid.Parse(NewRoleId)) { Name = role.Name };
                    roles.Add(newRole);
                    return newRole;
                });

        mockRoleRepository.Setup(
                repo => repo.UpdateAsync(It.IsAny<Role>()))
            .Callback(
                (Role role) =>
                {
                    var existingRole = roles.FirstOrDefault(r => r.Id == role.Id);
                    if (existingRole is null)
                    {
                        throw new ApplicationException();
                    }

                    existingRole.Name = role.Name;
                });

        mockRoleRepository.Setup(
                repo => repo.DeleteAsync(It.IsAny<Guid>()))
            .Callback(
                (Guid roleId) =>
                {
                    var existingRole = roles.FirstOrDefault(role => role.Id == roleId);
                    if (existingRole != null) roles.Remove(existingRole);
                });

        return mockRoleRepository;
    }
}