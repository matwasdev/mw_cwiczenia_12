using Microsoft.EntityFrameworkCore;
using mw_cwiczenia_12.Data;
using mw_cwiczenia_12.DTOs;

namespace mw_cwiczenia_12.Service;

public class DbService : IDbService
{
    private readonly _2019sbdContext _context;

    public DbService(_2019sbdContext context)
    {
        _context = context;
    }

    public async Task<TripsInfoDto> GetTripsAsync(int page, int pageSize)
    {
        
        TripsInfoDto tripsWithInfoDto = new TripsInfoDto();
        tripsWithInfoDto.PageNum = page;
        tripsWithInfoDto.PageSize = pageSize;
        tripsWithInfoDto.AllPages = await _context.Trips.CountAsync();

        var trips = await _context.Trips
            .Include(ct => ct.IdCountries)
            .Include(clT => clT.ClientTrips)
                .ThenInclude(c => c.IdClientNavigation)
            .OrderByDescending(a => a.DateFrom)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(a => new TripDto
            {
                Name = a.Name,
                Description = a.Description,
                DateFrom = a.DateFrom,
                DateTo = a.DateTo,
                MaxPeople = a.MaxPeople,
                Countries = a.IdCountries.Select(c => new CountryDto
                {
                    Name = c.Name
                }).ToList(),
                Clients = a.ClientTrips.Select(ct => new ClientDto
                {
                    FirstName = ct.IdClientNavigation.FirstName,
                    LastName = ct.IdClientNavigation.LastName,
                }).ToList()
            }).ToListAsync();
        
        tripsWithInfoDto.Trips = trips;
        
        return tripsWithInfoDto;
    }
    
    
}