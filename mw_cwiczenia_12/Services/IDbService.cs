using mw_cwiczenia_12.DTOs;

namespace mw_cwiczenia_12.Service;

public interface IDbService
{
    Task<TripsInfoDto> GetTripsAsync(int page, int pageSize);
    Task RemoveClientAsync(int idClient);
    Task RegisterClientForTripAsync(int idTrip, RegisterClientForTripDto request);
    
}