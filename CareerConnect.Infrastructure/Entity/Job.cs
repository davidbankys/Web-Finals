namespace CareerConnect.Infrastructure.Entity
{

    public class Job
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public int? SalaryRangeFrom { get; set; }
        public int? SalaryRangeTo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public Employer? CreatedBy { get; set; }
        public Guid? CreatedById { get; set; }
        public List<Resume> ApplyingResumes { get; set; } = new();
    }
}
