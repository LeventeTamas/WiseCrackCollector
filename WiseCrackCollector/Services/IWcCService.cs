using WiseCrackCollector.Models;

namespace WiseCrackCollector.Services
{
    public interface IWcCService
    {
        UserGroupPermissionSet? GetUserGroupPermissions(string userId, string groupId);

        List<Group> GetGroupsOwnedByUser(string userId);
        string CreateGroup(string userId, string groupName);
        Group? GetGroupById(string groupId);
        void DeleteGroup(string groupId);
        void EditGroup(string groupId, string groupName);
    }
}
