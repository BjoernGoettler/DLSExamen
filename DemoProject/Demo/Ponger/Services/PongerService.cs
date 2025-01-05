using System.Security.AccessControl;
using Monitoring;
using Ponger.Models;

namespace Ponger.Services;

public class PongerService
{
    public async Task<SampleResponse> Pong()
    {
        using(var activity = MonitorService.ActivitySource.StartActivity())
        {
            MonitorService.Log.Here().Debug("Pong requested");
            return await Task.FromResult(new SampleResponse
            {
                Message = "Here is your pong",
                Success = true
            });
        }
        
    }
}