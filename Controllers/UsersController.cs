using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using QDR_Server.DTO;
using QDR_Server.DTO.ResponseMessages;
using QDR_Server.Models;
using QDR_Server.Services;
using System.Security.Claims;

namespace QDR_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(UserService userService) : ControllerBase
    {
        // GET: api/<UsersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetAll()
        {
            var (users, status) = await userService.GetAllUsers();
            switch (status)
            {
                case UserOperationStatus.UserNotFound:
                    return NotFound();
                case UserOperationStatus.Success:
                    return Ok(users);
                default:
                    return BadRequest();
            }
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDTO>> GetById(Guid id)
        {
            var (users, status) = await userService.GetUserById(id);

            switch (status) {
                case UserOperationStatus.UserNotFound:
                    return NotFound();
                case UserOperationStatus.Success:
                    return Ok(users);
                default:
                    return BadRequest();
            }
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<ActionResult<UserResponseDTO>> Create(CreateUserDTO dto)
        {
            var (result, user) = await userService.CreateUser(dto);

            if (result != UserOperationStatus.Success || user is null)
            {
                return result switch
                {
                    UserOperationStatus.EmailTaken => Conflict("Email already in use."),
                    UserOperationStatus.OrganizationNotFound => BadRequest("One or more organizations do not exist."),
                    _ => BadRequest()
                };
            }

            var response = new UserResponseDTO(
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
        public async Task<IActionResult> Update(Guid id, UpdateUserDTO dto)
        {
            var result = await userService.UpdateUserById(id, dto);

            return result switch
            {
                UserOperationStatus.UserNotFound => NotFound(),
                UserOperationStatus.EmailTaken => Conflict("Email already in use."),
                UserOperationStatus.OrganizationNotFound => BadRequest("One or more organizations do not exist."),
                _ => NoContent()
            };
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var status = await userService.DeleteUserById(id);
            switch (status) {
                case UserOperationStatus.UserNotFound:
                    return NotFound();
                case UserOperationStatus.Success:
                    return Ok();
                default:
                    return BadRequest();
            }
        }
    }
}