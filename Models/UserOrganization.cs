namespace QDR_Server.Models
{
    public enum OrganizationPosition
    {
        Owner,
        Admin,
        Organizer,
        Member
    }

    public class UserOrganization
    {
        public Guid UserId { get; set; }
        public User? User { get; set; } = null;
        
        public Guid OrganizationId { get; set; }
        public Organization? Organization { get; set; } = null;
        
        public OrganizationPosition Position { get; set; } = OrganizationPosition.Member;
    }
}