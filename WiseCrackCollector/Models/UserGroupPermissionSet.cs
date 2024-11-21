using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WiseCrackCollector.Models
{
    public enum UserGroupPermissionType
    {
        Read,
        Add,
        Update,
        Delete,
        ManageMembers
    }

    [Table("UserGroupPermissions")]
    public class UserGroupPermissionSet
    {
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

        public bool CheckPermission(UserGroupPermissionType permissionType)
        {
            switch (permissionType)
            {
                case UserGroupPermissionType.Read:
                    return Read;
                case UserGroupPermissionType.Add:
                    return Add;
                case UserGroupPermissionType.Update:
                    return Update;
                case UserGroupPermissionType.Delete:
                    return Delete;
                case UserGroupPermissionType.ManageMembers:
                    return ManageMembers;
                default:
                    return false;
            }
        }
    }
}
