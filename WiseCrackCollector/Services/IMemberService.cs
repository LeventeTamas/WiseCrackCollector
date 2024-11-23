using WiseCrackCollector.Models;

namespace WiseCrackCollector.Services
{
    public interface IMemberService
    {
        void AddMember(GroupUserMembership membership);
        List<GroupUserMembership> GetMembershipsByGroupId(string groupId);
    }
}
