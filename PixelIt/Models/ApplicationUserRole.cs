using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PixelIt.Models
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public required string UserId { get; set; }
        public required string RoleId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser? User { get; set; }

        [ForeignKey(nameof(RoleId))]
        public ApplicationRole? Role { get; set; }


    }
}
