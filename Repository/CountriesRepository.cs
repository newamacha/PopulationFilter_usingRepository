using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using test.Data;
using test.DTO;
using test.Models;
using test.Repository.Interface;

namespace test.Repository
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly TestDb _context;

        public CountriesRepository(TestDb context)
        {
            _context = context;
        }

        public async Task<bool> CountryExists(string countryName)
        {
            return await _context.Countries.AnyAsync(c => c.CountryName == countryName);
        }

        public async Task CreateCountry(CountriesDTO countriesDTO)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var country = new Country
                    {
                        CountryName = countriesDTO.CountryName
                    };
                    _context.Add(country);
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