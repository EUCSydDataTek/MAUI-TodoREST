using HttpGenericRepository;
using Polly;
using Polly.Caching;
using Polly.Registry;
using TodoREST.Services;
using TodoREST.ViewModels;
using TodoREST.Views;

namespace TodoREST;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        builder.Services.AddSingleton<IHttpsClientHandlerService, HttpsClientHandlerService>();
        builder.Services.AddSingleton<ITodoService, TodoService>();

        builder.Services.AddSingleton<IGenericRepository, GenericRepository>();


        builder.Services.AddSingleton<TodoListViewModel>();
        builder.Services.AddSingleton<TodoListPage>();

        builder.Services.AddTransient<TodoItemViewModel>();
        builder.Services.AddTransient<TodoItemPage>();

        // Polly Caching
        builder.Services.AddMemoryCache();
        builder.Services.AddSingleton<IAsyncCacheProvider, Polly.Caching.Memory.MemoryCacheProvider>();
        builder.Services.AddSingleton<IReadOnlyPolicyRegistry<string>, PolicyRegistry>((serviceProvider) =>
        {
            PolicyRegistry registry = new();
            registry.Add("myCachePolicy",
                Policy.CacheAsync(serviceProvider.GetRequiredService<IAsyncCacheProvider>().AsyncFor<HttpResponseMessage>(),
                    TimeSpan.FromSeconds(10)));
            return registry;
        });


        return builder.Build();
    }
}
