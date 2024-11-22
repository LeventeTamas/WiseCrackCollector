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

        public List<GroupUserMembership> GetMembershipsByGroupId(string groupId)
        {
            return dbContext.GroupUserMemberships.Include(m => m.User).Where(m => m.GroupId.Equals(groupId)).ToList();
        }
    }
}
