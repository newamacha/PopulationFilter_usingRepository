using Microsoft.AspNetCore.Mvc;
using test.Data;

namespace test.Controllers
{
    public class AuthController : Controller
    {
        private readonly TestDb _testDb;

        public AuthController(TestDb testDb)
        {
            _testDb = testDb;
        }

        public IActionResult Login()
        {
            ViewData["HideNavBarItems"] = true;
            return View();

        }
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult Login(string username, string password)
        {
            try
            {
                if (_testDb.Users.Any(u => u.Username == username && u.Password == password))
                {
                    return RedirectToAction("Summary", "Populations");
                }
                else
                {
                    return RedirectToAction("Login", "Auth");
                }
            }
            catch (Exception ex) 
            {
                return RedirectToAction("Login", "Auth");
            }
            
        }

        [HttpPost]
        public IActionResult Logout()
        {
            return RedirectToAction("Login", "Auth");

        }
    }
}
