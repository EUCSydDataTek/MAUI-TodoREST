using HttpGenericRepository;
using Polly;
using Polly.Registry;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TodoREST.Policies;

namespace TodoREST.Repository;
public class GenericRepository : IGenericRepository
{
    readonly HttpClient _client;
    readonly JsonSerializerOptions _serializerOptions;
    private readonly ClientPolicy _clientPolicy;
    IAsyncPolicy<HttpResponseMessage> cachePolicy;

    public GenericRepository(IReadOnlyPolicyRegistry<string> policyRegistry, ClientPolicy clientPolicy)
    {
        _client = new HttpClient();

        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        cachePolicy = policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>("myCachePolicy");
        _clientPolicy = clientPolicy;
    }


    #region GET
    public async Task<T> GetAsync<T>(Uri uri, string authToken = "")
    {
        ConfigureHttpClient(authToken);

        T result = default;

        try
        {
            //HttpResponseMessage response = await _client.GetAsync(uri); // Erstattes af de næste linjer:

            #region POLLY
            HttpResponseMessage response = await Policy
                .HandleResult<HttpResponseMessage>(res => !res.IsSuccessStatusCode)

              .RetryAsync(10)
            //.WaitAndRetryAsync(retryCount: 5, retryAttempt => TimeSpan.FromSeconds(3))
            //.WaitAndRetryAsync(retryCount: 5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
            //.WaitAndRetryAsync
            //(
            //    retryCount: 5,
            //    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            //    onRetry: (ex, time) =>
            //    {
            //        Debug.WriteLine($"--> TimeSpan: {time.TotalSeconds}");
            //    }
            //)

            .ExecuteAsync(async () => await _client.GetAsync(uri));

            // With CachePolicy
            //HttpResponseMessage response = await cachePolicy.ExecuteAsync(context => _client.GetAsync(uri), new Context("FooKey"));

            // With ClientPolicy.cs
            //HttpResponseMessage response = await _clientPolicy.LoggingExponentialHttpRetry.ExecuteAsync(() =>
            //_client.GetAsync(uri));
            #endregion

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
    public async Task<bool> PostAsync<T>(Uri uri, T data, string authToken = "")
    {
        ConfigureHttpClient(authToken);

        try
        {
            string json = JsonSerializer.Serialize(data, _serializerOptions);
            StringContent content = new(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            response = await _client.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine(@"+++++ Item successfully created.");
                return true;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"----- ERROR {0}", ex.Message);
        }
        Debug.WriteLine(@"----- Item NOT created!");
        return false;
    }

    public async Task<R> PostAsync<T, R>(Uri uri, T data, string authToken = "")
    {

        ConfigureHttpClient(authToken);

        R result = default;

        try
        {
            string json = JsonSerializer.Serialize(data, _serializerOptions);
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
    public async Task<bool> PutAsync<T>(Uri uri, T data, string authToken = "")
    {
        ConfigureHttpClient(authToken);

        try
        {
            string json = JsonSerializer.Serialize(data, _serializerOptions);
            StringContent content = new(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            response = await _client.PutAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine(@"+++++ Item successfully updated.");
                return true;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"----- ERROR {0}", ex.Message);
        }
        Debug.WriteLine(@"----- Item NOT updated!");
        return false;
    }
    #endregion

    #region DELETE
    public async Task<bool> DeleteAsync(Uri uri, string authToken = "")
    {
        try
        {
            HttpResponseMessage response = await _client.DeleteAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine(@"+++++ TodoItem successfully deleted.");
                return true;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"----- ERROR {0}", ex.Message);
        }
        Debug.WriteLine(@"----- Item NOT deleted!");
        return false;
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