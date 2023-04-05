# 1.MedViewModels

I forhold til master branchen er her benyttet MVVM og CommunityToolkit.Mvvm til at lave IPNC og Commands.

&nbsp;

## Consume a REST-based web service

This sample demonstrates a Todo list application where the data is stored and accessed from a REST-based web service. The web service code is in the TodoAPI project.

The app functionality is:

- View a list of tasks.
- Add, edit, and delete tasks.
- Set a task's status to 'done'.

In all cases the tasks are stored in an in-memory collection that's accessed through a REST-based web service.

For more information about the sample see:

- [Consume a REST-based web service](https://docs.microsoft.com/dotnet/maui/data-cloud/rest)


## Web service operations

| Operation                	| HTTP Method 	| Relative URI        	| Parameters                	|
|--------------------------	|-------------	|---------------------	|---------------------------	|
| Get a list of todo items 	| GET         	| /api/todoitems/     	|                           	|
| Create a new todo item   	| POST        	| /api/todoitems/     	| A JSON formatted TodoItem 	|
| Update a todo item       	| PUT         	| /api/todoitems/     	| A JSON formatted TodoItem 	|
| Delete a todo item       	| DELETE      	| /api/todoitems/{id} 	|                           	|

&nbsp;

## Dev Tunnel

[How to use dev tunnels in Visual Studio 2022 with ASP.NET Core apps](https://learn.microsoft.com/da-dk/aspnet/core/test/dev-tunnels?view=aspnetcore-7.0)

Oprettes på WebApi projektet. Benyt *Persistent* tunnel og hvis fysisk device benyttes så skal den også være *public*.

Start WebApi projektet for at få Url'en, som benyttes i client-projektet.

Sæt begge projekter til at starte (*Properties* på Solution og vælg *Multiple startup projects*)