using Microsoft.EntityFrameworkCore;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        return await _context.Users
            .Include(u => u.Role)
            .ToListAsync();
    }

    public async Task CreateUser(string externalId, string email)
    {
        var basicUserRole = await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == "BasicUser");

        if (basicUserRole != null)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                ExternalId = externalId,
                Email = email,
                RoleId = basicUserRole.Id
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task AssignUserRole(string email, string roleName)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        var role = await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == roleName);

        if (user != null && role != null)
        {
            user.RoleId = role.Id;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
