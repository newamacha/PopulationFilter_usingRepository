using System.ComponentModel.DataAnnotations;

namespace test.Models
{
    public class Country
    {
        public int CountryId { get; set; }
        [Required(ErrorMessage = "Country Name is required.")]
        [StringLength(100, ErrorMessage = "Country Name can't be longer than 100 characters.")]
        public string CountryName { get; set; }
        public ICollection<City>? Cities { get; set; }
    }
}
