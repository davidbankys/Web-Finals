using CareerConnect.Infrastructure.Common;
using CareerConnect.Infrastructure.DbContexts;
using CareerConnect.Infrastructure.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CareerConnect.Infrastructure.Service
{
    public class AuthenticationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly CoreDbContext _coreDbContext;
        private readonly AccountDbContext _accountDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthenticationService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, CoreDbContext coreDbContext, IHttpContextAccessor httpContextAccessor, AccountDbContext accountDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _coreDbContext = coreDbContext;
            _httpContextAccessor = httpContextAccessor;
            _accountDbContext = accountDbContext;
            InitAdministrator().Wait();
        }

        private async Task InitAdministrator()
        {
            var hasUser = _accountDbContext.Set<IdentityUser>().Any();

            if (hasUser) return;

            var adminAccount = new IdentityUser
            {
                UserName = "admin",
                Email = "admin.career.connect@gmail.com",
            };

            await _userManager.CreateAsync(adminAccount, "123456aA@");

            var admin = new Admin
            {
                AccountId = adminAccount.Id,
                Name = "Career Connect Administrator"
            };

            _coreDbContext.Add(admin);
            await _coreDbContext.SaveChangesAsync();
        }
        public async Task<Result> LoginAsync(string? username, string? password)
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                return new();
            }

            var account = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower());

            if (account == null)
            {
                return new();
            }

            var result = await _signInManager.PasswordSignInAsync(account, password, true, true);

            if (!result.Succeeded)
            {
                return new();
            }

            var applicant = _coreDbContext.Set<Applicant>().FirstOrDefault(e => e.AccountId == account.Id);

            if (applicant != null)
            {
                _httpContextAccessor.HttpContext.Session.SetString(Const.UserType, nameof(Applicant));
                _httpContextAccessor.HttpContext.Session.SetString(Const.ApplicantId, applicant.Id.ToString());

                return new Result()
                {
                    Success = true
                };
            }

            var employer = _coreDbContext.Set<Employer>().FirstOrDefault(e => e.AccountId == account.Id);

            if (employer != null)
            {
                _httpContextAccessor.HttpContext.Session.SetString(Const.UserType, nameof(Employer));
                _httpContextAccessor.HttpContext.Session.SetString(Const.EmployerId, employer.Id.ToString());
                return new Result()
                {
                    Success = true
                };
            }

            var admin = _coreDbContext.Set<Admin>().FirstOrDefault(e => e.AccountId == account.Id);

            if (admin != null)
            {
                _httpContextAccessor.HttpContext.Session.SetString(Const.UserType, nameof(Admin));
                _httpContextAccessor.HttpContext.Session.SetString(Const.AdminId, admin.Id.ToString());

                return new Result()
                {
                    Success = true
                };
            }

            return new();
        }

        public async Task LogOutAsync()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
            await _signInManager.SignOutAsync();
        }
    }
}
