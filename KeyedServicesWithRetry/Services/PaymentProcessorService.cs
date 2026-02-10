using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyedServicesWithRetry.Services;

namespace KeyedServiceWithRetry.Services
{
    public class PaymentProcessorService
    {
        private readonly PaymentProviderSelector _selector;
        private readonly IPaymentExecutionEngine _engine;
        public PaymentProcessorService(PaymentProviderSelector selector, IPaymentExecutionEngine engine)
        {
            _selector = selector;
            _engine = engine;
        }

        public async Task ProcessPaymentAsync(decimal amount)
        {
            var provider = await _selector.GetHealthyProviderAsync();
            await _engine.ExecuteAsync(provider, amount);
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