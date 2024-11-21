﻿using Microsoft.AspNetCore.Authorization;
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

        public GroupsController(IGroupService _groupService, IWisecrackService _wisecrackService)
        {
            groupService = _groupService;
            wisecrackService = _wisecrackService;
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
            return View(groups);
        }

        [Authorize]
        [HttpPost]
        [Route("/Groups/New")]
        public IActionResult New(string new_group_name)
        {
            string newGroupId = groupService.CreateGroup(new_group_name);
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
            UserGroupPermissionSet userGroupPermissionSet;
            if (!groupService.CheckPermissionOnGroup(delete_group_id, UserGroupPermissionType.Delete, out userGroupPermissionSet))
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
            UserGroupPermissionSet userGroupPermissionSet;
            if (!groupService.CheckPermissionOnGroup(empty_group_id, UserGroupPermissionType.Delete, out userGroupPermissionSet))
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
            UserGroupPermissionSet userGroupPermissionSet;
            if (!groupService.CheckPermissionOnGroup(edit_group_id, UserGroupPermissionType.Update, out userGroupPermissionSet))
                return Forbid();

            groupService.EditGroup(edit_group_id, edit_group_name);

            return RedirectToAction("MyGroups");
        }

        [Authorize]
        [Route("/Groups/{groupId}")]
        public IActionResult Details(string groupId)
        {
            // check if group exists
            if (!groupService.IsGroupExists(groupId))
                return NotFound();

            // Check if the user has enough permission
            UserGroupPermissionSet userGroupPermissionSet;
            if (!groupService.CheckPermissionOnGroup(groupId, UserGroupPermissionType.Read, out userGroupPermissionSet))
                return Forbid();

            Group group = groupService.GetGroupById(groupId);

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
