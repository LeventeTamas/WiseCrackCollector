using Microsoft.EntityFrameworkCore;
using WiseCrackCollector.Data;
using WiseCrackCollector.Models;

namespace WiseCrackCollector.Services
{
    public class GroupService : IGroupService
    {
        private ApplicationDbContext dbContext;
        private IAppUserService appUserService;

        public GroupService(ApplicationDbContext _dbContext, IAppUserService appUserService)
        {
            dbContext = _dbContext;
            this.appUserService = appUserService;
        }

        public bool IsGroupExists(string groupId)
        {
            return dbContext.Groups.Any(g => g.Id.Equals(groupId));
        }

        public bool CheckPermissionOnGroup(string groupId, PermissionType permission, out GroupUserMembership membership)
        {
            AppUser user = appUserService.GetCurrentUser();
            Group group = GetGroupById(groupId);
            if (group.Owner.Id.Equals(user.Id))
            {
                membership = new GroupUserMembership(user.Id, group.Id) {
                    User = user, 
                    Group = group, 
                    Read = true, 
                    Update = true, 
                    Delete = true, 
                    Add = true, 
                    ManageMembers = true 
                };
                return true;
            }

            membership = GetMembership(user.Id, group.Id);
            return membership != null && membership.CheckPermission(permission);
        }

        public string CreateGroup(string groupName)
        {
            AppUser user = appUserService.GetCurrentUser();

            Group newGroup = new Group()
            {
                Name = groupName,
                Owner = user
            };
            dbContext.Groups.Add(newGroup);
            dbContext.SaveChanges();

            return newGroup.Id;
        }

        public void DeleteGroup(string groupId)
        {
            dbContext.Groups.Remove(GetGroupById(groupId));
            dbContext.SaveChanges();
        }

        public void EditGroup(string groupId, string groupName)
        {
            dbContext.Groups
                .Where(g => g.Id.Equals(groupId))
                .ExecuteUpdate(g => g.SetProperty(n => n.Name, groupName));
        }

        public Group GetGroupById(string groupId)
        {
            return dbContext.Groups
                .Include(g => g.Owner)
                .Include(g => g.Wisecracks)
                .First(g => g.Id.Equals(groupId));
        }

        public List<Group> GetGroupsOwnedByCurrentUser()
        {
            string userId = appUserService.GetCurrentUser().Id;
            return dbContext.Groups
                .Include(g => g.Wisecracks)
                .Include(g => g.Owner)
                .Where(g => g.Owner.Id.Equals(userId))
                .ToList();
        }

        public GroupUserMembership? GetMembership(string userId, string groupId)
        {
            return dbContext.GroupUserMemberships.FirstOrDefault(p => p.User.Id.Equals(userId) && p.Group.Id.Equals(groupId));
        }

        public List<Group> GetGroupsConnectedToCurrentUser()
        {
            string userId = appUserService.GetCurrentUser().Id;
            return dbContext.GroupUserMemberships
                .Include(m => m.Group)
                .Include(m => m.Group.Wisecracks)
                .Include (m => m.Group.Owner)
                .Where(m => m.UserId.Equals(userId))
                .Select(m => m.Group)
                .ToList();
        }
    }
}
