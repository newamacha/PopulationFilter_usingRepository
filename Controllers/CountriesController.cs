using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using test.Data;
using test.DTO;
using test.Models;
using test.Repository.Interface;

namespace test.Controllers
{
    public class CountriesController : Controller
    {
        private readonly TestDb _context;
        private readonly ICountriesRepository _countriesRepository;

        public CountriesController(TestDb context, ICountriesRepository countriesRepository)
        {
            _context = context;
            _countriesRepository = countriesRepository;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CountriesDTO countries)
        {
            if (await _countriesRepository.CountryExists(countries.CountryName))
            {
                ModelState.AddModelError("CountryName", "Country name already exists.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _countriesRepository.CreateCountry(countries);
                    TempData["SuccessMessage"] = "Country added successfully!";
                    return RedirectToAction(nameof(Create));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving the country. Please try again.");
                }
            }  
            return View(countries); 
        }

        [HttpGet]
        public IActionResult GetCountries()
        {
            var countries = _context.Countries
                .Select(c => new { c.CountryId, Name = c.CountryName })
                .ToList();

            return Json(countries);
        }
    }
}
