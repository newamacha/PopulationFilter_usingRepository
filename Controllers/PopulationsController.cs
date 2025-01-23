using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using test.Data;
using test.DTO;
using test.Models;
using test.Repository.Interface;

namespace test.Controllers
{
    public class PopulationsController : Controller
    {
        private readonly TestDb _context;
        private readonly IPopulationRepository _populationRepository;

        public PopulationsController(TestDb context, IPopulationRepository populationRepository)
        {
            _context = context;
            _populationRepository = populationRepository;
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
                    var existingPopulation = await _populationRepository.GetExistingPopulationAsync(population.CountryId, population.CityId, population.AgeGroup);

                    if (existingPopulation != null)
                    {
                        existingPopulation.FemalePopulation = population.FemalePopulation;
                        existingPopulation.MalePopulation = population.MalePopulation;

                        await _populationRepository.UpdatePopulationAsync(existingPopulation);
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

                        await _populationRepository.AddPopulationAsync(newPop);
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

            var groupedData = await _populationRepository.GetPopulationSummaryAsync(countryId, cityId, populationType);

            var topCountries = await _populationRepository.GetTopCountriesAsync(3);

            ViewBag.GroupedData = groupedData;
            ViewBag.TopCountries = topCountries;

            return View();
        }


        [HttpGet]
        public async Task<JsonResult> FilterData(int? countryId, int? cityId, string populationType = null)
        {
            var groupedData = await _populationRepository.FilterPopulationsAsync(countryId, cityId, populationType);

            if (!countryId.HasValue && !cityId.HasValue && string.IsNullOrEmpty(populationType))
            {
                groupedData = await _populationRepository.GetAllGroupedPopulationsAsync();
            }

            var topCountries = await _populationRepository.GetTopCountriesAsync(3);

            return Json(new { groupedData, topCountries });
        }

    }
}