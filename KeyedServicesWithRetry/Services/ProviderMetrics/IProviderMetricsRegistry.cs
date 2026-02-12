using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyedServicesWithRetry.Models.Entities;

namespace KeyedServicesWithRetry.Services.ProviderMetrics
{
    public interface IProviderMetricsRegistry
    {
        ProviderMetricsModel Get(string providerName);
    }
}