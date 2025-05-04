using Microsoft.AspNetCore.Identity;

namespace PixelIt.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
