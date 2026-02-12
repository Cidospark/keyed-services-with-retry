using KeyedServicesWithRetry.Middleware;
using KeyedServicesWithRetry.Models;
using KeyedServicesWithRetry.Services;
using KeyedServicesWithRetry.Services.ProviderMetrics;
using KeyedServiceWithRetry.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.Configure<PaymentProviderOptions>(builder.Configuration.GetSection("PaymentProviders"));
builder.Services.AddKeyedScoped<IPaymentService, PaystackPaymentService>("paystack");
builder.Services.AddKeyedScoped<IPaymentService, FlutterwavePaymentService>("flutterwave");
builder.Services.AddScoped<PaymentProviderSelector>();
builder.Services.AddScoped<PaymentProcessorService>();
builder.Services.AddScoped<Func<string, IPaymentService>>(
    sp => key => sp.GetRequiredKeyedService<IPaymentService>(key) // resolver delegate
);
builder.Services.AddScoped<IPaymentExecutionEngine, PaymentExecutionEngine>();
builder.Services.AddSingleton<IProviderMetricsRegistry, ProviderMetricsRegistry>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<GlobalExceptionHandler>();

app.Run();
