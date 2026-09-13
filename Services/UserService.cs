using Microsoft.EntityFrameworkCore;
using QDR_Server.Data;
using QDR_Server.DTO;
using QDR_Server.Models;

namespace QDR_Server.Services
{
    // Status codes
    public enum UserOperationStatus
    {
        Success,
        UserNotFound,
        EmailTaken,
        OrganizationNotFound
    }

    public class UserService(AppDbContext context, OrganizationService organizationService)
    {
        public async Task<(IEnumerable<UserDTO>?, UserOperationStatus)> GetAllUsers()
        {
            var users = await context.Users
                .Select(user => new UserDTO(
                    user.Id,
                    user.Username,
                    user.Email,
                    user.Role,
                    user.IsVerified,
                    user.Organizations.Select(o => o.Id).ToList()))
                .ToListAsync();
            if (users.Count == 0) return (null, UserOperationStatus.UserNotFound);
            return (users, UserOperationStatus.Success);
        }

        public async Task<(UserDTO?, UserOperationStatus)> GetUserById(Guid id)
        {
            var user = await context.Users
                .Include(u => u.Organizations)
                .FirstOrDefaultAsync(u => u.Id == id);

            if(user == null) return (null, UserOperationStatus.UserNotFound) ;

            return (
                new UserDTO(
                user.Id,
                user.Username,
                user.Email,
                user.Role,
                user.IsVerified,
                user.Organizations.Select(o => o.Id).ToList()), UserOperationStatus.Success);
        }

        public async Task<User?> GetUserByEmailForAuth(string email)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<(UserOperationStatus, User? User)> CreateUser(CreateUserDTO dto)
        {
            var emailTaken = await context.Users.AnyAsync(u => u.Email == dto.Email);
            if (emailTaken)
                return (UserOperationStatus.EmailTaken, null);

            var org = organizationService.CreateDefaultOrg(dto.Username, dto.Email);

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Role = "Member",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            var userOrganization = new UserOrganization
            {
                User = user,
                Organization = org,
                Position = OrganizationPosition.Owner,
            };

            user.UserOrganizations.Add(userOrganization);
            context.Users.Add(user);
            await context.SaveChangesAsync();

            return (UserOperationStatus.Success, user);
        }

        public async Task<UserOperationStatus> UpdateUser(Guid id, UpdateUserDTO dto)
        {
            var user = await context.Users
                .Include(u => u.Organizations)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
                return UserOperationStatus.UserNotFound;

            if (dto.Username is not null)
                user.Username = dto.Username;

            if (dto.Email is not null)
            {
                var emailTaken = await context.Users.AnyAsync(u => u.Email == dto.Email && u.Id != id);
                if (emailTaken)
                    return UserOperationStatus.EmailTaken;

                user.Email = dto.Email;
            }

            if (dto.OrganizationIds is not null)
            {
                var orgs = await context.Organizations
                    .Where(o => dto.OrganizationIds.Contains(o.Id))
                    .ToListAsync();

                if (orgs.Count != dto.OrganizationIds.Count)
                    return UserOperationStatus.OrganizationNotFound;

                user.Organizations = orgs;
            }

            await context.SaveChangesAsync();
            return UserOperationStatus.Success;
        }

        public async Task<UserOperationStatus> DeleteUser(Guid id)
        {
            var user = await context.Users.FindAsync(id);
            if (user is null) return UserOperationStatus.UserNotFound;

            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return UserOperationStatus.Success;
        }
    }
}