using WiseCrackCollector.Models;

namespace WiseCrackCollector.ViewModels
{
    public class GroupMembersViewModel
    {
        public Group Group { get; set; }
        public List<GroupUserMembership> Memberships { get; set; }
    }
}
