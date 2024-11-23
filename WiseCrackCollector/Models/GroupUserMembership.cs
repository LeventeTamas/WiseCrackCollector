using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WiseCrackCollector.Models
{
    public enum PermissionType
    {
        Read,
        Add,
        Update,
        Delete,
        ManageMembers
    }

    [Table("GroupUserMemberships")]
    public class GroupUserMembership
    {
        public GroupUserMembership(string userId, string groupId)
        {
            UserId = userId;
            GroupId = groupId;
        }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public AppUser User { get; set; }

        [Required]
        public string GroupId { get; set; }

        [ForeignKey("GroupId")]
        public Group Group { get; set; }

        [Required]
        public bool Read { get; set; } = false;

        [Required]
        public bool Add { get; set; } = false;

        [Required]
        public bool Update { get; set; } = false;

        [Required]
        public bool Delete { get; set; } = false;

        [Required]
        public bool ManageMembers { get; set; } = false;

        public bool CheckPermission(PermissionType permissionType)
        {
            switch (permissionType)
            {
                case PermissionType.Read:
                    return Read;
                case PermissionType.Add:
                    return Add;
                case PermissionType.Update:
                    return Update;
                case PermissionType.Delete:
                    return Delete;
                case PermissionType.ManageMembers:
                    return ManageMembers;
                default:
                    return false;
            }
        }
    }
}
