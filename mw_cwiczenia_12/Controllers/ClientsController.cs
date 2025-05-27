using Microsoft.AspNetCore.Mvc;
using mw_cwiczenia_12.Exceptions;
using mw_cwiczenia_12.Service;

namespace mw_cwiczenia_12.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly IDbService _dbService;

    public ClientsController(IDbService dbService)
    {
        _dbService = dbService;
    }


    [HttpDelete]
    [Route("{idClient}")]
    public async Task<IActionResult> RemoveClient(int idClient)
    {
        try
        {
            await _dbService.RemoveClientAsync(idClient);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Internal Server Error occured");
        }
    }
    
    
}