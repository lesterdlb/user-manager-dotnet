using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManager.Api.Controllers.Common;
using UserManager.Application.Common.DTOs.User;
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
    public async Task<ActionResult<IEnumerable<UserDto>>> Get()
    {
        var users = await Mediator.Send(new GetUsersQuery());

        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDto>> Get(Guid id)
    {
        var result = await Mediator.Send(new GetUserQuery(id.ToString()));

        return result.Match(Ok, Problem);
    }
}