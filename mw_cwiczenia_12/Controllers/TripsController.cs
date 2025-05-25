using Microsoft.AspNetCore.Mvc;
using mw_cwiczenia_12.DTOs;
using mw_cwiczenia_12.Exceptions;
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
            return StatusCode(500, "Internal server error occured");
        }
    }

    [HttpPost]
    [Route("{idTrip}/clients")]
    public async Task<IActionResult> RegisterClientForTripAsync(int idTrip, RegisterClientForTripDto request)
    {
        try
        {
            await _dbService.RegisterClientForTripAsync(idTrip, request);
            return Created();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Internal server error occured");
        }
    }
    
}