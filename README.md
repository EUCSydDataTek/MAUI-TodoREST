# 3.NetworkResiliencePolly

## Kontrol af netv�rksadgang med Connectivity

`Connectivity` konfigureres i *Startup.cs* i MAUI projektet.
I *MainPageViewModel* laves et check af om der er netv�rksadgang.

```csharp
if (connectivity.NetworkAccess != NetworkAccess.Internet)
{
    await Shell.Current.DisplayAlert("No connectivity!", $"Please check internet and try again.", "OK");
    return;
}
```

&nbsp;

## Polly Retry Policy
NuGet pakken **Polly** tilf�jes MAUI projektet.

I *GenericRepository.cs* og `GetAsync()` metoden tilf�jes en `Policy` med en `RetryAsync` metode:

```csharp
HttpResponseMessage response = await Policy
    .HandleResult<HttpResponseMessage>(res => !res.IsSuccessStatusCode)
    .RetryAsync(10)
```

I **ItemWebApi** projektet, *Program* klassen og metoden `GetAllTodos()` kastes et tilf�ldigt tal mellem 1 og 100.
Tallet sammenlignes med det tal, som er skrevet ind i variablen �rrorPercent`.
Er tallet 0 returneres altid HTTP 200, jo st�rre v�rdi jo st�rre chanche for fejl. 
V�lges 100 vil den hver gang returnere HTTP 500. Status logges i Terminal-vinduet.

Test: s�t errorPercent = 80, start begge projekter og k�r app'en. Se i terminalen at der nogle gange laves flere fors�g inden statuskode 200 opn�s. Dog maks. 10 fors�g.

#### Med TimeSpan
Nu �ndres `RetryAsync` til at tage en `TimeSpan` som parameter: 
```csharp
HttpResponseMessage response = await Policy
    .HandleResult<HttpResponseMessage>(res => !res.IsSuccessStatusCode)
    .WaitAndRetryAsync(retryCount: 5, retryAttempt => TimeSpan.FromSeconds(3))
```
Bem�rk at der nu g�r 3 sekunder mellem hvert fors�g.

#### Med Exponential TimeSpan
Nu �ndres `WaitAndRetryAsync` til at beregne en `TimeSpan` som bliver st�rre for hver gang: 
```csharp
HttpResponseMessage response = await Policy
    .HandleResult<HttpResponseMessage>(res => !res.IsSuccessStatusCode)
    .WaitAndRetryAsync(retryCount: 5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
```

Man kan tilf�je en delegatemetode, som udskriver TimeSpan:
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
Kig i Output-vinduet: Her ser man tydeligt at der g�r l�ngere og l�ngere tid mellem hvert fors�g: 2, 4, 8, 16 og 32 sekunder. 
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
Test og se at det virker som f�r!

&nbsp;

#### With Polly Caching
Install�r nu NuGet pakken **Polly.Caching.Memory** i MAUI projektet.

F�lgende kode registreres i MauiProgram.cs:
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

Tilf�j f�lgende:
```csharp
HttpResponseMessage response = await cachePolicy.ExecuteAsync(context => _client.GetAsync(uri), new Context("FooKey"));
```

S�t errorPercent = 0 i WebApi og s�t et breakpoint i `GetAllTodos`.
K�r solution og se at der kun standses p� breakpoint i Api f�rste gang og derefter kun n�r der er g�et 10 sekunder!
&nbsp;


 