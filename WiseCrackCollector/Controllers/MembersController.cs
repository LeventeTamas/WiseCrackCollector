using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WiseCrackCollector.Models;
using WiseCrackCollector.Services;

namespace WiseCrackCollector.Controllers
{
    public class MembersController : Controller
    {

        private IMemberService memberService;

        public MembersController(IMemberService _memberService)
        {
            memberService = _memberService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [Route("/Members/GroupMembers")]
        public IActionResult GroupMembers(string groupId)
        {
            return View();
        }
    }
}
