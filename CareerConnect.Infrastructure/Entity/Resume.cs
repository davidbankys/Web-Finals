namespace CareerConnect.Infrastructure.Entity
{
    public class Resume
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? FilePath { get; set; }
        public Guid? ApplicantId { get; set; }
        public Applicant? Applicant { get; set; }
        public List<Resume> AppliedJobs { get; set; } = new();
    }
}
