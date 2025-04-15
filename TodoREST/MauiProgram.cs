using HttpGenericRepository;
using Microsoft.Extensions.Logging;
using TodoREST.Repository;
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

        builder.Services.AddSingleton<IRestService, RestService>();

        builder.Services.AddHttpClient<IGenericRepository, GenericRepository>(client =>
        {
            client.BaseAddress = new Uri(Constants.BaseUrl);
        })
        .AddStandardResilienceHandler();    // Add resilience handler to all HttpClient instances

        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<MainPage>();

        builder.Services.AddTransient<DetailItemViewModel>();
        builder.Services.AddTransient<DetailItemPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
