namespace QDR_Server.DTO
{
    public class CreateEventDTO
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required DateTime Date { get; set; }
        public required string Location { get; set; }
        public required Guid OrganizationID { get; set; } 
    }
    public class UpdateEventDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? Date { get; set; }
        public string? Location { get; set; }
        public Guid? OrganizationID { get; set; }
    }
    public class EventDTO
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required DateTime Date { get; set; }
        public required string Location { get; set; }
    }
}