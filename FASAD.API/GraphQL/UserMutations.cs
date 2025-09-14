[ExtendObjectType("Mutations")]
public class UserMutations
{
    public async Task<bool> CreateUser(string externalId, string email, [Service] IUserService userService)
    {
        await userService.CreateUser(externalId, email);

        return true;
    }

    public async Task<bool> AssignUserRole(string email, string roleName, [Service] IUserService userService)
    {
        await userService.AssignUserRole(email, roleName);

        return true;
    }
}