using CareerConnect.Infrastructure.Common;
using CareerConnect.Infrastructure.DbContexts;
using CareerConnect.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace CareerConnect.Infrastructure.Service
{
    public class JobService
    {
        private readonly CoreDbContext _coreDb;
        private readonly UserInformation _userInformation;

        public JobService(CoreDbContext coreDb, UserInformation userInformation)
        {
            _coreDb = coreDb;
            _userInformation = userInformation;
        }

        public async Task<Result<List<Job>>> GetJobsAsync(string? name,  string? address, int? salary)
        {
            var jobs = await _coreDb.Set<Job>()
                .WhereIf(!string.IsNullOrEmpty(name), j => j.Name.ToLower().Contains(name))
                .WhereIf(!string.IsNullOrEmpty(address),j =>  j.Address.ToLower().Contains(address))
                .WhereIf(salary.HasValue, j => salary >= j.SalaryRangeFrom && salary <= j.SalaryRangeTo)
                .Include(j => j.CreatedBy)
                .OrderByDescending(e => e.CreatedDate)
                .ToListAsync();

            return new()
            {
                Success = true,
                Data = jobs
            };
        }
        public async Task<Result<List<Job>>> GetManageJobsAsync()
        {
            List<Job> jobs = new();

            if (_userInformation.UserType == UserType.Admin)
            {
                jobs = await _coreDb.Set<Job>()
                    .Include(j => j.ApplyingResumes)
                    .Include(j => j.CreatedBy)
                    .OrderByDescending(e => e.CreatedDate)
                    .ToListAsync();
            }

            if (_userInformation.UserType == UserType.Employer)
            {
                jobs = await _coreDb.Set<Job>()
                    .Include(j => j.ApplyingResumes)
                    .Include(j => j.CreatedBy)
                    .OrderByDescending(e => e.CreatedDate)
                    .Where(j => j.CreatedById == _userInformation.EmployerId)
                    .ToListAsync();
            }

            return new()
            {
                Success = true,
                Data = jobs
            };
        }

        public async Task<Result<Job>> CreateJobAsync(string? name, string? address, string? description, int? salaryRangeFrom, int? salaryRangeTo, DateTime? expiryDate)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(address))
            {
                return new("Job name, address and description must not be empty");
            }


            if (salaryRangeFrom == null || salaryRangeTo == null)
            {
                return new("Salary range is required");
            }

            if (salaryRangeFrom > salaryRangeTo)
            {
                return new("Salary range is invalid");
            }
            var job = new Job
            {
                Id = default,
                Name = name,
                Address = address,
                Description = description,
                SalaryRangeFrom = salaryRangeFrom,
                SalaryRangeTo = salaryRangeTo,
                CreatedDate = DateTime.Now,
                ExpiryDate = expiryDate,
                CreatedById = _userInformation.EmployerId,
            };

            _coreDb.Add(job);
            await _coreDb.SaveChangesAsync();

            return new()
            {
                Success = true,
                Data = job
            };
        }

        public async Task<Result<Job>> UpdateJobAsync(Guid? id, string? name, string? address,  string? description, int? salaryRangeFrom, int? salaryRangeTo, DateTime? expiryDate)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(address))
            {
                return new("Job name, address and description must not be empty");
            }


            if (salaryRangeFrom == null || salaryRangeTo == null)
            {
                return new("Salary range is required");
            }

            if (salaryRangeFrom > salaryRangeTo)
            {
                return new("Salary range is invalid");
            }

            var job = await _coreDb.Set<Job>().FindAsync(id);

            if (job == null)
            {
                return new("Job not found");
            }


            job.Name = name;
            job.Description = description;
            job.SalaryRangeFrom = salaryRangeFrom;
            job.SalaryRangeTo = salaryRangeTo;
            job.ExpiryDate = expiryDate;
            job.Address = address;
            _coreDb.Update(job);
            await _coreDb.SaveChangesAsync();

            return new()
            {
                Success = true,
                Data = job
            };
        }

        public async Task<Result<Job>> GetJobAsync(Guid id)
        {
            var job = await _coreDb.Set<Job>()
                .Include(j => j.CreatedBy)
                .Include(e => e.ApplyingResumes).ThenInclude(r=> r.Applicant)
                .Where(j => j.Id == id)
                .OrderByDescending(e => e.CreatedDate)
                .FirstOrDefaultAsync();

            if (job == null)
            {
                return new();
            }

            return new()
            {
                Success = true,
                Data = job
            };
        }

        public async Task<Result> ApplyAsync(Guid id)
        {
            var job = await _coreDb.Set<Job>()
                .Include(j => j.ApplyingResumes)
                .FirstOrDefaultAsync(j => j.Id == id);

            var applicant = await _coreDb.Set<Applicant>()
                .Include(a => a.Resumes)
                .FirstOrDefaultAsync(a => a.Id == _userInformation.ApplicantId);

            if (job == null || applicant == null)
            {
                return new("Can't find applicant");
            }

            var latestResume = applicant.Resumes.OrderByDescending(r => r.CreatedDate).FirstOrDefault();

            if (latestResume == null )
            {
                return new("You don't have any resume, please upload at least 1 resume");
            }

            job.ApplyingResumes.Add(latestResume);

            await _coreDb.SaveChangesAsync();

            return new Result
            {
                Success= true
            };
        }

        public async Task<Result<Job>> DeleteJobAsync(Guid id)
        {
            var job = await _coreDb.Set<Job>()
                .FirstOrDefaultAsync(j => j.Id == id);

            if (job == null)
            {
                return new("Can't find job");
            }

            if (_userInformation.UserType != UserType.Admin && _userInformation.UserType != UserType.Applicant &&
                job.CreatedById != _userInformation.EmployerId)
            {
                return new("You don't have permission to delete this job");
            }

            _coreDb.Remove(job);
            await _coreDb.SaveChangesAsync();

            return new()
            {
                Success = true,
                Data = job
            };
        }
    }
}
