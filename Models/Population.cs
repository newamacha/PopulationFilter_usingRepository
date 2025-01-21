using System.ComponentModel.DataAnnotations;

namespace test.Models
{
    public class Population
    {
        public int PopulationId { get; set; }

        [Required]
        public int CityId { get; set; } 
        public City? City { get; set; } 

        [Required]
        public string AgeGroup { get; set; } 

        [Required]
        public int MalePopulation { get; set; }

        [Required]
        public int FemalePopulation { get; set; }
        public int CountryID { get; set; }
        public Country Country { get; set; }
    }
}