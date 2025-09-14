public class SecurityEvent
{
    public Guid Id { get; set; }
    public required string EventType { get; set; }
    public Guid AuthorUserId { get; set; }
    public Guid AffectedUserId { get; set; }
    public DateTime OccurredUtc { get; set; }
    public required string Details { get; set; }

    // Navigation Properties
    public User ?AuthorUser { get; set; }
    public User ?AffectedUser { get; set; }
}