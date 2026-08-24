namespace QDR_Server.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public string Role { get; set; } = string.Empty;
        public Organization? Organization { get; set; }
        public Guid OrganizationID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
