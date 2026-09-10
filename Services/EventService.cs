using Microsoft.EntityFrameworkCore;
using QDR_Server.Data;
using QDR_Server.DTO;
using QDR_Server.Models;

namespace QDR_Server.Services
{
    public enum EventOperationStatus
    {
        Success,
        EventNotFound,
        OrganizationNotFound,
        CreationFailed,
    }
    public class EventService(AppDbContext context)
    {
        public async Task<(List<EventDTO>?, EventOperationStatus)> GetAllEvents()
        {
            var result = await context.Events.Select(e => new EventDTO { 
                Name = e.Name, 
                Description = e.Description, 
                Date = e.Date, 
                Location = e.Location})
                .ToListAsync();
            if (result.Count == 0) {
                return (null, EventOperationStatus.EventNotFound);
            }
            
            return (result, EventOperationStatus.Success);
        }

        public async Task<(EventDTO?, EventOperationStatus)> GetEventById(Guid id)
        {
            var result = await context.Events.Where(e => e.Id == id).Select(e => new EventDTO
            {
                Name = e.Name,
                Description = e.Description,
                Date = e.Date,
                Location = e.Location
            }).FirstOrDefaultAsync();
            if (result == null) {
                return (null, EventOperationStatus.EventNotFound);
            }
            return (result, EventOperationStatus.Success);
        }

        public async Task<EventOperationStatus> CreateEvent(CreateEventDTO dto)
        {
            var orgExists = await context.Organizations.AnyAsync(o => o.Id == dto.OrganizationID);
            if (!orgExists)
                return EventOperationStatus.OrganizationNotFound;

            var newEvent = new Event
            {
                Name = dto.Name,
                Description = dto.Description ?? string.Empty,
                Date = dto.Date,
                Location = dto.Location,
                OrganizationID = dto.OrganizationID
            };

            context.Events.Add(newEvent);
            await context.SaveChangesAsync();

            return EventOperationStatus.Success;
        }

        public async Task<EventOperationStatus> UpdateEvent(Guid id, UpdateEventDTO dto)
        {
            var eventEntity = await context.Events.FindAsync(id);
            if (eventEntity is null)
                return EventOperationStatus.EventNotFound;

            eventEntity.Name = dto.Name ?? eventEntity.Name;
            eventEntity.Description = dto.Description ?? eventEntity.Description;
            eventEntity.Date = dto.Date ?? eventEntity.Date;
            eventEntity.Location = dto.Location ?? eventEntity.Location;

            if (dto.OrganizationID is not null)
            {
                var orgExists = await context.Organizations.AnyAsync(o => o.Id == dto.OrganizationID);
                if (!orgExists)
                    return EventOperationStatus.OrganizationNotFound;   // ← now actually reported

                eventEntity.OrganizationID = dto.OrganizationID.Value;
            }

            await context.SaveChangesAsync();
            return EventOperationStatus.Success;
        }

        public async Task<EventOperationStatus> DeleteEvent(Guid id)
        {
            var eventObj = await context.Events.FindAsync(id);
            if (eventObj is null) return EventOperationStatus.EventNotFound;

            context.Events.Remove(eventObj);
            await context.SaveChangesAsync();

            return EventOperationStatus.Success;
        }
    }
}
