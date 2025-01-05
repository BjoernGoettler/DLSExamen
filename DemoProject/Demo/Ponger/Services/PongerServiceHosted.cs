using EasyNetQ;
using SharedMessages;
using Monitoring;

namespace Ponger.Services;

public class PongerServiceHosted : BackgroundService
{
    private IBus _bus;
    
    public PongerServiceHosted(IBus bus)
        => _bus = bus;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        MonitorService.Log.Information("Starting PongerServiceHosted...");

        // Initialize the RabbitMQ Bus connection
        //_bus = RabbitHutch.CreateBus("host=rabbitmq;username=mquser;password=verysecret");

        // Set up the RPC responder
        await _bus.Rpc.RespondAsync<PongerRequest, SampleResponse>(req =>
        {
            Console.WriteLine("Received request: {0}", req);

            // Return a response containing the Pong message
            return Task.FromResult(new SampleResponse
            {
                Message = "Here is your pong!",
                Success = true
            });
        }, stoppingToken);

        MonitorService.Log.Information("Listening for PongerRequest messages...");

        // Keep service alive until cancellation is requested
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(500, stoppingToken); // Delay prevents busy waiting
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        MonitorService.Log.Information("Stopping PongerServiceHosted...");
        _bus?.Dispose();
        return base.StopAsync(cancellationToken);
    }
}