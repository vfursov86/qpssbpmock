using Prometheus;
using Microsoft.Extensions.Logging;
using static Microsoft.Extensions.Logging.Log4NetExtensions;
//--------------------------------------------------------------
var builder = WebApplication.CreateBuilder(args);

// Disable standard logger provider.
builder.Logging.ClearProviders();
builder.Logging.AddLog4Net();

// See more Kestrel server options https://learn.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel/endpoints?view=aspnetcore-9.0
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(3);
    serverOptions.Limits.MaxConcurrentConnections = 200;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseResponseCaching();

// Add Prometheus exporter.
app.UseHttpMetrics();

app.MapControllers();
app.Run();