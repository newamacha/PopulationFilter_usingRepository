using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using test.Data;
using test.DTO;
using test.Models;

namespace test.Controllers
{
    public class PopulationsController : Controller
    {
        private readonly TestDb _context;

        public PopulationsController(TestDb context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Countries = new SelectList(_context.Countries, "CountryId", "CountryName");
            ViewBag.Cities = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.AgeGroups = new SelectList(new[] { "Old", "Young", "Child" });
            return View();
        }

        public async Task<IActionResult> Create(PopulationDTO population)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingPopulation = await _context.Populations
                        .FirstOrDefaultAsync(p => p.CountryID == population.CountryId && p.CityId == population.CityId && p.AgeGroup == population.AgeGroup);

                    if (existingPopulation != null)
                    {
                        existingPopulation.FemalePopulation = population.FemalePopulation;
                        existingPopulation.MalePopulation = population.MalePopulation;

                        _context.Update(existingPopulation);
                        TempData["SuccessMessage"] = "Population data updated successfully!";
                    }
                    else
                    {
                        var newPop = new Population()
                        {
                            CountryID = population.CountryId,
                            CityId = population.CityId,
                            AgeGroup = population.AgeGroup,
                            FemalePopulation = population.FemalePopulation,
                            MalePopulation = population.MalePopulation,
                        };

                        _context.Add(newPop);
                        TempData["SuccessMessage"] = "Population data added successfully!";
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Create));
                }
                catch
                {
                    ModelState.AddModelError("", "An error occurred while saving population data.");
                }
            }

            ViewBag.Countries = new SelectList(_context.Countries, "CountryId", "CountryName");
            ViewBag.Cities = new SelectList(_context.Cities.Where(c => c.CountryId == population.CountryId), "CityId", "CityName");
            ViewBag.AgeGroups = new SelectList(new[] { "Old", "Young", "Child" });
            return View(population);
        }


        [HttpGet]
        public JsonResult GetCities(int countryId)
        {
            var cities = _context.Cities
                .Where(c => c.CountryId == countryId)
                .Select(c => new { id = c.CityId, name = c.CityName })
                .ToList();

            return Json(cities);
        }

        public async Task<IActionResult> Summary(int? countryId, int? cityId, string populationType = null)
        {
            ViewBag.Countries = new SelectList(_context.Countries, "CountryId", "CountryName");
            ViewBag.Cities = cityId.HasValue
                ? new SelectList(_context.Cities.Where(c => c.CountryId == countryId), "CityId", "CityName")
                : new SelectList(Enumerable.Empty<SelectListItem>());

            var populations = _context.Populations
                .Include(p => p.City)
                .ThenInclude(c => c.Country)
                .AsQueryable();

            if (countryId.HasValue)
                populations = populations.Where(p => p.City.CountryId == countryId);

            if (cityId.HasValue)
                populations = populations.Where(p => p.CityId == cityId);

            if (!string.IsNullOrEmpty(populationType))
                populations = populations.Where(p => p.AgeGroup == populationType);

            var groupedData = await populations
                .GroupBy(p => p.AgeGroup)
                .Select(g => new
                {
                    PopulationType = g.Key,
                    TotalNumber = g.Sum(p => p.MalePopulation + p.FemalePopulation)
                }).ToListAsync();

            var topCountries = await _context.Countries
                .Select(c => new
                {
                    CountryName = c.CountryName,
                    Population = c.Cities.Sum(ci => ci.Populations.Sum(p => p.MalePopulation + p.FemalePopulation))
                })
                .OrderByDescending(c => c.Population)
                .Take(3)
                .ToListAsync();

            ViewBag.GroupedData = groupedData;
            ViewBag.TopCountries = topCountries;

            return View();
        }


        [HttpGet]
        public async Task<JsonResult> FilterData(int? countryId, int? cityId, string populationType = null)
        {
            var populations = _context.Populations
                .Include(p => p.City)
                .ThenInclude(c => c.Country)
                .AsQueryable();

            if (countryId.HasValue)
                populations = populations.Where(p => p.City.CountryId == countryId);

            if (cityId.HasValue)
                populations = populations.Where(p => p.CityId == cityId);

            if (!string.IsNullOrEmpty(populationType))
                populations = populations.Where(p => p.AgeGroup == populationType);

            var groupedData = await populations
                .GroupBy(p => p.AgeGroup)
                .Select(g => new GroupedPopulationData
                {
                    PopulationType = g.Key ?? "Unknown",
                    TotalNumber = g.Sum(p => p.MalePopulation + p.FemalePopulation)
                }).ToListAsync();

            if (!countryId.HasValue && !cityId.HasValue && string.IsNullOrEmpty(populationType))
            {
                groupedData = await _context.Populations
                    .GroupBy(p => p.AgeGroup)
                    .Select(g => new GroupedPopulationData
                    {
                        PopulationType = g.Key ?? "Unknown",
                        TotalNumber = g.Sum(p => p.MalePopulation + p.FemalePopulation)
                    }).ToListAsync();
            }

            var topCountries = await _context.Countries
                .Select(c => new
                {
                    CountryName = c.CountryName,
                    Population = c.Cities.Sum(ci => ci.Populations.Sum(p => p.MalePopulation + p.FemalePopulation))
                })
                .OrderByDescending(c => c.Population)
                .Take(3)
                .ToListAsync();

            return Json(new { groupedData, topCountries });
        }

    }
}
