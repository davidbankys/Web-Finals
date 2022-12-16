using CareerConnect.Infrastructure.Common;
using CareerConnect.Infrastructure.Entity;
using CareerConnect.Infrastructure.Models;
using CareerConnect.Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerConnect.Controllers
{
    public class JobController : BaseController
    {
        private readonly JobService _jobService;
        private readonly UserInformation _userInformation;
        public JobController(JobService jobService, UserInformation userInformation)
        {
            _jobService = jobService;
            _userInformation = userInformation;
        }

        public async Task<IActionResult> Apply(Guid id)
        {
            var result = await _jobService.ApplyAsync(id);
            if (!result.Success)
            {
                AddMessage(result.Message);
                return RedirectToAction("View",new {Id = id});
            }
            return View();
        }

        public async Task<IActionResult> Manage(JobsVm vm)
        {
            var result = await _jobService.GetManageJobsAsync();

            if (!result.Success || result.Data == null)
            {
                return BadRequest();
            }

            vm.Jobs = result.Data;

            return View("Manage", vm);
        }

        public async Task<IActionResult> Applying(JobsVm vm)
        {
            var result = await _jobService.GetManageJobsAsync();

            if (!result.Success || result.Data == null)
            {
                return BadRequest();
            }

            vm.Jobs = result.Data;

            return View("Manage", vm);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> View(Guid id)
        {
            var result = await _jobService.GetJobAsync(id);

            if (!result.Success || result.Data == null)
            {
                return BadRequest();
            }

            var job = result.Data;

            return View(job);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var vm = new UpdateJobVm();
            return View("Update", vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var vm = new UpdateJobVm();
            var result = await _jobService.GetJobAsync(id);
            if (!result.Success || result.Data == null)
            {
                return BadRequest();
            }

            var job = result.Data;
            vm.ExpiryDate = job.ExpiryDate;
            vm.Description = job.Description;
            vm.Name = job.Name;
            vm.Id = job.Id;
            vm.SalaryRangeFrom = job.SalaryRangeFrom;
            vm.SalaryRangeTo = job.SalaryRangeTo;

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateJobVm vm)
        {
            Result<Job> result;

            if (vm.Id == Guid.Empty)
            {
                result = await _jobService.CreateJobAsync(vm.Name, vm.Address, vm.Description, vm.SalaryRangeFrom, vm.SalaryRangeTo, vm.ExpiryDate);
            }
            else
            {
                result = await _jobService.UpdateJobAsync(vm.Id, vm.Address, vm.Name, vm.Description, vm.SalaryRangeFrom, vm.SalaryRangeTo, vm.ExpiryDate);
            }

            if (!result.Success)
            {
                AddMessage(result.Message);
                return View(vm);
            }

            return RedirectToAction("Manage");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(JobsVm vm)
        {
            var result = await _jobService.GetJobsAsync(vm.Name, vm.Address, vm.Salary);

            if (!result.Success || result.Data == null)
            {
                return BadRequest();
            }

            vm.Jobs = result.Data;

            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _jobService.GetJobAsync(id);
            if (!result.Success || result.Data == null)
            {
                AddMessage(result.Message);
                return BadRequest();
            }

            var job = result.Data;

            return View(job);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var result = await _jobService.DeleteJobAsync(id);

            if (!result.Success || result.Data == null)
            {
                AddMessage(result.Message);
                return BadRequest();
            }
            return RedirectToAction("Manage");
        }
    }
}