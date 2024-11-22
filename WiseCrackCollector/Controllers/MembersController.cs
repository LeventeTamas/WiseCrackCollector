using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WiseCrackCollector.Models;
using WiseCrackCollector.Services;

namespace WiseCrackCollector.Controllers
{
    public class MembersController : Controller
    {

        private IMemberService memberService;
        private IGroupService groupService;

        public MembersController(IMemberService _memberService, IGroupService _groupService)
        {
            memberService = _memberService;
            groupService = _groupService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [Route("/Members/GroupMembers")]
        public IActionResult GroupMembers(string groupId)
        {
            // check if group exists
            if (!groupService.IsGroupExists(groupId))
                return NotFound();

            // Check if the user has enough permission
            GroupUserMembership membership;
            if (!groupService.CheckPermissionOnGroup(groupId, PermissionType.ManageMembers, out membership))
                return Forbid();

            Group group = groupService.GetGroupById(groupId);

            return View(group);
        }
    }
}
