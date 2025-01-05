using Microsoft.AspNetCore.Mvc;
using Monitoring;
using Ponger.Models;
using Ponger.Services;

namespace Ponger.Controllers;

[ApiController]
public class PongerController: ControllerBase
{
    private readonly PongerService _service;
    
    public PongerController()
        => _service = new PongerService();

    [HttpGet]
    [Route("/ping")]
    public async Task<SampleResponse> Ping()
    {
        MonitorService.Log.Information("Recieved a ping, answering with a pong");
        return await _service.Pong();
    }
        
    
}