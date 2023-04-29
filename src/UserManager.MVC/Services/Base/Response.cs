namespace UserManager.MVC.Services.Base;

public class Response<T>
{
    public string Message { get; set; } = null!;
    public List<string> ValidationErrors { get; set; } = new();
    public bool Success { get; set; }
    public T Data { get; set; } = default!;
}