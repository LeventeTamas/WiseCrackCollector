using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WiseCrackCollector.Models;
using WiseCrackCollector.Services;

namespace WiseCrackCollector.Controllers
{
    public class WisecracksController : Controller
    {
        private IWisecrackService wisecrackService;
        private IGroupService groupService;

        public WisecracksController(IWisecrackService _wisecrackService, IGroupService _groupService)
        {
            wisecrackService = _wisecrackService;
            groupService = _groupService;
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
            // check if group exists
            if (!groupService.IsGroupExists(new_wc_group_id))
                return NotFound();

            // Check if the user has enough permission
            UserGroupPermissionSet userGroupPermissionSet;
            if (!groupService.CheckPermissionOnGroup(new_wc_group_id, UserGroupPermissionType.Add, out userGroupPermissionSet))
                return Forbid();

            Wisecrack newWisecrack = new Wisecrack()
            {
                Content = new_wc_content,
                SaidBy = new_wc_saidBy == null ? "unknown" : new_wc_saidBy
            };

            wisecrackService.AddWisecrack(new_wc_group_id, newWisecrack);

            return RedirectToAction("Details", "Groups", new { groupId = new_wc_group_id });
        }

        [Authorize]
        [HttpPost]
        [Route("/Wisecracks/Delete")]
        public IActionResult Delete(string delete_wc_id)
        {
            // check if wisecrack exists
            if (!wisecrackService.IsWisecrackExists(delete_wc_id))
                return NotFound();

            Wisecrack wisecrack = wisecrackService.GetWisecrackById(delete_wc_id);

            // Check if the user has enough permission
            UserGroupPermissionSet userGroupPermissionSet;
            if (!groupService.CheckPermissionOnGroup(wisecrack.Group.Id, UserGroupPermissionType.Delete, out userGroupPermissionSet))
                return Forbid();

            wisecrackService.DeleteWisecrack(wisecrack);

            return RedirectToAction("Details", "Groups", new { groupId = wisecrack.Group.Id });
        }

        [Authorize]
        [HttpPost]
        [Route("/Wisecracks/Update")]
        public IActionResult Update(string edit_wc_id, string edit_wc_saidBy, string edit_wc_content, string edit_wc_date)
        {
            // check if wisecrack exists
            if (!wisecrackService.IsWisecrackExists(edit_wc_id))
                return NotFound();

            Wisecrack wisecrack = wisecrackService.GetWisecrackById(edit_wc_id);

            // check if the user has enough permission
            UserGroupPermissionSet userGroupPermissionSet;
            if (!groupService.CheckPermissionOnGroup(wisecrack.Group.Id, UserGroupPermissionType.Update, out userGroupPermissionSet))
                return Forbid();

            if (string.IsNullOrEmpty(edit_wc_content))
                return BadRequest();
            if (string.IsNullOrEmpty(edit_wc_saidBy))
                edit_wc_saidBy = "unknown";
            

            wisecrack.Content = edit_wc_content;
            wisecrack.CreatedAt = DateTime.Parse(edit_wc_date);
            wisecrack.SaidBy = edit_wc_saidBy;

            wisecrackService.UpdateWisecrack(wisecrack);

            return RedirectToAction("Details", "Groups", new { groupId = wisecrack.Group.Id });
        }
    }
}
