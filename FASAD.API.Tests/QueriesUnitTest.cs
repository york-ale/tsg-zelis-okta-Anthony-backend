using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;

public class QueriesTests
{
    [Fact]
    public async Task GetUsers_ReturnsMappedUserDtos()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();

        var users = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                ExternalId = "ext-1",
                Email = "user1@example.com",
                RoleId = Guid.NewGuid(),
                Role = new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    Description = "Administrator"
                }
            },
            new User
            {
                Id = Guid.NewGuid(),
                ExternalId = "ext-2",
                Email = "user2@example.com",
                RoleId = Guid.NewGuid(),
                Role = null // No role assigned
            }
        };

        mockUserService.Setup(s => s.GetUsers()).ReturnsAsync(users);

        var queries = new Queries();

        // Act
        var result = await queries.GetUsers(mockUserService.Object);

        // Assert
        var dtoList = result.ToList();
        Assert.Equal(2, dtoList.Count);
        Assert.Equal("Admin", dtoList[0].RoleName);
        Assert.Equal("", dtoList[1].RoleName);
    }

    [Fact]
    public async Task GetRoles_ReturnsMappedRoleDtos()
    {
        // Arrange
        var mockRoleService = new Mock<IRoleService>();

        var roles = new List<Role>
        {
            new Role
            {
                Id = Guid.NewGuid(),
                Name = "Admin",
                Description = "Administrator"
            },
            new Role
            {
                Id = Guid.NewGuid(),
                Name = "User",
                Description = "Standard user"
            }
        };

        mockRoleService.Setup(s => s.GetRoles()).ReturnsAsync(roles);

        var queries = new Queries();

        // Act
        var result = await queries.GetRoles(mockRoleService.Object);

        // Assert
        var dtoList = result.ToList();
        Assert.Equal(2, dtoList.Count);
        Assert.Contains(dtoList, r => r.Name == "Admin");
        Assert.Contains(dtoList, r => r.Name == "User");
    }

    [Fact]
    public async Task GetSecurityEvents_ReturnsMappedSecurityEventDtos()
    {
        // Arrange
        var mockAuditService = new Mock<IAuditService>();

        var securityEvents = new List<SecurityEvent>
        {
            new SecurityEvent
            {
                Id = Guid.NewGuid(),
                EventType = "LoginSuccess",
                OccurredUtc = DateTime.UtcNow,
                Details = "User logged in",
                AuthorUserId = Guid.NewGuid(),
                AffectedUserId = Guid.NewGuid(),
                AuthorUser = new User
                {
                    Id = Guid.NewGuid(),
                    ExternalId = "ext-1",
                    Email = "author@example.com",
                    RoleId = Guid.NewGuid()
                },
                AffectedUser = new User
                {
                    Id = Guid.NewGuid(),
                    ExternalId = "ext-2",
                    Email = "affected@example.com",
                    RoleId = Guid.NewGuid()
                }
            },
            new SecurityEvent
            {
                Id = Guid.NewGuid(),
                EventType = "RoleChanged",
                OccurredUtc = DateTime.UtcNow,
                Details = "Role changed",
                AuthorUserId = Guid.NewGuid(),
                AffectedUserId = Guid.NewGuid(),
                AuthorUser = null,
                AffectedUser = null
            }
        };

        mockAuditService.Setup(s => s.GetSecurityEvents()).ReturnsAsync(securityEvents);

        var queries = new Queries();

        // Act
        var result = await queries.GetSecurityEvents(mockAuditService.Object);

        // Assert
        var dtoList = result.ToList();
        Assert.Equal(2, dtoList.Count);
        Assert.Equal("author@example.com", dtoList[0].AuthorUserEmail);
        Assert.Equal("affected@example.com", dtoList[0].AffectedUserEmail);
        Assert.Equal("", dtoList[1].AuthorUserEmail);
        Assert.Equal("", dtoList[1].AffectedUserEmail);
    }
}
