using WiseCrackCollector.Models;

namespace WiseCrackCollector.Services
{
    public interface IAppUserService
    {
        AppUser GetCurrentUser();
        AppUser GetAppUserById(string appId);
    }
}
