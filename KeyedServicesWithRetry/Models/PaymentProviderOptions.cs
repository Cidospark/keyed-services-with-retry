using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeyedServicesWithRetry.Models
{
    public class PaymentProviderOptions
    {
        public string[] PriorityOrder { get; set; } = [];
    }
}