using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyedServiceWithRetry.Services;

namespace KeyedServicesWithRetry.Services
{
    public interface IPaymentExecutionEngine
    {
        Task ExecuteAsync(IPaymentService provider, decimal amount);
    }
}