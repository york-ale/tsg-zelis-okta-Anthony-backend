public class Claim
{
    public Guid Id { get; set; }
    public required string Type { get; set; }
    public required string Value { get; set; }

    // Navigation Properties
    public ICollection<RoleClaim> ?RoleClaims { get; set; }
}