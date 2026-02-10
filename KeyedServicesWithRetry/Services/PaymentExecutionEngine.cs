using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyedServiceWithRetry.Services;
using Polly;
using Polly.Wrap;

namespace KeyedServicesWithRetry.Services
{
    public class PaymentExecutionEngine : IPaymentExecutionEngine
    {
        private readonly AsyncPolicyWrap _policy;
        public PaymentExecutionEngine()
        {
            var retryPolicy = Policy.Handle<Exception>()
                                    .WaitAndRetryAsync(retryCount:3, 
                                                       sleepDurationProvider: attempt => TimeSpan.FromMicroseconds(200 * attempt));
            var fallbackPolicy = Policy.Handle<Exception>()
                                 .FallbackAsync(async ct => Console.WriteLine("⚠️ Provider failed after retries")); 

            _policy = Policy.WrapAsync(retryPolicy, fallbackPolicy);
        }
        public async Task ExecuteAsync(IPaymentService provider, decimal amount)
        {
            await _policy.ExecuteAsync(async () =>
            {
                await provider.ProcessAsync(amount);
            });
        }
    }
}