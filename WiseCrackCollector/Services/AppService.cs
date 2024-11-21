using System.Security.Claims;

namespace WiseCrackCollector.Services
{
    public class AppService : IAppService
    {
        private IHttpContextAccessor httpContextAccessor;

        public AppService(IHttpContextAccessor _httpContextAccessor)
        {
            httpContextAccessor = _httpContextAccessor;
        }

        public string GetCurrentUserId()
        {
            HttpContext httpContext = httpContextAccessor.HttpContext;
            ClaimsIdentity identity = (ClaimsIdentity?)httpContext.User.Identity;
            Claim claim = identity.FindFirst(ClaimTypes.NameIdentifier);
            return claim.Value;
        }
    }
}
