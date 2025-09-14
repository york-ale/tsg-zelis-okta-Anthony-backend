using Microsoft.EntityFrameworkCore;

public class RoleService : IRoleService
{
    private readonly AppDbContext _context;

    public RoleService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Role>> GetRoles()
    {
        return await _context.Roles
            .Include(r => r.RoleClaims)
            .ToListAsync();
    }
}