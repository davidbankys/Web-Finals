using System.ComponentModel.DataAnnotations;
using CareerConnect.Infrastructure.Entity;

namespace CareerConnect.Infrastructure.Models
{
    public class UpdateJobVm
    {
        public Guid Id { get; set; }
        [Display(Name = "Job name, title")]
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        [Display(Name = "Job description")]
        public string? Description { get; set; }
        [Required]
        [Display(Name = "Salary from")]
        public int? SalaryRangeFrom { get; set; }
        [Required]
        [Display(Name = "To")]
        public int? SalaryRangeTo { get; set; }

        [Display(Name = "Applying expiry date")]
        public DateTime? ExpiryDate { get; set; }
        public List<Resume> ApplyingResumes { get; set; } = new();
    }
}
