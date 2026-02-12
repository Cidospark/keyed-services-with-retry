using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyedServiceWithRetry.Services;

namespace PaymentFallbackTests.Services
{
    public class SuccessfulPaymentService : IPaymentService
    {
        public string Name => "success";

        public Task<bool> IsHealthyAsync() => Task.FromResult(true);

        public Task ProcessAsync(decimal amount)
            => Task.CompletedTask;
    }
}