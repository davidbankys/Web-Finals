using System.ComponentModel.DataAnnotations;
using CareerConnect.Infrastructure.Entity;
using Microsoft.AspNetCore.Http;

namespace CareerConnect.Infrastructure.Models
{
    public class UpdateResumeVm
    {
        public Guid Id { get; set; }
        [Display(Name = "Resume file (.PDF format required)")]
        [Required]
        public IFormFile? File { get; set; }
        [Display(Name = "Resume name")]
        [Required]
        public string? Name { get; set; }
    }
}
