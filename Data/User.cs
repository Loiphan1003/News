namespace News.Data
{
    public class User
    {
        public Guid Id { get; set; }
        public required string Email {get; set;}
        public required string DisplayName {get; set;}
        public string? Password {get; set;}
        public string? Avatar {get; set;}

        public int RoleId {get; set;}
        public Role Role {get; set;} = null!;
    }
}