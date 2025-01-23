using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using test.Data;
using test.DTO;
using test.Models;
using test.Repository.Interface;

namespace test.Controllers
{
    public class CitiesController : Controller
    {
        private readonly TestDb _context;
        private readonly ICitiesRepository _citiesRepository;

        public CitiesController(TestDb context, ICitiesRepository citiesRepository)
        {
            _context = context;
            _citiesRepository = citiesRepository;
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Countries = new SelectList(_context.Countries, "CountryId", "CountryName");
            return View(new CitiesDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CitiesDTO cities)
        {
            if (await _citiesRepository.CityExists(cities.CountryId, cities.CityName))
            {
                ModelState.AddModelError("CityName", "A city with the same name already exists in the selected country.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _citiesRepository.CreateCity(cities);
                    TempData["SuccessMessage"] = "City added successfully!";
                    return RedirectToAction(nameof(Create));
                }
                catch
                {
                    ModelState.AddModelError("", "An error occurred while saving the city.");
                }
            }

            ViewBag.Countries = new SelectList(_context.Countries, "CountryId", "CountryName");
            return View(cities);
        }


        [HttpGet]
        public JsonResult GetCitiesByCountry(int countryId)
        {
            var cities = _context.Cities
                                 .Where(c => c.CountryId == countryId)
                                 .Select(c => new { id = c.CityId, name = c.CityName })
                                 .ToList();

            return Json(cities);
        }
    }
}
