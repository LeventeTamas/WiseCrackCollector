using Microsoft.AspNetCore.Identity;

namespace WiseCrackCollector.Models
{
    public class AppUser : IdentityUser
    {
        public List<GroupUserMembership> PermissionSets { get; set; }
    }
}
