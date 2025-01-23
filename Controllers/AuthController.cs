using Microsoft.AspNetCore.Mvc;
using test.Data;
using test.DTO;
using test.Repository.Interface;

namespace test.Controllers
{
    public class AuthController : Controller
    {
        private readonly TestDb _testDb;
        private readonly IAuthRepository _authRepository;

        public AuthController(TestDb testDb, IAuthRepository authRepository)
        {
            _testDb = testDb;
            _authRepository = authRepository;
        }

        public IActionResult Login()
        {
            ViewData["HideNavBarItems"] = true;
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Login(UserDTO userDTO)
        {
            try
            {
                if (await _authRepository.UserExists(userDTO.Username, userDTO.Password))
                {
                    return RedirectToAction("Summary", "Populations");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View(userDTO);
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
