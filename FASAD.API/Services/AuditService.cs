using Microsoft.EntityFrameworkCore;

public class AuditService : IAuditService
{
    private readonly AppDbContext _context;

    public AuditService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SecurityEvent>> GetSecurityEvents()
    {
        return await _context.SecurityEvents
            .Include(se => se.AuthorUser)
            .Include(se => se.AffectedUser)
            .OrderByDescending(se => se.OccurredUtc)
            .ToListAsync();
    }

    public async Task LoginSuccessEvent(string email, string provider)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user != null)
        {
            var securityEvent = new SecurityEvent
            {
                Id = Guid.NewGuid(),
                EventType = "LoginSuccess",
                AuthorUserId = user.Id,
                AffectedUserId = user.Id,
                OccurredUtc = DateTime.UtcNow,
                Details = $"provider = {provider}"
            };
            _context.SecurityEvents.Add(securityEvent);
            await _context.SaveChangesAsync();
        }
    }

    public async Task LogoutEvent(string email)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user != null)
        {
            var securityEvent = new SecurityEvent
            {
                Id = Guid.NewGuid(),
                EventType = "Logout",
                AuthorUserId = user.Id,
                AffectedUserId = user.Id,
                OccurredUtc = DateTime.UtcNow,
                Details = "local sign-out"
            };
            _context.SecurityEvents.Add(securityEvent);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RoleAssignedEvent(string authorUserEmail, string affectedUserEmail, string fromRoleName, string toRoleName)
    {
        var authorUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == authorUserEmail);

        var affectUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == affectedUserEmail);

        if (authorUser != null && affectUser != null)
        {
            var securityEvent = new SecurityEvent
            {
                Id = Guid.NewGuid(),
                EventType = "RoleAssigned",
                AuthorUserId = authorUser.Id,
                AffectedUserId = affectUser.Id,
                OccurredUtc = DateTime.UtcNow,
                Details = $"from = {fromRoleName} to = {toRoleName}"
            };
            _context.SecurityEvents.Add(securityEvent);
            await _context.SaveChangesAsync();
        }
    }
}
