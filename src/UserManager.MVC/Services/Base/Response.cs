namespace UserManager.MVC.Services.Base;

public class Response<T>
{
    public string Message { get; init; } = null!;
    public string ValidationErrors { get; init; } = null!;
    public bool Success { get; init; }
    public T Data { get; init; } = default!;
}