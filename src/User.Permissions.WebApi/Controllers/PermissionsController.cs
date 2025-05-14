using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using User.Permissions.Application.Commands;
using User.Permissions.Application.DTOs;
using User.Permissions.Application.Querys;

namespace User.Permissions.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermissionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("request")]
        [SwaggerOperation(Summary = "Request a new permission", Description = "Creates a new permission and logs it in elasticsearch and Kafka.")]
        [SwaggerResponse(201, "Permission successfully created", typeof(int))]
        [SwaggerResponse(400, "Invalid input data")]
        public async Task<ActionResult<int>> RequestPermission([FromBody] RequestPermissionCommand command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetPermissions), new { id }, id);
        }

        [HttpPut("modify/{id}")]
        [SwaggerOperation(Summary = "Modify an existing permission", Description = "Updates a permission by ID and logs the operation.")]
        [SwaggerResponse(204, "Permission successfully modified")]
        [SwaggerResponse(404, "Permission not found")]
        public async Task<IActionResult> ModifyPermission(int id, [FromBody] ModifyPermissionCommand command)
        {
            if (id != command.Id) command.Id = id;

            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Permission with ID {id} not found.");
            }
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all permissions", Description = "Retrieves all permissions from the database.")]
        [SwaggerResponse(200, "List of permissions", typeof(IEnumerable<PermissionDto>))]
        public async Task<ActionResult<IEnumerable<PermissionDto>>> GetPermissions()
        {
            var result = await _mediator.Send(new GetPermissionsQuery());
            return Ok(result);
        }
    }
}
