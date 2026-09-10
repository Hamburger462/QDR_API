using Microsoft.AspNetCore.Mvc;
using QDR_Server.DTO;
using QDR_Server.Services;

namespace QDR_Server.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class OrgsController(OrganizationService orgService) : ControllerBase
    {
        // GET: api/<UsersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrganizationDTO>>> GetAll()
        {
            var (orgs, status) = await orgService.GetAllOrgs();
            switch (status)
            {
                case OrgOperationStatus.OrganizationNotFound:
                    return NotFound();
                case OrgOperationStatus.Success:
                    return Ok(orgs);
                default:
                    return BadRequest();
            }
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetById(Guid id)
        {
            var (org, status) = await orgService.GetOrgById(id);

            switch (status) {
                case OrgOperationStatus.OrganizationNotFound:
                    return NotFound();
                case OrgOperationStatus.Success:
                    return Ok(org);
                default:
                    return BadRequest();
            }
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateOrgDTO dto)
        {
            var status = await orgService.CreateOrg(dto);
            switch (status)
            {
                case OrgOperationStatus.Failure:
                    return BadRequest();
                case OrgOperationStatus.Success:
                    return Ok();
                default:
                    return BadRequest();
            }
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOrgDTO dto)
        {
            var status = await orgService.UpdateOrg(id, dto);

            switch (status)
            {
                case OrgOperationStatus.OrganizationNotFound:
                    return NotFound();
                case OrgOperationStatus.Success:
                    return Ok();
                default:
                    return BadRequest();
            }
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var status = await orgService.DeleteOrg(id);
            switch (status) {
                case OrgOperationStatus.Success:
                    return Ok();
                default:
                    return BadRequest();
            }
        }
    }
}