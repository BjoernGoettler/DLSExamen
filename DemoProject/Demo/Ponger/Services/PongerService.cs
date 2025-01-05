using Monitoring;
using Ponger.Models;

namespace Ponger.Services;

public class PongerService
{
    public async Task<SampleResponse> Pong()
    {
        MonitorService.Log.Information("Pong requested");
        return await Task.FromResult(new SampleResponse
        {
            Message = "Here is your pong",
            Success = true
        });
    }
}