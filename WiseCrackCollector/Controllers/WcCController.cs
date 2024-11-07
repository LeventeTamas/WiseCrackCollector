using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WiseCrackCollector.Models;
using WiseCrackCollector.Services;

namespace WiseCrackCollector.Controllers
{
    public class WccController : Controller
    {
        private IWcCService wccService;
        private IHttpContextAccessor httpContextAccessor;

        public WccController(IWcCService _wccService, IHttpContextAccessor _httpContextAccessor) 
        {
            httpContextAccessor = _httpContextAccessor;
            wccService = _wccService;
        }

        private string? GetCurrentUserId()
        {
            HttpContext? httpContext = httpContextAccessor.HttpContext;
            if (httpContext == null) return null;

            ClaimsIdentity? identity = (ClaimsIdentity?)httpContext.User.Identity;
            if (identity == null) return null;

            Claim? claim = identity.FindFirst(ClaimTypes.NameIdentifier);
            if(claim == null) return null;

            return claim.Value;
        }

        private bool CheckPermission(string userId, Group group, UserGroupPermissionType permission, out UserGroupPermissionSet userGroupPermissionSet)
        {

            if (group.Owner.Id.Equals(userId))
            {
                userGroupPermissionSet = new UserGroupPermissionSet() { Read = true, Update = true, Delete = true, Add = true };
                return true;
            }

            userGroupPermissionSet = wccService.GetUserGroupPermissions(userId, group.Id);
            return userGroupPermissionSet != null && userGroupPermissionSet.CheckPermission(permission);
        }

        [Authorize]
        public IActionResult Index() 
        {
            return View();
        }

        #region Groups

        [Authorize]
        public IActionResult MyGroups() 
        {
            string? userId = GetCurrentUserId();
            if (userId == null) return BadRequest();

            List<Group> groups = wccService.GetGroupsOwnedByUser(userId);
            return View(groups);
        }

        [Authorize]
        [HttpPost]
        public IActionResult NewGroup(string new_group_name)
        {
            string? userId = GetCurrentUserId();
            if (userId == null) return BadRequest();

            string newGroupId = wccService.CreateGroup(userId, new_group_name);
            return RedirectToAction("Group", new { groupId = newGroupId });
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteGroup(string delete_group_id) 
        {
            string? userId = GetCurrentUserId();
            if (userId == null) return BadRequest();

            Group? group = wccService.GetGroupById(delete_group_id);
            if (group == null)
                return NotFound();

            // Check permission
            UserGroupPermissionSet userGroupPermissionSet;
            if (!CheckPermission(userId, group, UserGroupPermissionType.Delete, out userGroupPermissionSet))
                return Forbid();

            wccService.DeleteGroup(delete_group_id);
            return RedirectToAction("MyGroups");
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditGroup(string edit_group_id, string edit_group_name)
        {
            string? userId = GetCurrentUserId();
            if (userId == null) return BadRequest();

            Group? group = wccService.GetGroupById(edit_group_id);
            if (group == null)
                return NotFound();

            // Check permission
            UserGroupPermissionSet userGroupPermissionSet;
            if (!CheckPermission(userId, group, UserGroupPermissionType.Update, out userGroupPermissionSet))
                return Forbid();

            wccService.EditGroup(edit_group_id, edit_group_name);
            return RedirectToAction("MyGroups");
        }

        [Authorize]
        [Route("/WcC/Group/{groupId}")]
        public IActionResult Group(string groupId)
        {
            string? userId = GetCurrentUserId();
            if (userId == null) return BadRequest();

            Group? group = wccService.GetGroupById(groupId);
            if (group == null)
                return NotFound();

            // Check permission
            UserGroupPermissionSet userGroupPermissionSet;
            if (!CheckPermission(userId, group, UserGroupPermissionType.Read, out userGroupPermissionSet))
                return Forbid();
            

            // Create view model
            GroupViewModel groupViewModel = new GroupViewModel()
            {
                Group = group,
                Permissions = userGroupPermissionSet,
                SortBy = WisecrackListSortBy.Date,
                SortOrder = WisecrackListSortOrder.Descending
            };

            return View(groupViewModel);
        }

        #endregion

        #region Wisecracks
        [Authorize]
        [HttpPost]
        public IActionResult AddWisecrack(string new_wc_group_id, string new_wc_content, string? new_wc_saidBy)
        {
            string? userId = GetCurrentUserId();
            if (userId == null) return BadRequest();

            Group? group = wccService.GetGroupById(new_wc_group_id);
            if (group == null)
                return NotFound();

            // Check permission
            UserGroupPermissionSet userGroupPermissionSet;
            if (!CheckPermission(userId, group, UserGroupPermissionType.Add, out userGroupPermissionSet))
                return Forbid();

            Wisecrack newWisecrack = new Wisecrack() { 
                Content = new_wc_content,
                SaidBy = new_wc_saidBy == null ? "unknown" : new_wc_saidBy
            };

            wccService.AddWisecrack(newWisecrack, new_wc_group_id, userId);

            return RedirectToAction("Group", new { groupId = new_wc_group_id });
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteWisecrack(string delete_wc_id)
        {
            string? userId = GetCurrentUserId();
            if (userId == null) return BadRequest();

            Wisecrack? wisecrack = wccService.GetWisecrackById(delete_wc_id);
            if (wisecrack == null)
                return NotFound();

            // Check permission
            UserGroupPermissionSet userGroupPermissionSet;
            if (!CheckPermission(userId, wisecrack.Group, UserGroupPermissionType.Delete, out userGroupPermissionSet))
                return Forbid();

            wccService.DeleteWisecrack(wisecrack);

            return RedirectToAction("Group", new { groupId = wisecrack.Group.Id });
        }

        [Authorize]
        [HttpPost]
        public IActionResult EmptyGroup(string empty_group_id)
        {
            string? userId = GetCurrentUserId();
            if (userId == null) return BadRequest();

            Group? group = wccService.GetGroupById(empty_group_id);
            if (group == null)
                return NotFound();

            // Check permission
            UserGroupPermissionSet userGroupPermissionSet;
            if (!CheckPermission(userId, group, UserGroupPermissionType.Delete, out userGroupPermissionSet))
                return Forbid();

            wccService.EmptyGroup(empty_group_id);

            return RedirectToAction("Group", new { groupId = empty_group_id });
        }

        [Authorize]
        [HttpPost]
        public IActionResult UpdateWisecrack(string edit_wc_id, string edit_wc_saidBy, string edit_wc_content, string edit_wc_date)
        {
            string? userId = GetCurrentUserId();
            if (userId == null) return BadRequest();

            Wisecrack? wisecrack = wccService.GetWisecrackById(edit_wc_id);
            if (wisecrack == null)
                return NotFound();

            // Check permission
            UserGroupPermissionSet userGroupPermissionSet;
            if (!CheckPermission(userId, wisecrack.Group, UserGroupPermissionType.Update, out userGroupPermissionSet))
                return Forbid();

            if (string.IsNullOrEmpty(edit_wc_content))
                return BadRequest();
            if (string.IsNullOrEmpty(edit_wc_saidBy))
                edit_wc_saidBy = "unknown";

            wccService.UpdateWisecrack(edit_wc_id, edit_wc_content, edit_wc_saidBy, edit_wc_date);

            return RedirectToAction("Group", new { groupId = wisecrack.Group.Id });
        }
        #endregion
    }
}