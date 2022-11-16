using ErrorOr;
using MediatR;
using UserManager.Application.Common.Contracts.Authentication;

namespace UserManager.Application.Authentication.Queries.Login;

public record LoginQuery(LoginRequest Request) : IRequest<ErrorOr<LoginResponse>>;