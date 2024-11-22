using WiseCrackCollector.Models;

namespace WiseCrackCollector.Services
{
    public interface IGroupService
    {
        bool CheckPermissionOnGroup(string groupId, PermissionType permission, out GroupUserMembership membership);
        GroupUserMembership? GetMembership(string userId, string groupId);
        List<Group> GetGroupsOwnedByCurrentUser();
        string CreateGroup(string groupName);
        Group GetGroupById(string groupId);
        void DeleteGroup(string groupId);
        void EditGroup(string groupId, string groupName);
        bool IsGroupExists(string groupId);
    }
}
