using Microsoft.AspNetCore.Identity;

namespace AHT.Models
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public string RoleAssigner { get; set; }
    }
}
