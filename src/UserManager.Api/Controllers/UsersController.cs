using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using UserManager.Api.Controllers.Common;
using UserManager.Application.Common.DTOs.User;
using UserManager.Application.Features.Users.Commands.CreateUser;
using UserManager.Application.Features.Users.Commands.DeleteUser;
using UserManager.Application.Features.Users.Queries.GetUser;
using UserManager.Application.Features.Users.Queries.GetUsers;

namespace UserManager.Api.Controllers;

[Authorize(Roles = "Admin")]
public class UsersController : ApiController
{
    public UsersController(IMapper mapper, ISender mediator)
        : base(mapper, mediator)
    {
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
    {
        var users = await Mediator.Send(new GetUsersQuery());

        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetUserById(Guid id)
    {
        var result = await Mediator.Send(new GetUserQuery(id.ToString()));

        return result.Match(Ok, Problem);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> CreateUser([FromBody] CreateUserCommand createUserCommand)
    {
        var result = await Mediator.Send(createUserCommand);

        return result.Match(
            id => CreatedAtActionResult(nameof(GetUserById), id, id),
            Problem);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteUser(Guid id)
    {
        var result = await Mediator.Send(new DeleteUserCommand(id));

        return result.Match(_ => NoContent(), Problem);
    }
}