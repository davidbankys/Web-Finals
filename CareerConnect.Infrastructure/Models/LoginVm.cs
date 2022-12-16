using System.ComponentModel.DataAnnotations;

namespace CareerConnect.Infrastructure.Models
{
    public class LoginVm
    {
        [Required]
        [Display(Name = "Type your username")]
        public string? Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
