using Microsoft.EntityFrameworkCore;
using QDR_Server.Data;
using QDR_Server.DTO;
using QDR_Server.Models;

namespace QDR_Server.Services
{
    public enum OrgOperationStatus
    {
        Success, Failure, OrganizationNotFound, UsersNotFound
    }
    public class OrganizationService(AppDbContext context)
    {
        // Get all organizations
        public async Task<(List<OrganizationDTO>?, OrgOperationStatus)> GetAllOrgs()
        {
            var orgs = await context.Organizations.Select(
                o => new OrganizationDTO { 
                    Name = o.Name,
                    Email = o.Email,
                    Description = o.Description,
                    isVerified = o.IsVerified,
                })
                .ToListAsync();
            if (orgs.Count == 0) return (null, OrgOperationStatus.OrganizationNotFound);
            return (orgs, OrgOperationStatus.Success);
        }
        // Get single organization by id
        public async Task<(OrganizationDTO?, OrgOperationStatus)> GetOrgById(Guid id)
        {
            var org = await context.Organizations.Select(o => 
            new OrganizationDTO
            {
                Name = o.Name,
                Email = o.Email,
                Description = o.Description,
                isVerified = o.IsVerified,
            }).FirstOrDefaultAsync();
            if (org == null) return (null, OrgOperationStatus.OrganizationNotFound);
            return (org, OrgOperationStatus.Success);
        }
        // Create an organization instance
        public async Task<OrgOperationStatus> CreateOrg(CreateOrgDTO dto)
        {
            var newOrg = new Organization
            {
                Name = dto.Name,
                Email = dto.Email,
                Description = dto.Description ?? "",
            };
            context.Organizations.Add(newOrg);
            await context.SaveChangesAsync();
            return OrgOperationStatus.Success;
        }
        // Update an organization instance by id
        public async Task<OrgOperationStatus> UpdateOrg(Guid id, UpdateOrgDTO dto)
        {
            var org = await context.Organizations.FirstOrDefaultAsync(o => o.Id == id);
            if (org == null) return OrgOperationStatus.OrganizationNotFound;
            
            org.Name = dto.Name ?? org.Name;
            org.Email = dto.Email ?? org.Email;
            org.Description = dto.Description ?? org.Description;
            await context.SaveChangesAsync();
            return OrgOperationStatus.Success;
        }
        // Delete an organization instance by id
        public async Task<OrgOperationStatus> DeleteOrg(Guid id)
        {
            var org = await context.Organizations.FirstOrDefaultAsync(o => o.Id == id);
            if(org == null) return OrgOperationStatus.OrganizationNotFound;
            context.Organizations.Remove(org);
            await context.SaveChangesAsync();
            return OrgOperationStatus.Success;
        }

        // Create a default user organization
        public Organization CreateDefaultOrg(string name, string email)
        {
            return new Organization { 
                Name = name,
                Email = email,
                Description = ""
            };
        }
    }
}
