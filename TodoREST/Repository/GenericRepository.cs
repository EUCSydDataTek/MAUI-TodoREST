using HttpGenericRepository;
using Polly;
using Polly.Registry;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
//using TodoREST.Policies;

namespace TodoREST.Repository;
public class GenericRepository : IGenericRepository
{
    readonly HttpClient _client;
    readonly JsonSerializerOptions _serializerOptions;

    public GenericRepository(HttpClient client)
    {
        _client = client;

        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }


    #region GET
    public async Task<T?> GetAsync<T>(string endpoint)
    {
        T? result = default;

        try
        {
            HttpResponseMessage response = await _client.GetAsync(endpoint);
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
    public async Task<bool> PostAsync<T>(string endpoint, T data)
    {
        try
        {
            string json = JsonSerializer.Serialize<T>(data, _serializerOptions);
            StringContent content = new(json, Encoding.UTF8, "application/json");

            HttpResponseMessage? response = null;
            response = await _client.PostAsync(endpoint, content);

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

    public async Task<R> PostAsync<T, R>(string endpoint, T data)
    {
        R? result = default;

        try
        {
            string json = JsonSerializer.Serialize<T>(data, _serializerOptions);
            StringContent content = new(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            response = await _client.PostAsync(endpoint, content);

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
    public async Task<bool> PutAsync<T>(string endpoint, T data)
    {
        try
        {
            string json = JsonSerializer.Serialize<T>(data, _serializerOptions);
            StringContent content = new(json, Encoding.UTF8, "application/json");

            HttpResponseMessage? response = null;
            response = await _client.PutAsync(endpoint, content);

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
    public async Task<bool> DeleteAsync(string endpoint)
    {
        try
        {
            HttpResponseMessage response = await _client.DeleteAsync(endpoint);
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
}