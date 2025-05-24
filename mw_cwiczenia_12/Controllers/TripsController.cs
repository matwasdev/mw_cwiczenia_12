using Microsoft.AspNetCore.Mvc;
using mw_cwiczenia_12.Service;

namespace mw_cwiczenia_12.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TripsController : ControllerBase
{

    public readonly IDbService _dbService;

    public TripsController(IDbService dbService)
    {
        _dbService = dbService;
    }


    [HttpGet]
    public async Task<IActionResult> GetTrips(int page = 1, int pageSize = 10)
    {
        try
        {
            var trips = await _dbService.GetTripsAsync(page, pageSize);
            return Ok(trips);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
}