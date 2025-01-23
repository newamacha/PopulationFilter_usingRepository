using System.ComponentModel.DataAnnotations;

namespace test.DTO
{
    public class PopulationDTO
    {
        public int PopulationId { get; set; }

        [Required]
        public int CityId { get; set; }

        [Required]
        public string AgeGroup { get; set; }

        [Required]
        public int MalePopulation { get; set; }
        [Required]
        public int CountryId { get; set; }
        [Required]
        public int FemalePopulation { get; set; }
    }

    public class PopulationSummaryDTO
    {
        public string PopulationType { get; set; }
        public int TotalNumber { get; set; }
    }

    public class TopCountryDTO
    {
        public string CountryName { get; set; }
        public int Population { get; set; }
    }

}
