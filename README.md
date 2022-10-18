
# Consume a REST-based web service

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

