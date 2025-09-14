public interface IRoleService
{
    Task<IEnumerable<Role>> GetRoles();
}