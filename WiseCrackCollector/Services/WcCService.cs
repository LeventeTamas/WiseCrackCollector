using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WiseCrackCollector.Data;
using WiseCrackCollector.Models;

namespace WiseCrackCollector.Services
{
    public class WcCService : IWcCService
    {
        private ApplicationDbContext dbContext;

        public WcCService(ApplicationDbContext _dbContext) 
        { 
            dbContext = _dbContext;
        }

        public List<Group> GetGroupsOwnedByUser(string userId)
        {
            return dbContext.Groups
                .Include(g => g.Wisecracks)
                .Include(g => g.Owner)
                .Where(g => g.Owner.Id.Equals(userId))
                .ToList();
        }

        public string CreateGroup(string userId, string groupName)
        {
            IdentityUser user = dbContext.Users.First(u => u.Id.Equals(userId));

            Group newGroup = new Group()
            {
                Name = groupName,
                Owner = user
            };
            dbContext.Groups.Add(newGroup);
            dbContext.SaveChanges();

            return newGroup.Id;
        }

        public Group? GetGroupById(string groupId)
        {
            return dbContext.Groups.Include(g => g.Owner).Include(g => g.Wisecracks).FirstOrDefault(g => g.Id.Equals(groupId));
        }

        public UserGroupPermissionSet? GetUserGroupPermissions(string userId, string groupId)
        {
            return dbContext.UserGroupPermissions.FirstOrDefault(p => p.UserId.Equals(userId) && p.GroupId.Equals(groupId));
        }

        public void DeleteGroup(string groupId)
        {
            EmptyGroup(groupId);

            Group group = dbContext.Groups.First(g => g.Id.Equals(groupId));
            dbContext.Groups.Remove(group);
            dbContext.SaveChanges();
        }

        public void EditGroup(string groupId, string groupName)
        {
            dbContext.Groups.Where(g => g.Id.Equals(groupId)).ExecuteUpdate(g => g.SetProperty(n => n.Name, groupName));
        }

        public void AddWisecrack(Wisecrack newWisecrack, string groupId, string userId)
        {
            IdentityUser user = dbContext.Users.First(u => u.Id.Equals(userId));
            Group group = dbContext.Groups.First(g => g.Id.Equals(groupId));

            newWisecrack.Owner = user;
            newWisecrack.Group = group;

            dbContext.Wisecracks.Add(newWisecrack);
            dbContext.SaveChanges();
        }

        public Wisecrack? GetWisecrackById(string delete_wc_id)
        {
            return dbContext.Wisecracks.Include(w => w.Group).Include(w => w.Group.Owner).Include(w => w.Owner).FirstOrDefault(w => w.Id.Equals(delete_wc_id));
        }

        public void DeleteWisecrack(Wisecrack wisecrack)
        {
            dbContext.Remove(wisecrack);
            dbContext.SaveChanges();
        }

        public void EmptyGroup(string empty_group_id)
        {
            IQueryable<Wisecrack> wisecrackList = dbContext.Wisecracks.Include(w => w.Group).Where(w => w.Group.Id.Equals(empty_group_id));
            foreach (var item in wisecrackList)
                dbContext.Wisecracks.Remove(item);
            dbContext.SaveChanges();
        }

        public void UpdateWisecrack(string wisecrackId, string newContent, string newSaidBy, string newCreatedAt)
        {
            Wisecrack wisecrack = GetWisecrackById(wisecrackId);
            wisecrack.Content = newContent;
            wisecrack.CreatedAt = DateTime.Parse(newCreatedAt);
            wisecrack.SaidBy = newSaidBy;

            dbContext.SaveChanges();
        }
    }
}
