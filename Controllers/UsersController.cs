using Microsoft.AspNetCore.Mvc;

using QDR_Server.DTO;
using QDR_Server.Services;

namespace QDR_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(UserService userService) : ControllerBase
    {
        // GET: api/<UsersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAll()
        {
            var result = await userService.GetAllUsers();
            return Ok(result);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetById(Guid id)
        {
            var result = await userService.GetUserById(id);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> Create(CreateUserDto dto)
        {
            var (result, user) = await userService.CreateUser(dto);

            if (result != UserOperationResult.Success || user is null)
            {
                return result switch
                {
                    UserOperationResult.EmailTaken => Conflict("Email already in use."),
                    UserOperationResult.OrganizationNotFound => BadRequest("One or more organizations do not exist."),
                    _ => BadRequest()
                };
            }

            var response = new UserResponseDto(
                user.Id,
                user.Username,
                user.Email,
                user.Role,
                user.IsVerified,
                user.Organizations.Select(o => o.Id).ToList());

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, response);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateUserDto dto)
        {
            var result = await userService.UpdateUserById(id, dto);

            return result switch
            {
                UserOperationResult.UserNotFound => NotFound(),
                UserOperationResult.EmailTaken => Conflict("Email already in use."),
                UserOperationResult.OrganizationNotFound => BadRequest("One or more organizations do not exist."),
                _ => NoContent()
            };
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await userService.DeleteUserById(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}