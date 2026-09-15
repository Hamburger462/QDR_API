using Microsoft.AspNetCore.Mvc;
using QDR_Server.Services;
using QDR_Server.DTO;
using Microsoft.AspNetCore.RateLimiting;


namespace QDR_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(TokenService tokenService, UserService userService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] CreateUserDTO registerData)
        {
            var (result, user) = await userService.CreateUser(registerData);

            if (result != UserOperationStatus.Success || user is null)
            {
                return result switch
                {
                    UserOperationStatus.EmailTaken => Conflict("Email already in use."),
                    UserOperationStatus.OrganizationNotFound => BadRequest("One or more organizations do not exist."),
                    _ => BadRequest()
                };
            }

            var token = tokenService.GenerateToken(user);
            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginUser([FromBody] AuthDTO.LoginDTO loginData)
        {
            var user = await userService.GetUserByEmailForAuth(loginData.Email);

            if (user is null || !BCrypt.Net.BCrypt.Verify(loginData.Password, user.PasswordHash))
                return Unauthorized("Invalid email or password.");

            var token = tokenService.GenerateToken(user);
            return Ok(new { token });
        }
    }
}