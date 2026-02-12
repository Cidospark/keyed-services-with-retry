using KeyedServiceWithRetry.Services;

namespace PaymentFallbackTests.Services
{
    public class FailingPaymentService : IPaymentService
    {
        public string Name => "fail";

        public Task<bool> IsHealthyAsync() => Task.FromResult(true);

        public Task ProcessAsync(decimal amount)
            => throw new Exception("Boom");
    }
}