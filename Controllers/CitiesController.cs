using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using test.Data;
using test.Models;

namespace test.Controllers
{
    public class CitiesController : Controller
    {
        private readonly TestDb _context;

        public CitiesController(TestDb context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Countries = new SelectList(_context.Countries, "CountryId", "CountryName");
            return View(new City());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(City city)
        {
            if (_context.Cities.Any(c => c.CountryId == city.CountryId && c.CityName == city.CityName))
            {
                ModelState.AddModelError("CityName", "A city with the same name already exists in the selected country.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(city);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "City added successfully!";
                    return RedirectToAction(nameof(Create));
                }
                catch
                {
                    ModelState.AddModelError("", "An error occurred while saving the city.");
                }
            }

            ViewBag.Countries = new SelectList(_context.Countries, "CountryId", "CountryName");
            return View(city);
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
