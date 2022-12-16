using System.ComponentModel.DataAnnotations;
using CareerConnect.Infrastructure.Entity;

namespace CareerConnect.Infrastructure.Models
{
    public class JobsVm
    {
        [Display(Name = "Search by job name")]
        public string? Name { get; set; }
        [Display(Name = "Search by job address")]
        public string? Address { get; set; }
        [Display(Name = "Salary")]
        public int? Salary { get; set; }
        public List<Job> Jobs { get; set; } = new();
    }
}
