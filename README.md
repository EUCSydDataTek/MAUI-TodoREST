# 3.NetworkResiliencePolly

## Kontrol af netværksadgang med Connectivity

`Connectivity` konfigureres i *Startup.cs* i MAUI projektet.
I *MainPageViewModel* laves et check af om der er netværksadgang.

```csharp
if (connectivity.NetworkAccess != NetworkAccess.Internet)
{
    await Shell.Current.DisplayAlert("No connectivity!", $"Please check internet and try again.", "OK");
    return;
}
```

&nbsp;

## Polly Retry Policy
NuGet pakken **Polly** tilføjes MAUI projektet.

I *GenericRepository.cs* og `GetAsync()` metoden tilføjes en `Policy` med en `RetryAsync` metode:

```csharp
HttpResponseMessage response = await Policy
    .HandleResult<HttpResponseMessage>(res => !res.IsSuccessStatusCode)
    .RetryAsync(10)
```

I **ItemWebApi** projektet, *Program* klassen og metoden `GetAllTodos()` kastes et tilfældigt tal mellem 1 og 100.
Tallet sammenlignes med det tal, som er skrevet ind i variablen èrrorPercent`.
Er tallet 0 returneres altid HTTP 200, jo større værdi jo større chanche for fejl. 
Vælges 100 vil den hver gang returnere HTTP 500. Status logges i Terminal-vinduet.

Test: sæt errorPercent = 80, start begge projekter og kør app'en. Se i terminalen at der nogle gange laves flere forsøg inden statuskode 200 opnås. Dog maks. 10 forsøg.

#### Med TimeSpan
Nu ændres `RetryAsync` til at tage en `TimeSpan` som parameter: 
```csharp
HttpResponseMessage response = await Policy
    .HandleResult<HttpResponseMessage>(res => !res.IsSuccessStatusCode)
    .WaitAndRetryAsync(retryCount: 5, retryAttempt => TimeSpan.FromSeconds(3))
```
Bemærk at der nu går 3 sekunder mellem hvert forsøg.

#### Med Exponential TimeSpan
Nu ændres `WaitAndRetryAsync` til at beregne en `TimeSpan` som bliver større for hver gang: 
```csharp
HttpResponseMessage response = await Policy
    .HandleResult<HttpResponseMessage>(res => !res.IsSuccessStatusCode)
    .WaitAndRetryAsync(retryCount: 5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
```

Man kan tilføje en delegatemetode, som udskriver TimeSpan:
```csharp
HttpResponseMessage response = await Policy
    .HandleResult<HttpResponseMessage>(res => !res.IsSuccessStatusCode)
.WaitAndRetryAsync
(
    retryCount: 5,
    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
    onRetry: (ex, time) =>
    {
        Debug.WriteLine($"--> TimeSpan: {time.TotalSeconds}");
    }
```
Kig i Output-vinduet: Her ser man tydeligt at der går længere og længere tid mellem hvert forsøg: 2, 4, 8, 16 og 32 sekunder. 
&nbsp;

#### With ClientPolicy
Der er oprettet en folder kaldet *Policies* og en klasse `ClientPolicy.cs`  med de forskellige udgaver af Policies.

Husk at registrere `ClientPolicy` i *Startup.cs*:
```csharp
builder.Services.AddSingleton(new ClientPolicy());
```

I *GenericRepository.cs* og `GetAsync()` metoden bruges nu `ClientPolicy`:
```csharp
HttpResponseMessage response = await _clientPolicy.LoggingExponentialHttpRetry.ExecuteAsync(() =>
_client.GetAsync(uri));
```
Test og se at det virker som før!

&nbsp;

#### With Polly Caching
Installér nu NuGet pakken **Polly.Caching.Memory** i MAUI projektet.

Følgende kode registreres i MauiProgram.cs:
```csharp
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
```

Tilføj følgende:
```csharp
HttpResponseMessage response = await cachePolicy.ExecuteAsync(context => _client.GetAsync(uri), new Context("FooKey"));
```

Sæt errorPercent = 0 i WebApi og sæt et breakpoint i `GetAllTodos`.
Kør solution og se at der kun standses på breakpoint i Api første gang og derefter kun når der er gået 10 sekunder!
&nbsp;


 