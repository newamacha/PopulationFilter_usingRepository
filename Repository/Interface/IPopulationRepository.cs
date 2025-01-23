using test.DTO;
using test.Models;

namespace test.Repository.Interface
{
    public interface IPopulationRepository
    {
        public Task<Population> GetExistingPopulationAsync(int countryId, int cityId, string ageGroup);
        public Task AddPopulationAsync(Population population);
        public Task UpdatePopulationAsync(Population population);
        public Task<List<PopulationSummaryDTO>> GetPopulationSummaryAsync(int? countryId, int? cityId, string populationType);
        public Task<List<TopCountryDTO>> GetTopCountriesAsync(int topCount);
        public Task<List<GroupedPopulationData>> FilterPopulationsAsync(int? countryId, int? cityId, string populationType);
        public Task<List<GroupedPopulationData>> GetAllGroupedPopulationsAsync();
    }
}
