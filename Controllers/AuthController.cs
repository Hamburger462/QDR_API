using Microsoft.AspNetCore.Mvc;
using QDR_Server.Data;
using QDR_Server.Services;
using QDR_Server.DTO;
using Microsoft.EntityFrameworkCore;

namespace QDR_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(TokenService tokenService, UserService userService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] CreateUserDto registerData)
        {
            var (result, user) = await userService.CreateUser(registerData);

            if (result != UserOperationResult.Success || user is null)
            {
                return result switch
                {
                    UserOperationResult.EmailTaken => Conflict("Email already in use."),
                    UserOperationResult.OrganizationNotFound => BadRequest("One or more organizations do not exist."),
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