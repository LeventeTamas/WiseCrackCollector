using WiseCrackCollector.Models;

namespace WiseCrackCollector.Services
{
    public interface IMemberService
    {
        List<GroupUserMembership> GetMembershipsByGroupId(string groupId);
    }
}
