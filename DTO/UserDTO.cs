namespace QDR_Server.DTO
{
    public record UserResponseDTO(
    Guid Id,
    string Username,
    string Email,
    string Role,
    bool IsVerified,
    List<Guid> OrganizationIds
);

    public class CreateUserDTO
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public List<Guid> OrganizationIds { get; set; } = new();
    }

    public class UpdateUserDTO
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public List<Guid>? OrganizationIds { get; set; }
    }
}
