using WiseCrackCollector.Models;

namespace WiseCrackCollector.Services
{
    public interface IWcCService
    {
        string? GetCurrentUserId();
        bool CheckPermissionOnGroup(string userId, Group group, UserGroupPermissionType permission, out UserGroupPermissionSet userGroupPermissionSet);
        UserGroupPermissionSet? GetUserGroupPermissions(string userId, string groupId);

        List<Group> GetGroupsOwnedByUser(string userId);
        string CreateGroup(string userId, string groupName);
        Group? GetGroupById(string groupId);
        void DeleteGroup(string groupId);
        void EditGroup(string groupId, string groupName);
        void AddWisecrack(Wisecrack newWisecrack, string groupId, string userId);
        Wisecrack? GetWisecrackById(string delete_wc_id);
        void DeleteWisecrack(Wisecrack wisecrack);
        void EmptyGroup(string empty_group_id);
        void UpdateWisecrack(string wisecrackId, string newContent, string newSaidBy, string newCreatedAt);
    }
}
