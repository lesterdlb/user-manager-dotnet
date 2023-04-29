using System.Net.Http.Headers;

using Newtonsoft.Json;

using UserManager.MVC.Contracts;

namespace UserManager.MVC.Services.Base;

public class BaseHttpService
{
    private readonly ILocalStorageService _localStorageService;

    protected IClient Client { get; }

    protected BaseHttpService(IClient client, ILocalStorageService localStorageService)
    {
        Client = client;
        _localStorageService = localStorageService;
    }

    protected static Response<TGuid> ConvertApiException<TGuid>(ApiException ex)
    {
        return ex is ApiException<ProblemDetails> exception
            ? ParseProblemDetails<TGuid>(exception)
            : ex.StatusCode switch
            {
                400 => new Response<TGuid>
                {
                    Success = false,
                    Message = "Validation errors have occurred",
                    ValidationErrors = new List<string> { ex.Message }
                },
                404 => new Response<TGuid> { Success = false, Message = "The requested resource was not found" },
                _ => new Response<TGuid> { Success = false, Message = "An error has occurred, please try again." }
            };
    }

    protected void AddBearerToken()
    {
        if (_localStorageService.Exists("token"))
        {
            Client.HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    _localStorageService.GetStorageValue<string>("token"));
        }
    }

    private static Response<TGuid> ParseProblemDetails<TGuid>(ApiException<ProblemDetails> ex)
    {
        var errors = ex.Result.AdditionalProperties["errors"];
        var errorsDict =
            JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(errors?.ToString() ?? string.Empty);

        if (errorsDict is null)
            return new Response<TGuid> { Success = false, Message = "An error has occurred, please try again." };

        var validationErrors = new List<string>();
        foreach ((string? _, List<string>? messages) in errorsDict)
        {
            validationErrors.AddRange(messages.Select(message => message));
        }

        return new Response<TGuid>
        {
            Success = false, Message = "Validation errors have occurred", ValidationErrors = validationErrors
        };
    }
}