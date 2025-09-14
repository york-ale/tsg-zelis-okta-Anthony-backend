public class Role
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }

    // Navigation Properties
    public ICollection<RoleClaim> ?RoleClaims { get; set; }
}