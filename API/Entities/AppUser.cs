namespace API.Entities;

public class AppUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string DisplayName { get; set; } //dupe property for db efficiency
    public required string Email { get; set; }
    public string? ImageUrl { get; set; } //dupe property for db efficiency
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }

    // Navigation properties
    public Member Member { get; set; } = null!;
}

