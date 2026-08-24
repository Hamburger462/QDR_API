namespace QDR_Server.Models
{
    public class Event
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;

        public List<Category> Categories { get; set; } = new List<Category>();
        public string Location { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Organization? Organization { get; set; }
        public Guid OrganizationID { get; set; }
    }
}
