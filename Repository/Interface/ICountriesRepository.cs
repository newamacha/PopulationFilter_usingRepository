using test.DTO;

namespace test.Repository.Interface
{
    public interface ICountriesRepository
    {
        public Task<bool> CountryExists(string countryName);
        public Task CreateCountry(CountriesDTO countriesDTO);
    }
}


