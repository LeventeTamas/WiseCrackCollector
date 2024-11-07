using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WiseCrackCollector.Models;
using WiseCrackCollector.Services;

namespace WiseCrackCollector.Controllers
{
    public class WisecracksController : Controller
    {
        private IWcCService wccService;

        public WisecracksController(IWcCService _wccService)
        {
            wccService = _wccService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [Route("/Wisecracks/Add")]
        public IActionResult Add(string new_wc_group_id, string new_wc_content, string? new_wc_saidBy)
        {
            string? userId = wccService.GetCurrentUserId();
            if (userId == null) return BadRequest();

            Group? group = wccService.GetGroupById(new_wc_group_id);
            if (group == null)
                return NotFound();

            // Check permission
            UserGroupPermissionSet userGroupPermissionSet;
            if (!wccService.CheckPermissionOnGroup(userId, group, UserGroupPermissionType.Add, out userGroupPermissionSet))
                return Forbid();

            Wisecrack newWisecrack = new Wisecrack()
            {
                Content = new_wc_content,
                SaidBy = new_wc_saidBy == null ? "unknown" : new_wc_saidBy
            };

            wccService.AddWisecrack(newWisecrack, new_wc_group_id, userId);

            return RedirectToAction("Details", new { groupId = new_wc_group_id });
        }

        [Authorize]
        [HttpPost]
        [Route("/Wisecracks/Delete")]
        public IActionResult Delete(string delete_wc_id)
        {
            string? userId = wccService.GetCurrentUserId();
            if (userId == null) return BadRequest();

            Wisecrack? wisecrack = wccService.GetWisecrackById(delete_wc_id);
            if (wisecrack == null)
                return NotFound();

            // Check permission
            UserGroupPermissionSet userGroupPermissionSet;
            if (!wccService.CheckPermissionOnGroup(userId, wisecrack.Group, UserGroupPermissionType.Delete, out userGroupPermissionSet))
                return Forbid();

            wccService.DeleteWisecrack(wisecrack);

            return RedirectToAction("Details", new { groupId = wisecrack.Group.Id });
        }

        [Authorize]
        [HttpPost]
        [Route("/Wisecracks/Update")]
        public IActionResult Update(string edit_wc_id, string edit_wc_saidBy, string edit_wc_content, string edit_wc_date)
        {
            string? userId = wccService.GetCurrentUserId();
            if (userId == null) return BadRequest();

            Wisecrack? wisecrack = wccService.GetWisecrackById(edit_wc_id);
            if (wisecrack == null)
                return NotFound();

            // Check permission
            UserGroupPermissionSet userGroupPermissionSet;
            if (!wccService.CheckPermissionOnGroup(userId, wisecrack.Group, UserGroupPermissionType.Update, out userGroupPermissionSet))
                return Forbid();

            if (string.IsNullOrEmpty(edit_wc_content))
                return BadRequest();
            if (string.IsNullOrEmpty(edit_wc_saidBy))
                edit_wc_saidBy = "unknown";

            wccService.UpdateWisecrack(edit_wc_id, edit_wc_content, edit_wc_saidBy, edit_wc_date);

            return RedirectToAction("Details", new { groupId = wisecrack.Group.Id });
        }
    }
}
