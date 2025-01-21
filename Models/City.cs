using System.ComponentModel.DataAnnotations;

namespace test.Models
{
    public class City
    {
        public int CityId { get; set; }
        [Required]
        public int CountryId { get; set; }
        [Required(ErrorMessage = "CityName is required.")]
        [StringLength(100, ErrorMessage = "CityName can't exceed 100 characters.")]
        public string CityName { get; set; }
        public Country? Country { get; set; }
        public ICollection<Population>? Populations { get; set; }
    }
}
