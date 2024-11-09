using Microsoft.AspNetCore.Identity;

namespace WiseCrackCollector.Models
{
    public class AppUser : IdentityUser
    {
        public List<UserGroupPermissionSet> PermissionSets { get; set; }
    }
}
