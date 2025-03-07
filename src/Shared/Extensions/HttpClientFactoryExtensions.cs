using Polly.Extensions.Http;
using Polly;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Extensions
{
    public static class HttpClientFactoryExtensions
    {
        public static IHttpClientBuilder AddRetryPolicy(this IHttpClientBuilder builder)
        {
            var policy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                    retryCount: 5,
                    sleepDurationProvider: (retryAttempt) => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            return builder.AddPolicyHandler(policy);
        }
    }
}
