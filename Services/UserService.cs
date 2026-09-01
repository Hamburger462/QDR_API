using Microsoft.EntityFrameworkCore;
using QDR_Server.Data;
using QDR_Server.DTO;
using QDR_Server.Models;

namespace QDR_Server.Services
{
    // Status codes
    public enum UserOperationResult
    {
        Success,
        UserNotFound,
        EmailTaken,
        OrganizationNotFound
    }

    public class UserService(AppDbContext context)
    {
        public async Task<IEnumerable<UserResponseDto>> GetAllUsers()
        {
            return await context.Users
                .Select(user => new UserResponseDto(
                    user.Id,
                    user.Username,
                    user.Email,
                    user.Role,
                    user.IsVerified,
                    user.Organizations.Select(o => o.Id).ToList()))
                .ToListAsync();
        }

        public async Task<UserResponseDto?> GetUserById(Guid id)
        {
            var user = await context.Users
                .Include(u => u.Organizations)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
                return null;

            return new UserResponseDto(
                user.Id,
                user.Username,
                user.Email,
                user.Role,
                user.IsVerified,
                user.Organizations.Select(o => o.Id).ToList());
        }

        public async Task<User?> GetUserByEmailForAuth(string email)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<(UserOperationResult Result, User? User)> CreateUser(CreateUserDto dto)
        {
            var emailTaken = await context.Users.AnyAsync(u => u.Email == dto.Email);
            if (emailTaken)
                return (UserOperationResult.EmailTaken, null);

            var orgs = new List<Organization>();
            // Organization WIP
            //if (dto.OrganizationIds.Count > 0)
            //{
            //    orgs = await context.Organizations
            //        .Where(o => dto.OrganizationIds.Contains(o.Id))
            //        .ToListAsync();

            //    if (orgs.Count != dto.OrganizationIds.Count)
            //        return (UserOperationResult.OrganizationNotFound, null);
            //}

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Role = dto.Role,
                Organizations = orgs,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return (UserOperationResult.Success, user);
        }

        public async Task<UserOperationResult> UpdateUserById(Guid id, UpdateUserDto dto)
        {
            var user = await context.Users
                .Include(u => u.Organizations)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
                return UserOperationResult.UserNotFound;

            if (dto.Username is not null)
                user.Username = dto.Username;

            if (dto.Email is not null)
            {
                var emailTaken = await context.Users.AnyAsync(u => u.Email == dto.Email && u.Id != id);
                if (emailTaken)
                    return UserOperationResult.EmailTaken;

                user.Email = dto.Email;
            }

            if (dto.OrganizationIds is not null)
            {
                var orgs = await context.Organizations
                    .Where(o => dto.OrganizationIds.Contains(o.Id))
                    .ToListAsync();

                if (orgs.Count != dto.OrganizationIds.Count)
                    return UserOperationResult.OrganizationNotFound;

                user.Organizations = orgs;
            }

            await context.SaveChangesAsync();
            return UserOperationResult.Success;
        }

        public async Task<bool> DeleteUserById(Guid id)
        {
            var user = await context.Users.FindAsync(id);
            if (user is null)
                return false;

            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return true;
        }
    }
}