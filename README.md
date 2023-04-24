# 2.ItemGenericRepository & Minimal Api

# Minimal API

**TodoWebApi** er et Minimal API med ASP.NET Core, som er bygget p� baggrund af Tutorial: [Create a minimal API with ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-7.0&tabs=visual-studio)

Data gemmes vha. EntityFramework i en Memory database.

API'et kan testes vha. den medf�lgende todo.http fil.

### API operations

| API                     | Description               | Request body | Response body        |
|-------------------------|---------------------------|--------------|----------------------|
| GET /todoitems          | Get all to-do items       | None         | Array of to-do items |
| GET /todoitems/complete | Get completed to-do items | None         | Array of to-do items |
| GET /todoitems/{id}     | Get an item by ID         | None         | To-do item           |
| POST /todoitems         | Add a new item            | To-do item   | To-do item           |
| PUT /todoitems/{id}     | Update an existing item   | To-do item   | None                 |
| DELETE /todoitems/{id}  | Delete an item            | None         | None                 |

&nbsp;
---

# MAUI mobile client

Mobil klienten har f�lgende funktioner:

- Henter automatisk alle todo items n�r app'en startes
- N�r der laves push-to-refresh hentes alle todo items igen
- Der kan tilf�jes et nyt todo item ved at klikke p� +-tegnet og udfylde `Name` og `Notes`. Klik Save.
- N�r der klikkes p� et todo item f�r man mulighed for at �ndre `Name`, `Notes` og `IsComplete`. Klik Save for at gemme eller Delete for at slette aktuelt todo Item.

&nbsp;

### Generic Reposistory

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
    // DevTunnes url
    //public static string RestUrl = $"<Dev Tunnel>/todoitems/{0}";

    public static string RestUrl = "https://1mec30x4-7247.euw.devtunnels.ms/todoitems/{0}";
}
```


&nbsp;


 