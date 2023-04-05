using ItemWebApi.Models;
using ItemWebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IItemService, ItemService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Tutorial: Create a minimal API with ASP.NET Core (https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-7.0&tabs=visual-studio)

app.MapGet("/items", (IItemService service) => service.GetAll());

app.MapGet("/items{id}", (string id, IItemService service) => service.Find(id)
    is Item item ? Results.Ok(item) : Results.NotFound());

app.MapPost("/items", (Item item, IItemService service) =>
{
    service.Insert(item);
    return Results.Created($"/items/{item.ID}", item);
});

app.MapPut("/items/{id}", (string id, Item inputItem, IItemService service) =>
{
    var item = service.Find(id);

    if (item is null) return Results.NotFound();

    item.Name = inputItem.Name;
    item.Notes = inputItem.Notes;
    item.Done = inputItem.Done;

    service.Update(item);

    return Results.NoContent();
});

app.MapDelete("/items/{id}", (string id, IItemService service) =>
{
    if (service.Find(id) is Item item)
    {
        service.Delete(id);
        return Results.Ok(item);
    }
    return Results.NotFound();
});

app.Run();

