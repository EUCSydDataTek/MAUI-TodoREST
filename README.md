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
- [Connect to local web services from iOS simulators and Android emulators](https://docs.microsoft.com/dotnet/maui/data-cloud/local-web-services)


## Web service operations

| Operation                	| HTTP Method 	| Relative URI        	| Parameters                	|
|--------------------------	|-------------	|---------------------	|---------------------------	|
| Get a list of todo items 	| GET         	| /api/todoitems/     	|                           	|
| Create a new todo item   	| POST        	| /api/todoitems/     	| A JSON formatted TodoItem 	|
| Update a todo item       	| PUT         	| /api/todoitems/     	| A JSON formatted TodoItem 	|
| Delete a todo item       	| DELETE      	| /api/todoitems/{id} 	|                           	|

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
 