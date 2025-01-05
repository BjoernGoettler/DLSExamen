using System.Diagnostics;
using System.Security.AccessControl;
using EasyNetQ;
using Monitoring;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using SharedMessages;

namespace Ponger.Services;

public class PongerService
{
    public async Task<SampleResponse> Pong()
    {
        var bus = RabbitHutch.CreateBus("host=rabbitmq;username=mquser;password=verysecret");
        bus.Rpc.RespondAsync<PongerRequest, SampleResponse>(req =>
            {
                var propagator = new TraceContextPropagator();
                var parentContext = propagator.Extract(default, req,
                    (carrier, key) =>
                    {
                        return new List<string>(new[]
                            { carrier.Header.ContainsKey(key) ? carrier.Header[key].ToString() : string.Empty });
                    });
                Baggage.Current = parentContext.Baggage;

                using var activity = MonitorService.ActivitySource.StartActivity("Message received",
                    ActivityKind.Consumer,
                    parentContext.ActivityContext);

                return Task.FromResult<SampleResponse>(new SampleResponse
                {
                    Message = "Here is your pong",
                    Success = true
                });
            }
        );
        return await Task.FromResult<SampleResponse>(new SampleResponse
        {
            Message = "Here is your pong",
            Success = true
        });
    }
}