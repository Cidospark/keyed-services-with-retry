using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyedServicesWithRetry.Services;
using KeyedServicesWithRetry.Services.ProviderMetrics;

namespace KeyedServiceWithRetry.Services
{
    public class PaymentProcessorService
    {
        private readonly PaymentProviderSelector _selector;
        private readonly IPaymentExecutionEngine _engine;
        private readonly IProviderMetricsRegistry _metrics;

        public PaymentProcessorService(IProviderMetricsRegistry metrics, 
            PaymentProviderSelector selector, IPaymentExecutionEngine engine)
        {
            _selector = selector;
            _engine = engine;
            _metrics = metrics;
        }

        public async Task ProcessPaymentAsync(decimal amount)
        {
            var provider = await _selector.GetHealthyProviderAsync();
            var providerMetrics = _metrics.Get(provider.Name);
            try
            {
                await _engine.ExecuteAsync(provider, amount);
                providerMetrics.RecordSuccess();
            }
            catch
            {
                providerMetrics.RecordFailure();
                throw;
            }
        }

        
    }
}

//   public class PaymentProcessorService
//     {

//         private readonly IServiceProvider _serviceProvider;
//         private readonly string[] _priorityOrder =
//         {
//             "flutterwave",
//             "paystack"
//         };
//         public PaymentProcessorService(IServiceProvider serviceProvider)
//         {
//             _serviceProvider = serviceProvider;
//         }

//         public async Task ProcessPaymentAsync(decimal amount)
//         {
//             var provider = await GetHealthyProviderAsync();
//             await provider.ProcessAsync(amount);
//         }

//         private async Task<IPaymentService> GetHealthyProviderAsync()
//         {
//             foreach (var key in _priorityOrder)
//             {
//                 var provider = _serviceProvider.GetRequiredKeyedService<IPaymentService>(key);
//                 if (await provider.IsHealthyAsync())
//                 {
//                     return provider;
//                 }
//             }
//             throw new InvalidOperationException("No healthy payment providers available");
//         }
        
//     }