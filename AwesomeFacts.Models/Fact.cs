namespace AwesomeFacts.Models;

public class Fact
{
    public int Id { get; set; }
    public required string Text { get; set; }
    public required string Category { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsVerified { get; set; }
} 