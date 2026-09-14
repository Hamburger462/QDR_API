namespace QDR_Server.DTO
{
    public record UserDTO(
    Guid Id,
    string Username,
    string Email,
    string Role,
    bool IsVerified
);

    public class CreateUserDTO
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class UpdateUserDTO
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
    }
}
