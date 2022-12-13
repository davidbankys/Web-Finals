using CareerConnect.Infrastructure.Common;
using CareerConnect.Infrastructure.Entity;
using CareerConnect.Infrastructure.Models;
using CareerConnect.Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;
using WebProjectFinals.controllers;

namespace CareerConnect.Controllers
{
    public class ResumeController : BaseController
    {
        private readonly ResumeService _resumeService;
        private readonly JobService _jobService;
        private readonly UserInformation _userInformation;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ResumeController(ResumeService resumeService, JobService jobService, UserInformation userInformation, IWebHostEnvironment webHostEnvironment)
        {
            _jobService = jobService;
            _userInformation = userInformation;
            _webHostEnvironment = webHostEnvironment;
            _resumeService = resumeService;
        }

        public async Task<IActionResult> Download(Guid id)
        {
            var fullPath = $"{_webHostEnvironment.ContentRootPath}/Files/{id}.pdf";

            if (System.IO.File.Exists(fullPath))
            {
                var data = await System.IO.File.ReadAllBytesAsync(fullPath);
                return File(data, "application/pdf");
            }

            return BadRequest();
        }
        [HttpGet]
        public async Task<IActionResult> Index(ResumesVm vm)
        {
            var result = await _resumeService.GetResumesAsync();

            if (!result.Success || result.Data == null)
            {
                AddMessage(result.Message);
                return BadRequest();
            }

            vm.Resumes = result.Data;

            return View(vm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            UpdateResumeVm vm = new();
            return View("Update", vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            UpdateResumeVm vm = new();

            var result = await _resumeService.GetResumeAsync(id);

            if (!result.Success)
            {
                AddMessage(result.Message);
                return BadRequest();
            }

            var resume = result.Data!;
            vm.Id = resume.Id;
            vm.Name = resume.Name;

            return View("Update", vm);
        }
        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var result = await _resumeService.GetResumeAsync(id);
            if (!result.Success)
            {
                return BadRequest();
            }

            var resume = result.Data!;

            return View(resume);
        }


        [HttpPost]
        public async Task<IActionResult> Update(UpdateResumeVm vm)
        {
            Result<Resume> result;
            if (vm.Id == Guid.Empty)
            {
                result = await _resumeService.CreateResumeAsync(vm.File, vm.Name);
            }
            else
            {
                result = await _resumeService.UpdateResumeAsync(vm.Id, vm.Name);
            }

            if (!result.Success)
            {
                AddMessage(result.Message);
                return View(vm);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _resumeService.GetResumeAsync(id);

            return View(result.Data);
        }

        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var result = await _resumeService.DeleteResumeAsync(id);
            return RedirectToAction("Index");
        }
    }
}
