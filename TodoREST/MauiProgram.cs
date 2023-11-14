using HttpGenericRepository;
using Polly.Caching;
using Polly.Registry;
using Polly;
using TodoREST.Services;
using TodoREST.ViewModels;
using TodoREST.Views;
using TodoREST.Repository;
using TodoREST.Policies;

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
		builder.Services.AddSingleton<IDataService, DataService>();

		builder.Services.AddSingleton<IGenericRepository, GenericRepository>();
        builder.Services.AddSingleton(new ClientPolicy());  // husk

        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<MainPage>();

        builder.Services.AddTransient<DetailItemViewModel>();
        builder.Services.AddTransient<DetailItemPage>();

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
