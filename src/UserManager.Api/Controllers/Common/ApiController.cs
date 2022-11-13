using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UserManager.Api.Controllers.Common;

[ApiController]
[Route("api/[controller]")]
public class ApiController : ControllerBase
{
    private readonly ISender _mediator = null!;

    protected ISender Mediator => _mediator;
}