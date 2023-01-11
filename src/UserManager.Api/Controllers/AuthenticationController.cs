﻿using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManager.Api.Controllers.Common;
using UserManager.Application.Authentication.Commands.Register;
using UserManager.Application.Authentication.Queries.Login;
using UserManager.Application.Authentication.Queries.Roles;
using UserManager.Application.Common.Contracts.Authentication;

namespace UserManager.Api.Controllers;

[AllowAnonymous]
[Route("api/auth")]
public class AuthenticationController : ApiController
{
    public AuthenticationController(IMapper mapper, ISender mediator)
        : base(mapper, mediator)
    {
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var query = new LoginQuery(request);

        var result = await Mediator.Send(query);

        return result.Match(
            Ok,
            Problem);
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest request)
    {
        var command = new RegisterCommand(request);

        var result = await Mediator.Send(command);

        return result.Match(
            Ok,
            Problem);
    }

    [HttpGet("roles")]
    public async Task<ActionResult<List<string>>> Roles()
    {
        return Ok(await Mediator.Send(new GetRolesQuery()));
    }
}