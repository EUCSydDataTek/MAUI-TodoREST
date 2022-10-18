using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TodoREST;

namespace HttpGenericRepository;
public class GenericRepository : IGenericRepository
{
    readonly HttpClient _client;
    readonly JsonSerializerOptions _serializerOptions;
    readonly IHttpsClientHandlerService _httpsClientHandlerService;

    public GenericRepository(IHttpsClientHandlerService service)
    {
#if DEBUG
        _httpsClientHandlerService = service;
        HttpMessageHandler handler = _httpsClientHandlerService.GetPlatformMessageHandler();
        if (handler != null)
            _client = new HttpClient(handler);
        else
            _client = new HttpClient();
#else
            _client = new HttpClient();
#endif
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }


    #region GET
    public async Task<T> GetAsync<T>(string id = "", string authToken = "")
    {
        ConfigureHttpClient(authToken);

        T result = default;

        Uri uri = new(string.Format(Constants.RestUrl, id));
        try
        {
            HttpResponseMessage response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<T>(content, _serializerOptions);
                Debug.WriteLine(@"+++++ Item(s) successfully received.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"----- ERROR {0}", ex.Message);
        }
        return result;
    }
    #endregion

    #region POST
    public async Task PostAsync<T>(T data, string authToken = "")
    {
        ConfigureHttpClient(authToken);

        Uri uri = new(string.Format(Constants.RestUrl, string.Empty));

        try
        {
            string json = JsonSerializer.Serialize<T>(data, _serializerOptions);
            StringContent content = new(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            response = await _client.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
                Debug.WriteLine(@"+++++ Item successfully created.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"----- ERROR {0}", ex.Message);
        }
    }

    public async Task<R> PostAsync<T, R>(T data, string authToken = "")
    {

        ConfigureHttpClient(authToken);

        R result = default;

        Uri uri = new(string.Format(Constants.RestUrl, string.Empty));

        try
        {
            string json = JsonSerializer.Serialize<T>(data, _serializerOptions);
            StringContent content = new(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            response = await _client.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine(@"+++++ Item successfully created.");
                string jsonResult = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<R>(jsonResult, _serializerOptions);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"----- ERROR {0}", ex.Message);
        }
        return result;
    }
    #endregion

    #region PUT
    public async Task PutAsync<T>(T data, string authToken = "")
    {
        ConfigureHttpClient(authToken);

        Uri uri = new(string.Format(Constants.RestUrl, string.Empty));

        try
        {
            string json = JsonSerializer.Serialize<T>(data, _serializerOptions);
            StringContent content = new(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            response = await _client.PutAsync(uri, content);

            if (response.IsSuccessStatusCode)
                Debug.WriteLine(@"+++++ Item successfully updated.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"----- ERROR {0}", ex.Message);
        }
    }
    #endregion

    #region DELETE
    public async Task DeleteAsync(string id, string authToken = "")
    {
        Uri uri = new(string.Format(Constants.RestUrl, id));

        try
        {
            HttpResponseMessage response = await _client.DeleteAsync(uri);
            if (response.IsSuccessStatusCode)
                Debug.WriteLine(@"+++++ TodoItem successfully deleted.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"----- ERROR {0}", ex.Message);
        }
    }
    #endregion

    #region HELPER
    private void ConfigureHttpClient(string authToken)
    {
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        if (!string.IsNullOrEmpty(authToken))
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }
        else
        {
            _client.DefaultRequestHeaders.Authorization = null;
        }
    }
    #endregion
}
