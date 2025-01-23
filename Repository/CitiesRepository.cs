using Microsoft.EntityFrameworkCore;
using test.Data;
using test.DTO;
using test.Models;
using test.Repository.Interface;

namespace test.Repository
{
    public class CitiesRepository : ICitiesRepository
    {
        private readonly TestDb _context;
        public CitiesRepository(TestDb context)
        {
            _context = context;
        }

        public async Task<bool> CityExists(int countryId, string cityName)
        {
            return await _context.Cities.AnyAsync(c => c.CountryId == countryId && c.CityName == cityName);
        }

        public async Task CreateCity(CitiesDTO citiesDTO)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var city = new City
                    {
                        CountryId = citiesDTO.CountryId,
                        CityName = citiesDTO.CityName
                    };
                    _context.Add(city);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
