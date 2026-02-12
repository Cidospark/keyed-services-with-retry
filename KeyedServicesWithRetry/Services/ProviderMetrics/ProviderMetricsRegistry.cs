using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyedServicesWithRetry.Models.Entities;

namespace KeyedServicesWithRetry.Services.ProviderMetrics
{
    public class ProviderMetricsRegistry : IProviderMetricsRegistry
    {
        private readonly ConcurrentDictionary<string, ProviderMetricsModel> _metrics = new();
        public ProviderMetricsModel Get(string providerName)
        {
            return _metrics.GetOrAdd(providerName, _ => new ProviderMetricsModel());
        }
    }
}