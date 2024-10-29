using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WiseCrackCollector.Data;
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
            return RedirectToAction("MyGroups");
        }

        [Authorize]
        [Route("/WcC/ViewGroup/{groupId}")]
        public IActionResult ViewGroup(string groupId) 
        {
            string? userId = GetCurrentUserId();
            if (userId == null) return BadRequest();

            UserGroupPermission? userGroupPermission = wccService.GetUserGroupPermission(userId, groupId);
            Group? group = wccService.GetGroupById(groupId);
            if (group == null)
                return NotFound();
            if (!group.Owner.Id.Equals(userId) && (userGroupPermission == null || !userGroupPermission.Read))
                return Forbid();
            
            ViewBag.Group = group;
            ViewBag.Permissions = userGroupPermission;

            return View();
        }
    }
}