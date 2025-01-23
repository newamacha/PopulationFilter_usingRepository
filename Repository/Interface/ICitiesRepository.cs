using test.DTO;

namespace test.Repository.Interface
{
    public interface ICitiesRepository
    {
        public Task<bool> CityExists(int countryId, string cityName);
        public Task CreateCity(CitiesDTO citiesDTO);
    }
}
