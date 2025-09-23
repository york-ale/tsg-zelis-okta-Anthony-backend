using System.Threading.Tasks;
using Moq;
using Xunit;

public class UserMutationsTests
{
    [Fact]
    public async Task CreateUser_CallsServiceAndReturnsTrue()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();

        mockUserService
            .Setup(s => s.CreateUser(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        var mutations = new UserMutations();

        // Act
        var result = await mutations.CreateUser("ext-123", "test@example.com", mockUserService.Object);

        // Assert
        Assert.True(result);
        mockUserService.Verify(s => s.CreateUser("ext-123", "test@example.com"), Times.Once);
    }

    [Fact]
    public async Task AssignUserRole_CallsServiceAndReturnsTrue()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        
        mockUserService
            .Setup(s => s.AssignUserRole(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        var mutations = new UserMutations();

        // Act
        var result = await mutations.AssignUserRole("test@example.com", "Admin", mockUserService.Object);

        // Assert
        Assert.True(result);
        mockUserService.Verify(s => s.AssignUserRole("test@example.com", "Admin"), Times.Once);
    }
}
