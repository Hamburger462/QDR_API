namespace QDR_Server.Models
{
    public class Organization
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public required string Email { get; set; }

        public List<Event> Events { get; set; } = new List<Event>();

        public bool IsVerified { get; set; } = false;

        public List<UserOrganization> UserOrganizations { get; set; } = new();
    }
}
