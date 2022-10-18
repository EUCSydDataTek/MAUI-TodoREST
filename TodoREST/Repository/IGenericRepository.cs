namespace HttpGenericRepository;

/// <summary>
/// Remember to create a Constants.cs class with url's
/// </summary>
public interface IGenericRepository
{
    Task<T> GetAsync<T>(string id = "", string authToken = "");

    Task PostAsync<T>(T data, string authToken = "");

    Task<R> PostAsync<T, R>(T data, string authToken = "");

    Task PutAsync<T>(T data, string authToken = "");

    Task DeleteAsync(string id, string authToken = "");
}
