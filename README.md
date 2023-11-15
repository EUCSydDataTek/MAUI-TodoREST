# 1.MinimalApiMAUIapp

## ItemWebApi - Minimal API med SQLite database

**ItemWebApi** er et Minimal API med ASP.NET Core, som er bygget på baggrund af denne tutorial: 
[Create a minimal API with ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-7.0&tabs=visual-studio)

Data gemmes vha. EntityFramework i en SQLite database.


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

## SQLite Database

#### Nuget libraries
- Microsoft.EntityFrameworkCore.Sqlite
- Microsoft.EntityFrameworkCore.Design 

Connectionstring er placeret i filen appsettings.json.

#### Migrations
Her benyttes *Developer Powershell* terminalen. Højre klik på Api-projektet og vælg "Open in Terminal":

Sørg for at Tool 'dotnet-ef' har samme version som Microsoft.EntityFrameworkCore.Design, version udlæses med:

```dotnet tool list --global```

Hvis versionen ikke er den samme, så opdater med dette script (tilpas versionsnummer x.x.x):

```dotnet tool update --global dotnet-ef --version x.x.x```

Lav migration:

```dotnet ef migrations add InitialCreate```            

Update database:

```dotnet ef database update``` 


----------------------------------------------------------------

&nbsp;

## Deploy med Dev Tunnel

Pga. at Android Emulator benytter et andet netværk og ikke kender localhost er det nemmest at benytte ***Dev Tunnels***. Det løser også problemet med at Android ikke
understøtter http eller self-signed certifikater.

[How to use dev tunnels in Visual Studio 2022 with ASP.NET Core apps](https://learn.microsoft.com/da-dk/aspnet/core/test/dev-tunnels?view=aspnetcore-7.0)

Oprettes på WebApi projektet. Benyt *Persistent* tunnel og vælg *public*.

Start først WebApi projektet for at få Url'en, som benyttes i client-projektet.


## Test med .http file
Som alternativ til Postman kan man oprette en testfil direkte i projektet:
Opret en .http fil i WebApi projektet. Tilpass adressen fra DevTunnet i eksemplet herunder:

```csharp
@devtunnel = https://xxx.euw.devtunnels.ms

GET {{devtunnel}}/todoitems

###

POST {{devtunnel}}/todoitems
Content-Type: application/json

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
    // DevTunnes url, tilpas adressen
    public static string BaseUrl = "https://xxx.euw.devtunnels.ms";
    public static string Endpoint = "todoitems";
}
```



