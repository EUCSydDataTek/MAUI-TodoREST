namespace HttpGenericRepository;

/// <summary>
/// Remember to create a Constants.cs class with RESTurl
/// </summary>
public interface IGenericRepository
{
    Task<T?> GetAsync<T>(string endpoint);

    Task<bool> PostAsync<T>(string endpoint,T data);

    Task<R> PostAsync<T, R>(string endpoint, T data);

    Task<bool> PutAsync<T>(string endpoint, T data);

    Task<bool> DeleteAsync(string endpoint);
}