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

        public int CountryId { get; set; }
        [Required]
        public int FemalePopulation { get; set; }
    }
}
