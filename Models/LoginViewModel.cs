
using Microsoft.AspNetCore.Authentication;

namespace News.Models
{
    public class LoginViewModel
    {   
        public required string Email {get; set;}
        public required string Password {get; set;}
    }    
}