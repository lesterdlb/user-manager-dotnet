using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using UserManager.Api.Controllers.Common;
using UserManager.Application.Common.DTOs.Role;
using UserManager.Application.Features.Roles.Commands.CreateRole;
using UserManager.Application.Features.Roles.Commands.DeleteRole;
using UserManager.Application.Features.Roles.Queries.GetRole;
using UserManager.Application.Features.Roles.Queries.GetRoles;

namespace UserManager.Api.Controllers;

// [Authorize(Roles = "Admin")]
[AllowAnonymous]
public class RolesController : ApiController
{
    public RolesController(IMapper mapper, ISender mediator)
        : base(mapper, mediator)
    {
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetAllRoles()
    {
        var roles = await Mediator.Send(new GetRolesQuery());

        return Ok(roles);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<RoleDto>> GetRoleById(Guid id)
    {
        var result = await Mediator.Send(new GetRoleQuery(id.ToString()));

        return result.Match(Ok, Problem);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<RoleDto>> Create([FromBody] CreateRoleDto roleDto)
    {
        var createRoleCommand = new CreateRoleCommand(roleDto);
        var result = await Mediator.Send(createRoleCommand);

        return result.Match(id => Ok(id), Problem);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Delete(Guid id)
    {
        var deleteRoleCommand = new DeleteRoleCommand(id);
        var result = await Mediator.Send(deleteRoleCommand);

        return result.Match(_ => Ok(), Problem);
    }
}