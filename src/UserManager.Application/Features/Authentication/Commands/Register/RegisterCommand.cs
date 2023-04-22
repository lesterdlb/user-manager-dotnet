using ErrorOr;

using MediatR;

using UserManager.Application.Common.Contracts.Authentication;

namespace UserManager.Application.Features.Authentication.Commands.Register;

public record RegisterCommand(RegisterRequest Request) : IRequest<ErrorOr<RegisterResponse>>;