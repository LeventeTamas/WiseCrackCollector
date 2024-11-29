using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        public IActionResult Add(string new_member_group_id, string new_member_user_id, string new_member_perm_add, string new_member_perm_read, string new_member_perm_update, string new_member_perm_delete, string new_member_perm_members)
        {
            GroupUserMembership groupUserMembership = new GroupUserMembership(new_member_user_id, new_member_group_id)
            {
                Add = !string.IsNullOrEmpty(new_member_perm_add),
                Read = !string.IsNullOrEmpty(new_member_perm_read),
                Update = !string.IsNullOrEmpty(new_member_perm_update),
                Delete = !string.IsNullOrEmpty(new_member_perm_delete),
                ManageMembers = !string.IsNullOrEmpty(new_member_perm_members)
            };
            memberService.AddMember(groupUserMembership);
            return RedirectToAction("GroupMembers", new { groupId = new_member_group_id });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Delete(string delete_member_user_id, string delete_member_group_id)
        {
            // check if group exists
            if (!groupService.IsGroupExists(delete_member_group_id))
                return NotFound();

            // check if membership exists
            if (!memberService.IsMembershipExists(delete_member_group_id, delete_member_user_id))
                return NotFound();

            // Check if the user has enough permission
            GroupUserMembership membership;
            if (!groupService.CheckPermissionOnGroup(delete_member_group_id, PermissionType.ManageMembers, out membership))
                return Forbid();

            memberService.DeleteMembership(delete_member_group_id, delete_member_user_id);

            return RedirectToAction("GroupMembers", new { groupId = delete_member_group_id });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Update(string edit_membership_group_id, string edit_membership_user_id, string edit_membership_perm_add, string edit_membership_perm_read, string edit_membership_perm_update, string edit_membership_perm_delete, string edit_membership_perm_members)
        {
            // check if group exists
            if (!groupService.IsGroupExists(edit_membership_group_id))
                return NotFound();

            // check if membership exists
            if (!memberService.IsMembershipExists(edit_membership_group_id, edit_membership_user_id))
                return NotFound();

            // Check if the user has enough permission
            GroupUserMembership currentUserPermissions;
            if (!groupService.CheckPermissionOnGroup(edit_membership_group_id, PermissionType.ManageMembers, out currentUserPermissions))
                return Forbid();

            // Update membership
            GroupUserMembership membership = memberService.GetGroupUserMembershipById(edit_membership_group_id, edit_membership_user_id);
            membership.Add = !string.IsNullOrEmpty(edit_membership_perm_add);
            membership.Read = !string.IsNullOrEmpty(edit_membership_perm_read);
            membership.Update = !string.IsNullOrEmpty(edit_membership_perm_update);
            membership.Delete = !string.IsNullOrEmpty(edit_membership_perm_delete);
            membership.ManageMembers = !string.IsNullOrEmpty(edit_membership_perm_members);
            memberService.UpdateMembership(membership);

            return RedirectToAction("GroupMembers", new { groupId = edit_membership_group_id });
        }
    }
}
