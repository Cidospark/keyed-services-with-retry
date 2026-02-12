using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeyedServicesWithRetry.Models.Entities
{
    public class ProviderMetricsModel
    {
        public int SuccessCount { get; private set; }
        public int FailureCount { get; private set; }

        public void RecordSuccess() => SuccessCount++;
        public void RecordFailure() => FailureCount++;
    }
}