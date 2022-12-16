using CareerConnect.Infrastructure.Common;
using CareerConnect.Infrastructure.DbContexts;
using CareerConnect.Infrastructure.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CareerConnect.Infrastructure.Service
{
    public class UserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly CoreDbContext _coreDbContext;
        private UserInformation _userInformation;
        public UserService(UserManager<IdentityUser> userManager, CoreDbContext coreDbContext, UserInformation userInformation)
        {
            _userManager = userManager;
            _coreDbContext = coreDbContext;
            _userInformation = userInformation;
        }

        public async Task<Result<Applicant>> CreateApplicantAsync(string? firstName, string? lastName, string? address, string? email,
            string? phoneNumber, DateTime? dateOfBirth, string? description, string? password)
        {
            var account = new IdentityUser
            {
                UserName = email,
                Email = email,
            };

            var accountResult = await _userManager.CreateAsync(account, password);
            if (!accountResult.Succeeded)
            {
                return new();
            }

            var applicant = new Applicant
            {
                AccountId = account.Id,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Address = address,
                PhoneNumber = phoneNumber,
                Description = description,
                CreatedDate = DateTime.Now,
                DateOfBirth = dateOfBirth,
            };


            _coreDbContext.Add(applicant);
            await _coreDbContext.SaveChangesAsync();

            return new()
            {
                Success = true,
                Data = applicant
            };
        }
        public Result<Applicant> UpdateApplicant()
        {
            return new();
        }

        public Result<Applicant> UpdateEmployer()
        {
            return new();
        }

        public async Task<Result<Employer>> CreateEmployerAsync(string? name, string? address, string? email, string? phoneNumber, string? description, string? password)
        {

            var account = new IdentityUser
            {
                UserName = email,
                Email = email,
            };

            var accountResult = await _userManager.CreateAsync(account, password);
            if (!accountResult.Succeeded)
            {
                return new();
            }

            var employer = new Employer
            {
                AccountId = account.Id,
                Email = email,
                PhoneNumber = phoneNumber,
                Name = name,
                Description = description,
                Address = address,
                CreatedDate = DateTime.Now,
            };

            _coreDbContext.Add(employer);
            await _coreDbContext.SaveChangesAsync();

            return new()
            {
                Success = true,
                Data = employer
            };
        }

        public async Task<Result<List<Employer>>> GetEmployersAsync()
        {
            if (_userInformation.UserType != UserType.Admin)
            {
                return new("Unauthorized");
            }

            var employers = await _coreDbContext.Set<Employer>().ToListAsync();
            return new()
            {
                Success = true,
                Data = employers
            };
        }

        public async Task<Result<List<Applicant>>> GetApplicantsAsync()
        {
            if (_userInformation.UserType != UserType.Admin)
            {
                return new("Unauthorized");
            }

            var applicants = await _coreDbContext.Set<Applicant>().ToListAsync();

            return new()
            {
                Success = true,
                Data = applicants
            };
        }
    }
}
