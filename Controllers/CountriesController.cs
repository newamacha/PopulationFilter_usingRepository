using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using test.Data;
using test.Models;

namespace test.Controllers
{
    public class CountriesController : Controller
    {
        private readonly TestDb _context;

        public CountriesController(TestDb context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Country country)
       {
            if (_context.Countries.Any(c => c.CountryName == country.CountryName))
            {
                ModelState.AddModelError("CountryName", "Country name already exists.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(country);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Country added successfully!";
                    return RedirectToAction(nameof(Create)); 
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving the country. Please try again.");
                }
            }
            return View(country); 
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
