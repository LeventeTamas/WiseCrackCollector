using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using WiseCrackCollector.Data;
using WiseCrackCollector.Models;

namespace WiseCrackCollector.Services
{
    public class AppUserService : IAppUserService
    {
        private ApplicationDbContext dbContext;
        private IAppService appService;

        public AppUserService (ApplicationDbContext _dbContext, IAppService appService)
        {
            this.dbContext = _dbContext;
            this.appService = appService;
        }

        public AppUser GetCurrentUser()
        {
            string currentUserId = appService.GetCurrentUserId();
            return GetAppUserById(currentUserId);
        }

        public AppUser GetAppUserById(string userId)
        {
            return dbContext.AppUsers.First(u => u.Id.Equals(userId));
        }

        public List<AppUser> GetAppUsers()
        {
            return dbContext.AppUsers.ToList();
        }
    }
}
