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

        [Authorize]
        public IActionResult Index() 
        {
            return View();
        }

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
            return RedirectToAction("ViewGroup", new { groupId = newGroupId });
        }

        [Authorize]
        [Route("/WcC/ViewGroup/{groupId}")]
        public IActionResult ViewGroup(string groupId) 
        {
            string? userId = GetCurrentUserId();
            if (userId == null) return BadRequest();

            Group? group = wccService.GetGroupById(groupId);
            if (group == null)
                return NotFound();

            UserGroupPermissionSet? userGroupPermission = wccService.GetUserGroupPermissions(userId, groupId);
            if (!group.Owner.Id.Equals(userId) && (userGroupPermission == null || !userGroupPermission.Read))
                return Forbid();
            
            ViewBag.Group = group;
            ViewBag.Permissions = userGroupPermission;

            return View();
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

            UserGroupPermissionSet? userGroupPermission = wccService.GetUserGroupPermissions(userId, delete_group_id);
            if (!group.Owner.Id.Equals(userId) && (userGroupPermission == null || !userGroupPermission.Delete))
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

            UserGroupPermissionSet? userGroupPermission = wccService.GetUserGroupPermissions(userId, edit_group_id);
            if (!group.Owner.Id.Equals(userId) && (userGroupPermission == null || !userGroupPermission.Update))
                return Forbid();

            wccService.EditGroup(edit_group_id, edit_group_name);
            return RedirectToAction("MyGroups");
        }
    }
}