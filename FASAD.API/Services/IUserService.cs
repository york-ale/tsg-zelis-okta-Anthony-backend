public interface IUserService
{
    Task<IEnumerable<User>> GetUsers();

    Task CreateUser(string externalId, string email);

    Task AssignUserRole(string email, string roleName);
}