using WiseCrackCollector.Models;

namespace WiseCrackCollector.Services
{
    public interface IGroupService
    {
        bool CheckPermissionOnGroup(string groupId, UserGroupPermissionType permission, out UserGroupPermissionSet userGroupPermissionSet);
        UserGroupPermissionSet? GetUserGroupPermissions(string userId, string groupId);
        List<Group> GetGroupsOwnedByCurrentUser();
        string CreateGroup(string groupName);
        Group GetGroupById(string groupId);
        void DeleteGroup(string groupId);
        void EditGroup(string groupId, string groupName);
        bool IsGroupExists(string groupId);
    }
}
