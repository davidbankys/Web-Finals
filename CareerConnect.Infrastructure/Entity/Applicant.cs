namespace CareerConnect.Infrastructure.Entity
{
    public class Applicant
    {
        public Guid Id { get; set; }
        public string? AccountId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public List<Resume> Resumes { get; set; } = new();
    }
}
