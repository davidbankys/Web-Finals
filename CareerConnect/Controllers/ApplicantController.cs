using CareerConnect.Infrastructure.Models;
using CareerConnect.Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;

namespace CareerConnect.Controllers
{
    public class ApplicantController : BaseController
    {
        private readonly UserService _userService;

        public ApplicantController(UserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index(ApplicantsVm vm)
        {
            var result = await _userService.GetApplicantsAsync();

            if (!result.Success || result.Data == null)
            {
                return BadRequest();
            }

            vm.Applicants = result.Data;

            return View(vm);
        }
    }
}
