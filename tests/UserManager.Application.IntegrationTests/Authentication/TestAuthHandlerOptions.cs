using Microsoft.AspNetCore.Authentication;

namespace UserManager.Application.IntegrationTests.Authentication;

public class TestAuthHandlerOptions : AuthenticationSchemeOptions
{
    public string DefaultUserId { get; set; } = null!;
}