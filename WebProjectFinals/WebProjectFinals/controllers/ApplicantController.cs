using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebProjectFinals.controllers
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