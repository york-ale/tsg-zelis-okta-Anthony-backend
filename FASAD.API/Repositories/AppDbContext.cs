using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Claim> Claims { get; set; }
    public DbSet<RoleClaim> RoleClaims { get; set; }
    public DbSet<SecurityEvent> SecurityEvents { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var basicUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var authObserverId = Guid.Parse("00000000-0000-0000-0000-000000000002");
        var securityAuditorId = Guid.Parse("00000000-0000-0000-0000-000000000003");
        
        var viewClaimId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var roleClaimId = Guid.Parse("00000000-0000-0000-0000-000000000002");

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.ExternalId)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(320);

            entity.HasIndex(u => u.Email)
                .IsUnique();

            entity.HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(r => r.Name)
                .IsUnique();

            entity.Property(r => r.Description)
                .HasMaxLength(200);

            entity.HasMany(r => r.RoleClaims)
                .WithOne(rc => rc.Role)
                .HasForeignKey(rc => rc.RoleId);

            entity.HasData(
                new Role { Id = basicUserId, Name = "BasicUser", Description = "Has no permissions" },
                new Role { Id = authObserverId, Name = "AuthObserver", Description = "Has view auth events permissions" },
                new Role { Id = securityAuditorId, Name = "SecurityAuditor", Description = "Has view auth events and role changes permissions" }
            );
        });

        modelBuilder.Entity<Claim>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Type)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(c => c.Value)
                .IsRequired()
                .HasMaxLength(200);

            entity.HasMany(c => c.RoleClaims)
                .WithOne(rc => rc.Claim)
                .HasForeignKey(rc => rc.ClaimId);

            entity.HasData(
                new Claim { Id = viewClaimId, Type = "permissions", Value = "Audit.ViewAuthEvents" },
                new Claim { Id = roleClaimId, Type = "permissions", Value = "Audit.RoleChanges" }
            );
        });

        modelBuilder.Entity<RoleClaim>(entity =>
        {
            entity.HasKey(rc => new { rc.RoleId, rc.ClaimId });

            entity.HasOne(rc => rc.Role)
                .WithMany(r => r.RoleClaims)
                .HasForeignKey(rc => rc.RoleId);

            entity.HasOne(rc => rc.Claim)
                .WithMany(c => c.RoleClaims)
                .HasForeignKey(rc => rc.ClaimId);

            entity.HasData(
                new RoleClaim { RoleId = authObserverId, ClaimId = viewClaimId },
                new RoleClaim { RoleId = securityAuditorId, ClaimId = viewClaimId },
                new RoleClaim { RoleId = securityAuditorId, ClaimId = roleClaimId }
            );
        });

        modelBuilder.Entity<SecurityEvent>(entity =>
        {
            entity.HasKey(se => se.Id);

            entity.Property(se => se.EventType)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(se => se.OccurredUtc)
                .IsRequired()
                .HasDefaultValueSql("SYSUTCDATETIME()");

            entity.Property(se => se.Details)
                .HasMaxLength(400);

            entity.HasOne(se => se.AuthorUser)
                .WithMany()
                .HasForeignKey(se => se.AuthorUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(se => se.AffectedUser)
                .WithMany()
                .HasForeignKey(se => se.AffectedUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}