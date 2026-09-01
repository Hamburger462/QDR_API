namespace QDR_Server.DTO
{
    public class AuthDTO
    {
        public class LoginDTO
        {
            public required string Email { get; set; }
            public required string Password { get; set; }
        }
    }
}
