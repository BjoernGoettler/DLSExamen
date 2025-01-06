using System.Diagnostics;
using EasyNetQ;
using EasyNetQ.DI;
using EasyNetQ.Serialization.NewtonsoftJson;
using FeatureHubSDK;
using Monitoring;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using Polly;
using Polly.CircuitBreaker;
using Serilog;
using SharedMessages;
namespace Pinger.Services;

public class PingerService
{
    private readonly ResiliencePipeline<SampleResponse> _policy;
    private EdgeFeatureHubConfig _featureHubConfig;
    private IClientContext fh;
    
     
    public PingerService()
    {
        FeatureLogging.DebugLogger += (sender, s)
            => MonitorService.Log.Here().Debug("Feature debugger " + s);
        FeatureLogging.TraceLogger += (sender, s)
            => MonitorService.Log.Here().Information("Feature tracer " + s);
        FeatureLogging.InfoLogger += (sender, s)
            => MonitorService.Log.Here().Information("Feature info " + s);
        FeatureLogging.ErrorLogger += (sender, s)
            => MonitorService.Log.Here().Error("Feature error " + s);
        _featureHubConfig = new EdgeFeatureHubConfig("http://featurehub:8085", "2b1728cf-42b5-45c0-a947-2d8120d441ff/K5q2tfawPkZ1JMdYSku1UGlUb10rD0w01l4PHv3J");
        
        
        
        _policy = new ResiliencePipelineBuilder<SampleResponse>()
            .AddTimeout(TimeSpan.FromSeconds(30))
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions<SampleResponse>
            {
                MinimumThroughput = 3,
                BreakDuration = TimeSpan.FromSeconds(60),
                ShouldHandle = args => ValueTask.FromResult(args.Outcome.Result == null || !args.Outcome.Result.Success),
                OnOpened = circuitState =>
                {
                    MonitorService.Log.Here().Warning("Circuit breaker opened due to failures");
                    return ValueTask.CompletedTask;
                },
                OnClosed = circuitState =>
                {
                    MonitorService.Log.Here().Information("Circuit breaker closed, service recovered");
                    return ValueTask.CompletedTask;
                }
            })
            .Build();
   
    }
    
    
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

    public async Task<SampleResponse> ConsumePower()
    {
        MonitorService.Log.Information("Checking if we are allowed to consume power");
        fh = await _featureHubConfig.NewContext().Build();
        var isPowerPriceNegative = fh["negativepowerprice"].IsEnabled;
        MonitorService.Log.Information($"FeatureHub context built {fh["negativepowerprice"]}");
        MonitorService.Log.Here().Information("Consuming power if allowed");
        SampleResponse PowerResponse;
        if (isPowerPriceNegative)
        {
            MonitorService.Log.Here().Error("We are allowed to consume power");
            PowerResponse = new SampleResponse
            {
                Message = "We are consuming power",
                Success = true
            };
        }
        else
        {
            MonitorService.Log.Here().Error("We are not allowed to consume power");
            PowerResponse = new SampleResponse
            {
                Message = "We are not consuming power",
                Success = true
            };
            
        }
        return await Task.FromResult(PowerResponse);
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
                SampleResponse result;
                try
                {
                    result = await _policy.ExecuteAsync(async ct => await bus.Rpc.RequestAsync<PongerRequest, SampleResponse>(request, ct));
                }
                catch
                {
                    result = await Task.FromResult(
                        new SampleResponse
                        {
                            Message = "Ponger is not responding",
                            Success = false,
                            Error = "Ponger is not responding"

                        }
                    );
                }
                
                return result;
            }
            
        }
        
    }
}