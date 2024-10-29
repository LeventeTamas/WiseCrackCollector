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
            Group? group = dbContext.Groups.Include(g => g.Owner).Include(g => g.Wisecracks).FirstOrDefault(g => g.Id.Equals(groupId));
            return group;
        }

        public UserGroupPermission? GetUserGroupPermission(string userId, string groupId)
        {
            return dbContext.UserGroupPermissions.FirstOrDefault(p => p.UserId.Equals(userId) && p.GroupId.Equals(groupId));
        }
    }
}
