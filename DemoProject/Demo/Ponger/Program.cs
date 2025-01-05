using EasyNetQ;
using Ponger.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IBus>(
    provider => RabbitHutch.CreateBus("host=rabbitmq;username=mquser;password=verysecret"));

builder.Services.AddHostedService<PongerServiceHosted>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.MapControllers();
app.Run();