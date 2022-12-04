using Hanssens.Net;
using UserManager.MVC.Contracts;

namespace UserManager.MVC.Services;

public class LocalStorageService : ILocalStorageService
{
    private readonly LocalStorage _localStorage;

    public LocalStorageService()
    {
        var config = new LocalStorageConfiguration
        {
            AutoLoad = true,
            AutoSave = true,
            Filename = "UserManager.MVC"
        };
        _localStorage = new LocalStorage(config);
    }

    public void ClearStorage(List<string> keys)
        => keys.ForEach(key => _localStorage.Remove(key));

    public bool Exists(string key) => _localStorage.Exists(key);

    public T GetStorageValue<T>(string key) => _localStorage.Get<T>(key);

    public void SetStorageValue<T>(string key, T value)
    {
        _localStorage.Store(key, value);
        _localStorage.Persist();
    }
}