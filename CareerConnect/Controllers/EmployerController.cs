using CareerConnect.Infrastructure.Models;
using CareerConnect.Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;

namespace CareerConnect.Controllers
{
    public class EmployerController : BaseController
    {
        private readonly UserService _userService;

        public EmployerController(UserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index(EmployersVm vm)
        {
            var result = await _userService.GetEmployersAsync();

            if (!result.Success || result.Data == null)
            {
                return BadRequest();
            }
            vm.Employers = result.Data;
            return View(vm);
        }
    }
}
