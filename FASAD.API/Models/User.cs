public class User
{
    public Guid Id { get; set; }
    public required string ExternalId { get; set; }
    public required string Email { get; set; }
    public Guid RoleId { get; set; }

    // Navigation Properties
    public Role ?Role { get; set; }
}