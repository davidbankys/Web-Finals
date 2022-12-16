using System.Security.Claims;
using CareerConnect.Infrastructure.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CareerConnect.Infrastructure.Service
{
    public enum UserType
    {
        Undefined = 0,
        Admin,
        Applicant,
        Employer
    }
    public class UserInformation
    {
        public string? AccountId { get; set; }
        public string? AccountUsername { get; set; }
        public Guid? ApplicantId { get; set; }
        public Guid? EmployerId { get; set; }
        public Guid? AdminId { get; set; }
        public UserType UserType { get; set; }
        public bool IsAuthenticated { get; set; }

        private SignInManager<IdentityUser> _signInManager;

        public UserInformation(IHttpContextAccessor httpContextAccessor, SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
            if (httpContextAccessor.HttpContext == null)
            {
                return;
            }

            AccountId = httpContextAccessor.HttpContext.User.FindFirst(claim =>
                claim.Type == ClaimTypes.NameIdentifier)?.Value;
            AccountUsername = httpContextAccessor.HttpContext.User.Identity?.Name;


            Guid.TryParse(httpContextAccessor.HttpContext.Session.GetString(Const.ApplicantId), out var applicantId);
            ApplicantId = applicantId;
            Guid.TryParse(httpContextAccessor.HttpContext.Session.GetString(Const.EmployerId), out var employerId);
            EmployerId = employerId;
            Guid.TryParse(httpContextAccessor.HttpContext.Session.GetString(Const.AdminId), out var adminId);
            AdminId = adminId;

            Enum.TryParse(httpContextAccessor.HttpContext.Session.GetString(Const.UserType), out UserType userType);

            UserType = userType;

            if (UserType == Service.UserType.Undefined && !string.IsNullOrEmpty(AccountId))
            {
                _signInManager.SignOutAsync().Wait();
                return;
            }

            IsAuthenticated = true == httpContextAccessor.HttpContext.User.Identity?.IsAuthenticated;
        }
    }
}
