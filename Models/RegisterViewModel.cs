namespace News.Models
{
    public class RegisterViewModel
    {
        public required string Email { get; set; }
        public required string DisplayName { get; set; }
        public required string Password { get; set; }
        public required string RePassword { get; set; }
    }
}