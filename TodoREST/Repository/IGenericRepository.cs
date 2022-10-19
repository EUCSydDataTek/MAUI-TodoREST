namespace HttpGenericRepository;

/// <summary>
/// Remember to create a Constants.cs class with url's
/// </summary>
public interface IGenericRepository
{
    Task<T> GetAsync<T>(Uri uri, string authToken = "");

    Task<bool> PostAsync<T>(Uri uri,T data, string authToken = "");

    Task<R> PostAsync<T, R>(Uri uri, T data, string authToken = "");

    Task<bool> PutAsync<T>(Uri uri, T data, string authToken = "");

    Task<bool> DeleteAsync(Uri uri, string authToken = "");
}
