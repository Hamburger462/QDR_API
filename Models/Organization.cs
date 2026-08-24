namespace QDR_Server.Models
{
    public class Organization
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public required string Email { get; set; }

        public List<User> Users { get; set; } = new List<User>();

        public List<Event> Events { get; set; } = new List<Event>();
    }
}
