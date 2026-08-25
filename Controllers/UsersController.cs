using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using QDR_Server.Data;
using QDR_Server.DTO;
using QDR_Server.Models;

namespace QDR_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAll()
        {
            var result = await _context.Users
                .Select(user => new UserResponseDto(
                    user.Id,
                    user.Username,
                    user.Email,
                    user.Role,
                    user.IsVerified,
                    user.Organizations.Select(o => o.Id).ToList()))
                .ToListAsync();

            return Ok(result);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetById(Guid id)
        {
            var user = await _context.Users
                .Include(u => u.Organizations)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
                return NotFound();

            var result = new UserResponseDto(
                user.Id,
                user.Username,
                user.Email,
                user.Role,
                user.IsVerified,
                user.Organizations.Select(o => o.Id).ToList());

            return Ok(result);
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> Create(CreateUserDto dto)
        {
            var emailTaken = await _context.Users.AnyAsync(u => u.Email == dto.Email);
            if (emailTaken)
                return Conflict("Email already in use.");

            var orgs = new List<Organization>();
            if (dto.OrganizationIds.Count > 0)
            {
                orgs = await _context.Organizations
                    .Where(o => dto.OrganizationIds.Contains(o.Id))
                    .ToListAsync();

                if (orgs.Count != dto.OrganizationIds.Count)
                    return BadRequest("One or more organizations do not exist.");
            }

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Role = dto.Role,
                Organizations = orgs,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = new UserResponseDto(
                user.Id,
                user.Username,
                user.Email,
                user.Role,
                user.IsVerified,
                user.Organizations.Select(o => o.Id).ToList());

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, result);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateUserDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Organizations)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
                return NotFound();

            if (dto.Username is not null)
                user.Username = dto.Username;

            if (dto.Email is not null)
            {
                var emailTaken = await _context.Users.AnyAsync(u => u.Email == dto.Email && u.Id != id);
                if (emailTaken)
                    return Conflict("Email already in use.");

                user.Email = dto.Email;
            }

            if (dto.OrganizationIds is not null)
            {
                var orgs = await _context.Organizations
                    .Where(o => dto.OrganizationIds.Contains(o.Id))
                    .ToListAsync();

                if (orgs.Count != dto.OrganizationIds.Count)
                    return BadRequest("One or more organizations do not exist.");

                user.Organizations = orgs;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}