using Microsoft.AspNetCore.Mvc;
using Monitoring;
using Pinger.Services;
using SharedMessages;

namespace Pinger.Controllers;

[ApiController]
public class PingerController: ControllerBase
{
    private readonly PingerService _service;
    
    
    public PingerController()
    {
        _service = new PingerService();
    }
    
    [HttpGet]
    [Route("/heartbeat")]
    public async Task<SampleResponse> Heartbeat()
    {
        MonitorService.Log.Information("Heartbeat requested");
        return await _service.Heartbeat(); 
    }
    
    [HttpGet]
    [Route("/stopwatch")]
    public async Task<SampleResponse> StopWatch()
    {
        MonitorService.Log.Information("Stopwatch requested");
        return await _service.StopWatch(); 
    }

    [HttpGet]
    [Route("/ping")]
    public async Task<SampleResponse> Ping()
    {
        MonitorService.Log.Information("Pinging Ponger");
        return await _service.Ping();
    }
    
    [HttpGet]
    [Route("/consumepower")]
    public async Task<SampleResponse> ConsumePower()
    {
        MonitorService.Log.Information("Considering some poser ");
        return await _service.ConsumePower();
    }
}