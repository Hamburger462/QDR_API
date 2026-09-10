using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QDR_Server.DTO;
using QDR_Server.DTO.ResponseMessages;
using QDR_Server.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QDR_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController(EventService eventService) : ControllerBase
    {
        // GET: api/<EventsController>
        [HttpGet]
        public async Task<ActionResult<List<EventDTO>>> GetAll()
        {
            var (events, status) = await eventService.GetAllEvents();
            switch (status)
            {
                case EventOperationStatus.EventNotFound:
                    return NotFound(new ResponseMessage("User not found"));
                case EventOperationStatus.Success:
                    return Ok(events);
                default:
                    return BadRequest();
            }
        }

        // GET api/<EventsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDTO>> GetById(Guid id)
        {
            var (events, status) = await eventService.GetEventById(id);
            switch (status)
            {
                case EventOperationStatus.EventNotFound:
                    return NotFound(new ResponseMessage("User not found"));
                case EventOperationStatus.Success:
                    return Ok(events);
                default:
                    return BadRequest();
            }
        }

        // POST api/<EventsController>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateEventDTO dto)
        {
            var status = await eventService.CreateEvent(dto);
            switch (status)
            {
                case EventOperationStatus.OrganizationNotFound:
                    return BadRequest(new ResponseMessage("Event belongs to the invalid organization"));
                case EventOperationStatus.Success:
                    return Ok(new ResponseMessage("Event created successfully"));
                default:
                    return BadRequest();
            }
        }

        // PUT api/<EventsController>/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateEventDTO dto)
        {
            var status = await eventService.UpdateEvent(id, dto);
            switch (status)
            {
                case EventOperationStatus.OrganizationNotFound:
                    return BadRequest(new ResponseMessage("Event belongs to the invalid organization"));
                case EventOperationStatus.Success:
                    return Ok(new ResponseMessage("Event updated successfully"));
                default:
                    return BadRequest();
            }
        }

        // DELETE api/<EventsController>/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var status = await eventService.DeleteEvent(id);
            switch (status)
            {
                case EventOperationStatus.EventNotFound:
                    return BadRequest(new ResponseMessage("Event does not exist"));
                case EventOperationStatus.Success:
                    return Ok(new ResponseMessage("Event has been deleted successfully"));
                default:
                    return BadRequest();
            }
        }
    }
}
