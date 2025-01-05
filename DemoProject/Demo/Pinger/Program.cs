using Microsoft.Extensions.Logging.Abstractions;
using Monitoring;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
MonitorService.Log.Information( "Mapping OpenApi"); 
app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "Pinger API V1");
});

MonitorService.Log.Information( "About to run Pinger");
app.MapControllers();
app.Run();
Log.CloseAndFlush();
