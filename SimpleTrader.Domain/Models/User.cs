namespace SimpleTrader.Domain.Models
{
    public class User : DomainObject
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}
