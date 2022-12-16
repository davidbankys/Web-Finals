using CareerConnect.Infrastructure.Entity;

namespace CareerConnect.Infrastructure.Models
{
    public class EmployersVm
    {
        public List<Employer> Employers { get; set; } = new();
    }
}
