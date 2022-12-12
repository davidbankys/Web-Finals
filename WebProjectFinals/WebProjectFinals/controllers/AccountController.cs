using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebProjectFinals.controllers
{
    public class AccountController : Controller
    {
        // GET: /<controller>/
        private readonly AuthenticationService _authenticationService;
        private readonly UserService _userService;

        public AccountController(AuthenticationService authenticationService, UserService userService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterVm vm)
        {
            if (vm.RegisterAs == RegisterAs.Employer)
            {
                var result = await _userService.CreateEmployerAsync(vm.Name, vm.Address, vm.Email, vm.PhoneNumber, vm.Description, vm.Password);
                if (!result.Success)
                {
                    ViewData["Message"] = "Register failed";
                    return View(vm);
                }
                return RedirectToAction("Login");
            }

            if (vm.RegisterAs == RegisterAs.Applicant)
            {
                var result = await _userService.CreateApplicantAsync(vm.FirstName, vm.LastName, vm.Address, vm.Email, vm.PhoneNumber,
                    vm.DateOfBirth, vm.Description, vm.Password);
                if (!result.Success)
                {
                    return View(vm);
                }

                return RedirectToAction("Login");
            }

            return View(vm);
        }

        [HttpGet]

        public async Task<IActionResult> Logout()
        {
            await _authenticationService.LogOutAsync();
            return RedirectToAction("Index", "Job");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _authenticationService.LoginAsync(vm.Username, vm.Password);

            if (!result.Success)
            {
                ViewData["Message"] = "Invalid login";
                return View();
            }

            return RedirectToAction("Index", "Job");
        }
    }
}
