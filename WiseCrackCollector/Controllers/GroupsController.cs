using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WiseCrackCollector.Models;
using WiseCrackCollector.Services;

namespace WiseCrackCollector.Controllers
{
    public class GroupsController : Controller
    {
        private IWcCService wccService;

        public GroupsController(IWcCService _wccService)
        {
            wccService = _wccService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("MyGroups");
        }

        [Authorize]
        [Route("/Groups/MyGroups")]
        public IActionResult MyGroups()
        {
            string? userId = wccService.GetCurrentUserId();
            if (userId == null) return BadRequest();

            List<Group> groups = wccService.GetGroupsOwnedByUser(userId);
            return View(groups);
        }

        [Authorize]
        [HttpPost]
        [Route("/Groups/New")]
        public IActionResult New(string new_group_name)
        {
            string? userId = wccService.GetCurrentUserId();
            if (userId == null) return BadRequest();

            string newGroupId = wccService.CreateGroup(userId, new_group_name);
            return RedirectToAction("Details", new { groupId = newGroupId });
        }

        [Authorize]
        [HttpPost]
        [Route("/Groups/Delete")]
        public IActionResult Delete(string delete_group_id)
        {
            string? userId = wccService.GetCurrentUserId();
            if (userId == null) return BadRequest();

            Group? group = wccService.GetGroupById(delete_group_id);
            if (group == null)
                return NotFound();

            // Check permission
            UserGroupPermissionSet userGroupPermissionSet;
            if (!wccService.CheckPermissionOnGroup(userId, group, UserGroupPermissionType.Delete, out userGroupPermissionSet))
                return Forbid();

            wccService.DeleteGroup(delete_group_id);
            return RedirectToAction("MyGroups");
        }

        [Authorize]
        [HttpPost]

        [Route("/Groups/Empty")]
        public IActionResult Empty(string empty_group_id)
        {
            string? userId = wccService.GetCurrentUserId();
            if (userId == null) return BadRequest();

            Group? group = wccService.GetGroupById(empty_group_id);
            if (group == null)
                return NotFound();

            // Check permission
            UserGroupPermissionSet userGroupPermissionSet;
            if (!wccService.CheckPermissionOnGroup(userId, group, UserGroupPermissionType.Delete, out userGroupPermissionSet))
                return Forbid();

            wccService.EmptyGroup(empty_group_id);

            return RedirectToAction("Details", new { groupId = empty_group_id });
        }

        [Authorize]
        [HttpPost]
        [Route("/Groups/Update")]
        public IActionResult Update(string edit_group_id, string edit_group_name)
        {
            string? userId = wccService.GetCurrentUserId();
            if (userId == null) return BadRequest();

            Group? group = wccService.GetGroupById(edit_group_id);
            if (group == null)
                return NotFound();

            // Check permission
            UserGroupPermissionSet userGroupPermissionSet;
            if (!wccService.CheckPermissionOnGroup(userId, group, UserGroupPermissionType.Update, out userGroupPermissionSet))
                return Forbid();

            wccService.EditGroup(edit_group_id, edit_group_name);
            return RedirectToAction("MyGroups");
        }

        [Authorize]
        [Route("/Groups/{groupId}")]
        public IActionResult Details(string groupId)
        {
            string? userId = wccService.GetCurrentUserId();
            if (userId == null) return BadRequest();

            Group? group = wccService.GetGroupById(groupId);
            if (group == null)
                return NotFound();

            // Check permission
            UserGroupPermissionSet userGroupPermissionSet;
            if (!wccService.CheckPermissionOnGroup(userId, group, UserGroupPermissionType.Read, out userGroupPermissionSet))
                return Forbid();


            // Create view model
            GroupViewModel groupViewModel = new GroupViewModel()
            {
                Group = group,
                Wisecracks = group.Wisecracks,
                Permissions = userGroupPermissionSet,
                SortBy = WisecrackListSortBy.Date,
                SortOrder = WisecrackListSortOrder.Descending
            };

            return View(groupViewModel);
        }
    }
}

