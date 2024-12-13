using Microsoft.EntityFrameworkCore;
using WiseCrackCollector.Data;
using WiseCrackCollector.Models;

namespace WiseCrackCollector.Services
{
    public class WisecrackService : IWisecrackService
    {
        private ApplicationDbContext dbContext;
        private IAppUserService appUserService;
        private IGroupService groupService;

        public WisecrackService(ApplicationDbContext _dbContext, IAppUserService _appUserService, IGroupService _groupService)
        {
            this.dbContext = _dbContext;
            this.appUserService = _appUserService;
            this.groupService = _groupService;
        }

        public void DeleteWisecracksByGroupId(string groupId)
        {
            IQueryable<Wisecrack> wisecrackList = dbContext.Wisecracks.Include(w => w.Group).Where(w => w.Group.Id.Equals(groupId));
            foreach (var item in wisecrackList)
                dbContext.Wisecracks.Remove(item);
            dbContext.SaveChanges();
        }

        public void AddWisecrack(string groupId, Wisecrack wisecrack)
        {
            AppUser user = appUserService.GetCurrentUser();
            Group group = groupService.GetGroupById(groupId);

            wisecrack.Owner = user;
            wisecrack.Group = group;

            dbContext.Wisecracks.Add(wisecrack);
            dbContext.SaveChanges();
        }

        public Wisecrack GetWisecrackById(string wisecrackId)
        {
            return dbContext.Wisecracks.Include(w => w.Group).Include(w => w.Group.Owner).Include(w => w.Owner).First(w => w.Id.Equals(wisecrackId));
        }

        public List<Wisecrack> GetWisecracksByGroupId(string groupId)
        {
            return dbContext.Wisecracks.Include(w => w.Group).Include(w => w.Owner).Where(w => w.Group.Id.Equals(groupId)).ToList();
        }

        public void DeleteWisecrack(Wisecrack wisecrack)
        {
            dbContext.Remove(wisecrack);
            dbContext.SaveChanges();
        }

        public void UpdateWisecrack(Wisecrack newWisecrack)
        {
            Wisecrack? wisecrack = GetWisecrackById(newWisecrack.Id);
            if (wisecrack == null)
                return;
            wisecrack.Content = newWisecrack.Content;
            wisecrack.CreatedAt = newWisecrack.CreatedAt;
            wisecrack.SaidBy = newWisecrack.SaidBy;

            dbContext.SaveChanges();
        }

        public bool IsWisecrackExists(string wisecrackId)
        {
            return dbContext.Wisecracks.Any(w => w.Id.Equals(wisecrackId));
        }
    }
}
