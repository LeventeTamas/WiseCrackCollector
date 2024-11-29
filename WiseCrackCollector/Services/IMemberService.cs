using WiseCrackCollector.Models;

namespace WiseCrackCollector.Services
{
    public interface IMemberService
    {
        bool IsMembershipExists(string groupId, string userId);
        void AddMember(GroupUserMembership membership);
        GroupUserMembership GetGroupUserMembershipById(string groupId, string userId);
        void DeleteMembership(string groupId, string userId);
        List<GroupUserMembership> GetMembershipsByGroupId(string groupId);
        void UpdateMembership(GroupUserMembership membership);
    }
}
