using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyedServicesWithRetry.Models;
using Microsoft.Extensions.Options;

namespace KeyedServiceWithRetry.Services
{
    public class PaymentProviderSelector
    {
        private readonly Func<string, IPaymentService> _resolver;
        private readonly string[] _priorityOrder;

        public PaymentProviderSelector(Func<string, IPaymentService> resolver, IOptions<PaymentProviderOptions> options)
        {
            _resolver = resolver;
            _priorityOrder = options.Value.PriorityOrder;
        }
        
        public async Task<IPaymentService> GetHealthyProviderAsync()
        {
            foreach (var key in _priorityOrder)
            {
                var provider = _resolver(key);
                if (await provider.IsHealthyAsync())
                {
                    return provider;
                }
            }
            throw new InvalidOperationException("No healthy payment providers available");
        }
    }
}