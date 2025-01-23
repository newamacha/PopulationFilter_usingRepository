using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using test.Data;
using test.DTO;
using test.Models;
using test.Repository.Interface;

namespace test.Repository
{
    public class PopulationRepository : IPopulationRepository
    {
        private readonly TestDb _context;
        public PopulationRepository(TestDb context)
        {
            _context = context;
        }

        public async Task AddPopulationAsync(Population population)
        {
            await _context.Populations.AddAsync(population);
        }

        public async Task<Population> GetExistingPopulationAsync(int countryId, int cityId, string ageGroup)
        {
            return await _context.Populations.FirstOrDefaultAsync(p => p.CountryID == countryId && p.CityId == cityId && p.AgeGroup == ageGroup);
        }

        public async Task UpdatePopulationAsync(Population population)
        {
            _context.Populations.Update(population);
        }

        public async Task<List<PopulationSummaryDTO>> GetPopulationSummaryAsync(int? countryId, int? cityId, string populationType)
        {
            var populations = _context.Populations
                .Include(p => p.City)
                .ThenInclude(c => c.Country)
                .AsQueryable();

            if (countryId.HasValue)
                populations = populations.Where(p => p.City.CountryId == countryId);

            if (cityId.HasValue)
                populations = populations.Where(p => p.CityId == cityId);

            if (!string.IsNullOrEmpty(populationType))
                populations = populations.Where(p => p.AgeGroup == populationType);

            return await populations
                .GroupBy(p => p.AgeGroup)
                .Select(g => new PopulationSummaryDTO
                {
                    PopulationType = g.Key,
                    TotalNumber = g.Sum(p => p.MalePopulation + p.FemalePopulation)
                }).ToListAsync();
        }

        public async Task<List<TopCountryDTO>> GetTopCountriesAsync(int topCount)
        {
            return await _context.Countries
                .Select(c => new TopCountryDTO
                {
                    CountryName = c.CountryName,
                    Population = c.Cities.Sum(ci => ci.Populations.Sum(p => p.MalePopulation + p.FemalePopulation))
                })
                .OrderByDescending(c => c.Population)
                .Take(topCount)
                .ToListAsync();
        }

        public async Task<List<GroupedPopulationData>> FilterPopulationsAsync(int? countryId, int? cityId, string populationType)
        {
            var populations = _context.Populations
                .Include(p => p.City)
                .ThenInclude(c => c.Country)
                .AsQueryable();

            if (countryId.HasValue)
                populations = populations.Where(p => p.City.CountryId == countryId);

            if (cityId.HasValue)
                populations = populations.Where(p => p.CityId == cityId);

            if (!string.IsNullOrEmpty(populationType))
                populations = populations.Where(p => p.AgeGroup == populationType);

            return await populations
                .GroupBy(p => p.AgeGroup)
                .Select(g => new GroupedPopulationData
                {
                    PopulationType = g.Key ?? "Unknown",
                    TotalNumber = g.Sum(p => p.MalePopulation + p.FemalePopulation)
                }).ToListAsync();
        }

        public async Task<List<GroupedPopulationData>> GetAllGroupedPopulationsAsync()
        {
            return await _context.Populations
            .GroupBy(p => p.AgeGroup)
            .Select(g => new GroupedPopulationData
            {
                PopulationType = g.Key ?? "Unknown",
                TotalNumber = g.Sum(p => p.MalePopulation + p.FemalePopulation)
            })
            .ToListAsync();
        }
    }
}
