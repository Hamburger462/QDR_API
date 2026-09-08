namespace QDR_Server.DTO
{
    public class OrganizationDTO
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Email { get; set; }
        public bool isVerified { get; set; } = false;
    }
    public class CreateOrgDTO
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Email { get; set; }
    }
    public class UpdateOrgDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Email { get; set; }
    }
}
