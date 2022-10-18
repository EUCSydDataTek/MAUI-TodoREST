# 2.GenericRepository

Her benyttes et generisk repository med følgende interface:

```csharp
public interface IGenericRepository
{
    Task<T> GetAsync<T>(string id = "", string authToken = "");

    Task PostAsync<T>(T data, string authToken = "");

    Task<R> PostAsync<T, R>(T data, string authToken = "");

    Task PutAsync<T>(T data, string authToken = "");

    Task DeleteAsync(string id, string authToken = "");
}
```

Alle metoderne har mulighed for at tage imod en AccessToken.

`Task<T> GetAsync<T>(string id = "", string authToken = "")` henter en `List<T>` hvis `id` er tom, men et specifikt item `<T>` hvis et `id` sendes med.

Der er to udgaver af Post, nemlig:

```
Task PostAsync<T>(T data, string authToken = "");

Task<R> PostAsync<T, R>(T data, string authToken = "");
```

Den første returnerer void, mens den anden returnerer det oprettede objekt, således at man f.eks. kan benytte et Id eller et TimeStamp som databasen opretter.

Alle metoder skriver i Output vinduet, hvis man benytter Debug.

&nbsp;

## Configuration af URL

Der oprettes en klasse kaldet Constants.cs, som tilpasses de aktuelle URL's:

```csharp
public static class Constants
{
    // URL of REST service
    //public static string RestUrl = "https://YOURPROJECT.azurewebsites.net/api/todoitems/{0}";

    // URL of REST service (Android does not use localhost)
    // Use http cleartext for local deployment. Change to https for production
    public static string LocalhostUrl = DeviceInfo.Platform == DevicePlatform.Android ? "10.0.2.2" : "localhost";
    public static string Scheme = "https"; // or https
    public static string Port = "5001"; // 5000 for http, 5001 for https
    public static string RestUrl = $"{Scheme}://{LocalhostUrl}:{Port}/api/todoitems/{{0}}";
}
```


&nbsp;

## HttpsClientHandlerService
I tilfælde af Debug, oprettes et `HttpMessageHandler` objekt, som sørger for at kortslutte
kontrollen af localhost-certifikatets gyldighed.
 