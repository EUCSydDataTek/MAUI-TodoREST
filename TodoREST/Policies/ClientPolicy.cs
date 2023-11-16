using Polly;
using Polly.Retry;
using System.Diagnostics;

namespace TodoREST.Policies;
public class ClientPolicy
{
    public AsyncRetryPolicy<HttpResponseMessage> ImmediateHttpRetry { get; }
    public AsyncRetryPolicy<HttpResponseMessage> LinearHttpRetry { get; }
    public AsyncRetryPolicy<HttpResponseMessage> ExponentialHttpRetry { get; }
    public AsyncRetryPolicy<HttpResponseMessage> LoggingExponentialHttpRetry { get; }

    public ClientPolicy()
    {
        ImmediateHttpRetry = Policy.HandleResult<HttpResponseMessage>(
            res => !res.IsSuccessStatusCode)
            .RetryAsync(10);

        LinearHttpRetry = Policy.HandleResult<HttpResponseMessage>(
            res => !res.IsSuccessStatusCode)
            .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(3));

        ExponentialHttpRetry = Policy.HandleResult<HttpResponseMessage>(
            res => !res.IsSuccessStatusCode)
            .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        LoggingExponentialHttpRetry = Policy.HandleResult<HttpResponseMessage>(
            res => !res.IsSuccessStatusCode)
            .WaitAndRetryAsync
            (
                retryCount: 5,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (ex, time) =>
                {
                    Debug.WriteLine($"========================> TimeSpan: {time.TotalSeconds}");
                }
            );
    }
}
