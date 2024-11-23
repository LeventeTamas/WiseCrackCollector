using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WiseCrackCollector.Models;
using WiseCrackCollector.Services;
using WiseCrackCollector.ViewModels;

namespace WiseCrackCollector.Controllers
{
    public class MembersController : Controller
    {

        private IMemberService memberService;
        private IGroupService groupService;
        private IAppUserService appUserService;

        public MembersController(IMemberService _memberService, IGroupService _groupService, IAppUserService _appUserService)
        {
            memberService = _memberService;
            groupService = _groupService;
            appUserService = _appUserService;
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
            List<GroupUserMembership> memberships = memberService.GetMembershipsByGroupId(groupId);
            List<AppUser> allUsers = appUserService.GetAppUsers();
            AppUser currentUser = appUserService.GetCurrentUser();

            List<AppUser> NotMemberUsers = allUsers.Where(i => !i.Id.Equals(currentUser.Id) && !memberships.Any(m => m.UserId.Equals(i.Id))).ToList();

            GroupMembersViewModel groupMembersViewModel = new GroupMembersViewModel()
            {
                Group = group,
                NotMemberUsers = NotMemberUsers,
                Memberships = memberships
            };

            return View(groupMembersViewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(string new_member_groupId, string new_member_id, string new_member_perm_add, string new_member_perm_read, string new_member_perm_update, string new_member_perm_delete, string new_member_perm_members)
        {
            GroupUserMembership groupUserMembership = new GroupUserMembership(new_member_id, new_member_groupId)
            {
                Add = new_member_perm_add == "on",
                Read = new_member_perm_read == "on",
                Update = new_member_perm_update == "on",
                Delete = new_member_perm_delete == "on",
                ManageMembers = new_member_perm_members == "on"

            };
            memberService.AddMember(groupUserMembership);
            return RedirectToAction("GroupMembers", new { groupId = new_member_groupId });
        }
    }
}
