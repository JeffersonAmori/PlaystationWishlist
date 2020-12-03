using Microsoft.AspNetCore.Identity;

namespace PlaystationWishlist.DataAccess.Models.Identity
{
    public class AppUser : IdentityUser<int>
    {
        public string Name { get; set; }
    }
}
