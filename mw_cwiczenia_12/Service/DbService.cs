using Microsoft.EntityFrameworkCore;
using mw_cwiczenia_12.Data;
using mw_cwiczenia_12.DTOs;
using mw_cwiczenia_12.Exceptions;
using mw_cwiczenia_12.Models;

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

    public async Task RemoveClientAsync(int idClient)
    {
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.IdClient == idClient);
        if (client==null)
            throw new NotFoundException($"Client with id {idClient} not found");
        
        bool hasAnyTrip = await _context.ClientTrips.AnyAsync(ct => ct.IdClient==idClient);
        if(!hasAnyTrip)
            throw new BadRequestException($"Client with id {idClient} has trips, cannot be removed.");

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
    }

    public async Task RegisterClientForTripAsync(int idTrip, RegisterClientForTripDto request)
    {
        
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == request.Pesel);

        var isAlreadyRegistered = await _context.ClientTrips.AnyAsync(ct => ct.IdClient == client.IdClient);
        if (isAlreadyRegistered)
            throw new ConflictException($"Client with id {client.IdClient} is already registered for trip {idTrip}");

        var trip = await _context.Trips.FirstOrDefaultAsync(t => t.IdTrip == idTrip);
        if(trip==null)
            throw new NotFoundException($"Trip with id {idTrip} not found");
        
        if(trip.DateFrom < DateTime.Now)
            throw new BadRequestException($"Trip with id {idTrip} is in the past, cannot register");

        ClientTrip clientTrip = new ClientTrip
        {
            IdClient = client.IdClient,
            IdTrip = trip.IdTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = request.PaymentDate,
        };
        
        _context.ClientTrips.Add(clientTrip);
        await _context.SaveChangesAsync();
    }
    
    
}