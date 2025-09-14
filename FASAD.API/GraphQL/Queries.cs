public class Queries
{
    public async Task<IEnumerable<UserDto>> GetUsers([Service] IUserService userService)
    {
        var users = await userService.GetUsers();

        return users.Select(u => new UserDto
        {
            Id = u.Id,
            Email = u.Email,
            RoleName = u.Role?.Name ?? ""
        });
    }

    public async Task<IEnumerable<RoleDto>> GetRoles([Service] IRoleService roleService)
    {
        var roles = await roleService.GetRoles();

        return roles.Select(r => new RoleDto
        {
            Id = r.Id,
            Name = r.Name
        });
    }

    public async Task<IEnumerable<SecurityEventDto>> GetSecurityEvents([Service] IAuditService auditService)
    {
        var securityEvents = await auditService.GetSecurityEvents();

        return securityEvents.Select(se => new SecurityEventDto
        {
            Id = se.Id,
            EventType = se.EventType,
            OccurredUtc = se.OccurredUtc,
            Details = se.Details,
            AuthorUserEmail = se.AuthorUser?.Email ?? "",
            AffectedUserEmail = se.AffectedUser?.Email ?? ""
        });
    }
}