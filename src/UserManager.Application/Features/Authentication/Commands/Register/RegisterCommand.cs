using ErrorOr;
using MediatR;
using UserManager.Application.Common.Contracts.Authentication;

namespace UserManager.Application.Authentication.Commands.Register;

public record RegisterCommand(RegisterRequest Request) : IRequest<ErrorOr<RegisterResponse>>;