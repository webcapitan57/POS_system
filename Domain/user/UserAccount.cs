using Microsoft.AspNetCore.Identity;

namespace POC.BL.Domain.user
{
    public class UserAccount : IdentityUser
    {
        public User User { get; set; } 
    }
}