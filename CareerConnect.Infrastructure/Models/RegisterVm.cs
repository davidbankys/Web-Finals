using System.ComponentModel.DataAnnotations;

namespace CareerConnect.Infrastructure.Models
{
    public enum RegisterAs
    {
        Applicant,
        Employer
    }

    public class RegisterVm
    {
        [DataType(DataType.Password)]
        [Required]
        public string? Password { get; set; }
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "Confirm password")]
        public string? ConfirmPassword { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required]
        [Display(Name = "Last name")]
        public string? LastName { get; set; }
        [Required]
        [Display(Name = "First name")]
        public string? FirstName { get; set; }
        [Required]
        [Display(Name = "Company or Organization name")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public RegisterAs? RegisterAs { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        [Display(Name = "Phone number")]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Date of birth")]
        public DateTime? DateOfBirth { get; set; }
    }
}
