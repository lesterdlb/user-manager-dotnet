using System.Net.Http.Headers;
using UserManager.MVC.Contracts;

namespace UserManager.MVC.Services.Base;

public class BaseHttpService
{
    private readonly ILocalStorageService _localStorageService;
    private readonly IClient _client;

    protected BaseHttpService(IClient client, ILocalStorageService localStorageService)
    {
        _client = client;
        _localStorageService = localStorageService;
    }

    protected Response<TGuid> ConvertApiException<TGuid>(ApiException ex)
    {
        return ex.StatusCode switch
        {
            400 => new Response<TGuid>
            {
                Success = false, Message = "Validation errors have occurred", ValidationErrors = ex.Response
            },
            404 => new Response<TGuid> { Success = false, Message = "The requested resource was not found" },
            _ => new Response<TGuid> { Success = false, Message = "An error has occurred, please try again." }
        };
    }

    protected void AddBearerToken()
    {
        if (_localStorageService.Exists("token"))
            _client.HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    _localStorageService.GetStorageValue<string>("token"));
    }
}