using KeyedServicesWithRetry.Models;
using KeyedServicesWithRetry.Services;
using KeyedServicesWithRetry.Services.ProviderMetrics;
using KeyedServiceWithRetry.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentFallbackTests.Services;

namespace PaymentFallback;

[TestFixture]
public class Tests
{
    private ServiceProvider _provider;

    [SetUp]
    public void Setup()
    {
        var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.Development.json", optional: false)
        .Build();

        var services = new ServiceCollection();
        services.Configure<PaymentProviderOptions>(configuration.GetSection("PaymentProviders"));
        services.AddKeyedScoped<IPaymentService, PaystackPaymentService>("paystack");
        services.AddKeyedScoped<IPaymentService, FlutterwavePaymentService>("flutterwave");
        services.AddScoped<PaymentProviderSelector>();

        services.AddScoped<Func<string, IPaymentService>>(sp => key => sp.GetRequiredKeyedService<IPaymentService>(key));

        services.AddSingleton<IPaymentExecutionEngine, PaymentExecutionEngine>();
        services.AddSingleton<IProviderMetricsRegistry, ProviderMetricsRegistry>();
        services.AddScoped<PaymentProcessorService>();

        _provider = services.BuildServiceProvider();
        //or  await using var provider = services.BuildServiceProvider();

    }

    [Test]
    public async Task Should_fallback_to_next_provider_when_first_fails()
    {
        var processor = _provider.GetRequiredService<PaymentProcessorService>();

        var metrics = _provider.GetRequiredService<IProviderMetricsRegistry>();

        Assert.DoesNotThrowAsync(async () =>
        {
            await processor.ProcessPaymentAsync(1000);
        });


        Assert.That(metrics.Get("flutterwave").FailureCount, Is.EqualTo(0));
        Assert.That(metrics.Get("flutterwave").SuccessCount, Is.EqualTo(1));
    }

    [TearDown]
    public async Task TearDown()
    {
        if (_provider is IAsyncDisposable asyncDisposable)
            await asyncDisposable.DisposeAsync();
        else
            _provider?.Dispose();
    }
}
