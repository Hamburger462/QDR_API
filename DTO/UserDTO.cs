namespace QDR_Server.DTO
{
    public record UserResponseDto(
    Guid Id,
    string Username,
    string Email,
    string Role,
    bool IsVerified,
    List<Guid> OrganizationIds
);

    public class CreateUserDto
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public List<Guid> OrganizationIds { get; set; } = new();
        public string Role { get; set; } = "Member";
    }

    public class UpdateUserDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public List<Guid>? OrganizationIds { get; set; }
    }
}
