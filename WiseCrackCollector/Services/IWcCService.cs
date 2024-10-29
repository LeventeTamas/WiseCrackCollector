using WiseCrackCollector.Models;

namespace WiseCrackCollector.Services
{
    public interface IWcCService
    {
        UserGroupPermission? GetUserGroupPermission(string userId, string groupId);

        List<Group> GetGroupsOwnedByUser(string userId);
        string CreateGroup(string userId, string groupName);
        Group? GetGroupById(string groupId);
    }
}
