namespace CareerConnect.Infrastructure.Entity
{
    public class Employer
    {
        public Guid Id { get; set; }
        public string? AccountId { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public DateTime? CreatedDate { get; set; }
        public List<Job> ListedJob { get; set; } = new();
    }
}
