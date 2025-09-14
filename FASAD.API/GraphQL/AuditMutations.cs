[ExtendObjectType("Mutations")]
public class AuditMutations
{
    public async Task<bool> LoginSuccessEvent(string email, string provider, [Service] IAuditService auditService)
    {
        await auditService.LoginSuccessEvent(email, provider);

        return true;
    }

    public async Task<bool> LogoutEvent(string email, [Service] IAuditService auditService)
    {
        await auditService.LogoutEvent(email);

        return true;
    }

    public async Task<bool> RoleAssignedEvent(string authorUserEmail, string affectedUserEmail, string fromRoleName, string toRoleName, [Service] IAuditService auditService)
    {
        await auditService.RoleAssignedEvent(authorUserEmail, affectedUserEmail, fromRoleName, toRoleName);

        return true;
    }
}