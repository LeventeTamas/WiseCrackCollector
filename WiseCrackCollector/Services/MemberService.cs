using Microsoft.EntityFrameworkCore;
using WiseCrackCollector.Data;
using WiseCrackCollector.Models;

namespace WiseCrackCollector.Services
{
    public class MemberService : IMemberService
    {
        ApplicationDbContext dbContext;
        public MemberService(ApplicationDbContext _dbContext) 
        { 
            dbContext = _dbContext;
        }
        public bool IsMembershipExists(string groupId, string userId)
        {
            return dbContext.GroupUserMemberships.Any(m => m.UserId.Equals(userId) && m.GroupId.Equals(groupId));
        }

        public void AddMember(GroupUserMembership membership)
        {
            dbContext.GroupUserMemberships.Add(membership);
            dbContext.SaveChanges();
        }

        public GroupUserMembership GetGroupUserMembershipById(string groupId, string userId)
        {
            return dbContext.GroupUserMemberships.First(m => m.UserId.Equals(userId) && m.GroupId.Equals(groupId));
        }

        public void DeleteMembership(string groupId, string userId)
        {
            dbContext.GroupUserMemberships.Remove(GetGroupUserMembershipById(groupId, userId));
            dbContext.SaveChanges();
        }

        public List<GroupUserMembership> GetMembershipsByGroupId(string groupId)
        {
            return dbContext.GroupUserMemberships.Include(m => m.User).Where(m => m.GroupId.Equals(groupId)).ToList();
        }

        public void UpdateMembership(GroupUserMembership newMembership)
        {
            GroupUserMembership? membership = GetGroupUserMembershipById(newMembership.GroupId, newMembership.UserId);
            if (membership == null)
                return;

            membership.Add = newMembership.Add;
            membership.Update = newMembership.Update;
            membership.Read = newMembership.Read;
            membership.Delete = newMembership.Delete;
            membership.ManageMembers = newMembership.ManageMembers;

            dbContext.SaveChanges();
        }
    }
}
