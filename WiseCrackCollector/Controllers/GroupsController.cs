using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WiseCrackCollector.Models;
using WiseCrackCollector.Services;
using WiseCrackCollector.ViewModels;

namespace WiseCrackCollector.Controllers
{
    public class GroupsController : Controller
    {
        private IGroupService groupService;
        private IWisecrackService wisecrackService;
        private IMemberService memberService;
        private IAppUserService appUserService;

        public GroupsController(IGroupService _groupService, IWisecrackService _wisecrackService, IMemberService _memberService, IAppUserService _appUserService)
        {
            groupService = _groupService;
            wisecrackService = _wisecrackService;
            memberService = _memberService;
            appUserService = _appUserService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("MyGroups");
        }

        [Authorize]
        [Route("/Groups/MyGroups")]
        public IActionResult MyGroups()
        {
            List<Group> groups = groupService.GetGroupsOwnedByCurrentUser();
            Response.Cookies.Append("redirectTo", "MyGroups");
            return View(groups);
        }

        [Authorize]
        [Route("/Groups/MyMemberships")]
        public IActionResult MyMemberships()
        {
            List<Group> groups = groupService.GetGroupsConnectedToCurrentUser();
            Response.Cookies.Append("redirectTo", "MyMemberships");
            return View(groups);
        }

        [Authorize]
        [HttpPost]
        [Route("/Groups/New")]
        public IActionResult New(string new_group_name)
        {
            string newGroupId = groupService.CreateGroup(new_group_name);
            string userId = appUserService.GetCurrentUser().Id;
            GroupUserMembership groupUserMembership = new GroupUserMembership(userId, newGroupId)
            {
                Read = true,
                Add = true,
                Update = true,
                Delete = true,
                ManageMembers = true
            };
            memberService.AddMember(groupUserMembership);
            return RedirectToAction("Details", new { groupId = newGroupId });
        }

        [Authorize]
        [HttpPost]
        [Route("/Groups/Delete")]
        public IActionResult Delete(string delete_group_id)
        {
            // check if group exists
            if (!groupService.IsGroupExists(delete_group_id))
                return NotFound();

            // Check if the user has enough permission
            GroupUserMembership membership;
            if (!groupService.CheckPermissionOnGroup(delete_group_id, PermissionType.Delete, out membership))
                return Forbid();

            wisecrackService.DeleteWisecracksByGroupId(delete_group_id);
            groupService.DeleteGroup(delete_group_id);

            return RedirectToAction("MyGroups");
        }

        [Authorize]
        [HttpPost]

        [Route("/Groups/Empty")]
        public IActionResult Empty(string empty_group_id)
        {
            // check if group exists
            if (!groupService.IsGroupExists(empty_group_id))
                return NotFound();

            // Check if the user has enough permission
            GroupUserMembership membership;
            if (!groupService.CheckPermissionOnGroup(empty_group_id, PermissionType.Delete, out membership))
                return Forbid();

            wisecrackService.DeleteWisecracksByGroupId(empty_group_id);

            return RedirectToAction("Details", new { groupId = empty_group_id });
        }

        [Authorize]
        [HttpPost]
        [Route("/Groups/Update")]
        public IActionResult Update(string edit_group_id, string edit_group_name)
        {
            // check if group exists
            if (!groupService.IsGroupExists(edit_group_id))
                return NotFound();

            // Check if the user has enough permission
            GroupUserMembership membership;
            if (!groupService.CheckPermissionOnGroup(edit_group_id, PermissionType.Update, out membership))
                return Forbid();

            groupService.EditGroup(edit_group_id, edit_group_name);

            return RedirectToAction("MyGroups");
        }

        [Authorize]
        [Route("/Groups/{groupId}")]
        public IActionResult Details(string groupId)
        {
            string redirectTo = "MyGroups";
            if (Request.Cookies.ContainsKey("redirectTo"))
                redirectTo = Request.Cookies["redirectTo"];

            // check if group exists
            if (!groupService.IsGroupExists(groupId))
                return NotFound();

            // Check if the user has enough permission
            GroupUserMembership membership;
            if (!groupService.CheckPermissionOnGroup(groupId, PermissionType.Read, out membership))
                return Forbid();

            Group group = groupService.GetGroupById(groupId);
            List<Wisecrack> wisecracks = wisecrackService.GetWisecracksByGroupId(groupId);

            // Create view model
            GroupViewModel groupViewModel = new GroupViewModel()
            {
                Group = group,
                Wisecracks = wisecracks,
                Permissions = membership,
                RedirectAction = redirectTo == "MyMemberships" ? "MyMemberships" : "MyGroups",
                RedirectTitle = redirectTo == "MyMemberships" ? "Memberships" : "My Groups"
            };

            return View(groupViewModel);
        }
    }
}

