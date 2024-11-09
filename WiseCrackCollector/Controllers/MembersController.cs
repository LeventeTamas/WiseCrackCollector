using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WiseCrackCollector.Services;

namespace WiseCrackCollector.Controllers
{
    public class MembersController : Controller
    {

        private IWcCService wccService;

        public MembersController(IWcCService _wccService)
        {
            wccService = _wccService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [Route("/Members/Manage")]
        public IActionResult Manage(string groupId)
        {
            return View();
        }
    }
}
