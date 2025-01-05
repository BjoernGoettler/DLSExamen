using System.Diagnostics;
using EasyNetQ;
using EasyNetQ.DI;
using EasyNetQ.Serialization.NewtonsoftJson;
using Monitoring;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using Serilog;
using SharedMessages;

namespace Pinger.Services;

public class PingerService
{
    public async Task<SampleResponse> Heartbeat()
        => await Task.FromResult(new SampleResponse
        {
            Message = "We are alive",
            Success = true
        });

    public async Task<SampleResponse> StopWatch()
    {
        using(MonitorService.Log.Here().BeginTimedOperation("Measure sleep"))
        {
            var random = new Random();
            var sleepTime = random.Next(1000);
            MonitorService.Log.Information("Sleeping for {SleepTime}ms", sleepTime);
            Thread.Sleep(sleepTime);
        }
        return await Task.FromResult(new SampleResponse
        {
            Message = "We took a nap",
            Success = true
        });
    }
    public async Task<SampleResponse> Ping()
    {
        using (var activity = MonitorService.ActivitySource.StartActivity())
        {
            MonitorService.Log.Here().Error("Ping requested");
            using(MonitorService.Log.Here().BeginTimedOperation("Measure Ponger service"))
            {
                var bus = RabbitHutch.CreateBus("host=rabbitmq;username=mquser;password=verysecret",
                    x => x.Register<ISerializer, NewtonsoftJsonSerializer>());
                var request = new PongerRequest();
                
                var activityContext = activity?.Context ?? Activity.Current?.Context ?? default;
                var propagationContext = new PropagationContext(activityContext, Baggage.Current);
                var propagator = new TraceContextPropagator();
                
                propagator.Inject(propagationContext
                    , request
                    , (carrier, key, value)
                        =>
                    {
                        carrier.Header.Add(key, value);
                    }
                    );
                
                //var client = new HttpClient();
                MonitorService.Log.Information("Pinging the ponger");
                var result = await bus.Rpc.RequestAsync<PongerRequest, SampleResponse>(request);
                //var response = await client.GetFromJsonAsync<SampleResponse>("http://pongservice:8080/ping");
                return result;
            }
            
        }
        
    }
}