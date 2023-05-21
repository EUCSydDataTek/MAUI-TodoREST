# 2.ItemGenericRepository & Minimal Api

# Minimal API (ItemWebApi)

**ItemWebApi** er et Minimal API med ASP.NET Core, som er bygget på baggrund af Tutorial: 
[Create a minimal API with ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-7.0&tabs=visual-studio)

Data gemmes vha. EntityFramework i en Memory database.

API'et kan testes vha. den medfølgende **todo.http** fil.

### API operations

| API                     | Description               | Request body | Response body        |
|-------------------------|---------------------------|--------------|----------------------|
| GET /todoitems          | Get all to-do items       | None         | Array of to-do items |
| GET /todoitems/complete | Get completed to-do items | None         | Array of to-do items |
| GET /todoitems/{id}     | Get an item by ID         | None         | To-do item           |
| POST /todoitems         | Add a new item            | To-do item   | To-do item           |
| PUT /todoitems/{id}     | Update an existing item   | To-do item   | None                 |
| DELETE /todoitems/{id}  | Delete an item            | None         | None                 |

###### Tabellen er lavet med [Tables Generator](https://www.tablesgenerator.com/markdown_tables)

&nbsp;

#### Nuget libraries
- Microsoft.EntityFrameworkCore.InMemory 7.x.x

&nbsp;

## Deploy med Dev Tunnel

Pga. at Android Emulator benytter et andet netværk og heller ikke kender localhost er det nemmest at benytte ****Dev Tunnels***.

[How to use dev tunnels in Visual Studio 2022 with ASP.NET Core apps](https://learn.microsoft.com/da-dk/aspnet/core/test/dev-tunnels?view=aspnetcore-7.0)

Oprettes på WebApi projektet. Benyt *Persistent* tunnel og hvis fysisk device benyttes så skal den også være *public*.

Start først WebApi projektet for at få Url'en, som benyttes i client-projektet.


## Test med .http file

Opret en .http fil i WebApi projektet:

```csharp
@devtunnel = "<your DevTunnel address">

GET {{devtunnel}}/todoitems

###

POST {{devtunnel}}/todoitems
Content-Type application/json

{
  "name": "Walk dog",
  "notes": "In the rain",
  "isComplete": false
}
```

&nbsp;

---
# MAUI mobile client

Mobil klienten har følgende funktioner:

- Henter automatisk alle todo items når app'en startes
- Når der laves push-to-refresh hentes alle todo items igen
- Der kan tilføjes et nyt todo item ved at klikke på +-tegnet og udfylde `Name` og `Notes`. Klik Save.
- Når der klikkes på et todo item får man mulighed for at ændre `Name`, `Notes` og `IsComplete`. Klik Save for at gemme eller Delete for at slette aktuelt todo Item.

&nbsp;

#### Nuget libraries
- CommunityToolkit.Mvvm 8.x.x

&nbsp;

## Test

Sæt begge projekter til at starte (*Properties* på Solution og vælg *Multiple startup projects*)

I `Constants` klassen tilrettes url'en:

```csharp
public static class Constants
{
    // DevTunnes url
    //public static string RestUrl = $"<Dev Tunnel>/todoitems/{0}";

    public static string RestUrl = "https://1yec70x4-7247.euw.devtunnels.ms/todoitems/{0}";
}
```


&nbsp;

# Generic Reposistory

Her benyttes et generisk repository med følgende interface:

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

Den første returnerer void, mens den anden returnerer det oprettede objekt, således at man f.eks. kan benytte et Id eller et TimeStamp som databasen opretter.

Alle metoder skriver i Output vinduet, hvis man benytter Debug.

&nbsp;

## Configuration af URL

Pga. at Android Emulator benytter et andet netværk og heller ikke kender localhost er det nemmest at benytte ****Dev Tunnels***.
Sæt WebApi projektet som Startup og vælg *https* og opret/vælg en *Dev Tunnel*. Derefter sættes begge projekter til Startup.

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


 