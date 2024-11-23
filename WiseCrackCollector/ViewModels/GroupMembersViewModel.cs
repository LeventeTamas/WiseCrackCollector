using WiseCrackCollector.Models;

namespace WiseCrackCollector.ViewModels
{
    public class GroupMembersViewModel
    {
        public Group Group { get; set; }
        public List<AppUser> NotMemberUsers { get; set; }
        public List<GroupUserMembership> Memberships { get; set; }
    }
}
