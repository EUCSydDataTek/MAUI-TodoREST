# 2.TodoGenericReposistory

Her benyttes et generisk repository med f�lgende interface:

```csharp
public interface IGenericRepository
{
    Task<T> GetAsync<T>(Uri uri, string authToken = "");

    Task<bool> PostAsync<T>(Uri uri,T data, string authToken = "");

    Task<R> PostAsync<T, R>(Uri uri, T data, string authToken = "");

    Task<bool> PutAsync<T>(Uri uri, T data, string authToken = "");

    Task<bool> DeleteAsync(Uri uri, string authToken = "");
}
```

Alle metoderne har mulighed for at tage imod en AccessToken.

`Task<T> GetAsync<T>(string id = "", string authToken = "")` henter en `List<T>` hvis `id` er tom, men et specifikt item `<T>` hvis et `id` sendes med.

Der er to udgaver af Post, nemlig:

```
Task PostAsync<T>(T data, string authToken = "");

Task<R> PostAsync<T, R>(T data, string authToken = "");
```

Den f�rste returnerer void, mens den anden returnerer det oprettede objekt, s�ledes at man f.eks. kan benytte et Id eller et TimeStamp som databasen opretter.

Alle metoder skriver i Output vinduet, hvis man benytter Debug.

&nbsp;

## Configuration af URL

Pga. at Android Emulator benytter et andet netv�rk og heller ikke kender localhost er det nemmest at benytte ****Dev Tunnels***.
S�t WebApi projektet som Startup og v�lg *https* og opret/v�lg en *Dev Tunnel*. Derefter s�ttes begge projekter til Startup.

I Constants klassen tilrettes url'en:

```csharp
public static class Constants
{
    // DevTunnes url, tilpas adressen
    public static string BaseUrl = "https://xxx.euw.devtunnels.ms";
    public static string Endpoint = "todoitems";
}
```


&nbsp;


 