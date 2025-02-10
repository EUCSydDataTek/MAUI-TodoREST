using TodoREST.Services;
using TodoREST.ViewModels;
using TodoREST.Views;
using TodoREST.Repository;
using Microsoft.Extensions.Logging;
using HttpGenericRepository;

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

        builder.Services.AddSingleton<IRestService, RestService>();

        builder.Services.AddHttpClient<IGenericRepository, GenericRepository>(client =>
        {
            client.BaseAddress = new Uri(Constants.BaseUrl);
        })
        .AddStandardResilienceHandler();

        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<MainPage>();

        builder.Services.AddTransient<DetailItemViewModel>();
        builder.Services.AddTransient<DetailItemPage>();

        // Polly Caching
        //builder.Services.AddMemoryCache();
        //builder.Services.AddSingleton<IAsyncCacheProvider, Polly.Caching.Memory.MemoryCacheProvider>();
        //builder.Services.AddSingleton<IReadOnlyPolicyRegistry<string>, PolicyRegistry>((serviceProvider) =>
        //{
        //    PolicyRegistry registry = new();
        //    registry.Add("myCachePolicy",
        //        Policy.CacheAsync(serviceProvider.GetRequiredService<IAsyncCacheProvider>().AsyncFor<HttpResponseMessage>(),
        //            TimeSpan.FromSeconds(10)));
        //    return registry;
        //});

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
