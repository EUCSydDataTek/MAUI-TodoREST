# 1.HttpClient & Minimal Api

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

## SQLite Database

#### Nuget libraries
- Microsoft.EntityFrameworkCore.Sqlite
- Microsoft.EntityFrameworkCore.Design

I Program.cs ændres klassen `UseInMemoryDatabase` til `UseSqlite`:

```csharp

#### Migrations
Her benyttes *Developer Powershell* terminalen. Højre klik på Api-projektet og vælg "Open in Terminal":

Sørg for at Tool 'dotnet-ef' har samme version som Microsoft.EntityFrameworkCore.Design, version udlæses med:

```dotnet tool list --global```

Hvis versionen ikke er den samme, så opdater med og tilpas versionsnummer x.x.x:

```dotnet tool update --global dotnet-ef --version `x.x.x```

Lav migration:

```dotnet ef migrations add InitialCreate```            

Update database:

```dotnet ef database update``` 






