using Monitoring;
using Pinger.Models;
using Serilog;

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
}