using System.Threading.Tasks;
using Moq;
using Xunit;

public class AuditMutationsUnitTests
{
    [Fact]
    public async Task LoginSuccessEvent_CallsServiceAndReturnsTrue()
    {
        // Arrange
        var mockAuditService = new Mock<IAuditService>();

        mockAuditService
            .Setup(s => s.LoginSuccessEvent(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        var mutations = new AuditMutations();

        // Act
        var result = await mutations.LoginSuccessEvent("test@example.com", "Google", mockAuditService.Object);

        // Assert
        Assert.True(result);
        mockAuditService.Verify(s => s.LoginSuccessEvent("test@example.com", "Google"), Times.Once);
    }

    [Fact]
    public async Task LogoutEvent_CallsServiceAndReturnsTrue()
    {
        // Arrange
        var mockAuditService = new Mock<IAuditService>();

        mockAuditService
            .Setup(s => s.LogoutEvent(It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        var mutations = new AuditMutations();

        // Act
        var result = await mutations.LogoutEvent("test@example.com", mockAuditService.Object);

        // Assert
        Assert.True(result);
        mockAuditService.Verify(s => s.LogoutEvent("test@example.com"), Times.Once);
    }

    [Fact]
    public async Task RoleAssignedEvent_CallsServiceAndReturnsTrue()
    {
        // Arrange
        var mockAuditService = new Mock<IAuditService>();

        mockAuditService
            .Setup(s => s.RoleAssignedEvent(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        var mutations = new AuditMutations();

        // Act
        var result = await mutations.RoleAssignedEvent("author@example.com", "user@example.com", "OldRole", "NewRole", mockAuditService.Object);

        // Assert
        Assert.True(result);
        mockAuditService.Verify(s => s.RoleAssignedEvent("author@example.com", "user@example.com", "OldRole", "NewRole"), Times.Once);
    }
}