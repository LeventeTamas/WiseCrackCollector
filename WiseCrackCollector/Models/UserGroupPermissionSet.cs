using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WiseCrackCollector.Models
{
    public enum UserGroupPermissionType
    {
        Read = 1,
        Add = 2,
        Update = 4,
        Delete = 8
    }

    [Table("UserGroupPermissions")]
    public class UserGroupPermissionSet
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string GroupId { get; set; }

        [Required]
        public bool Read { get; set; } = false;

        [Required]
        public bool Add { get; set; } = false;

        [Required]
        public bool Update { get; set; } = false;

        [Required]
        public bool Delete { get; set; } = false;

        public bool CheckPermission(UserGroupPermissionType permissionType)
        {
            switch (permissionType)
            {
                case UserGroupPermissionType.Read:
                    {
                        return Read;
                    }
                case UserGroupPermissionType.Add:
                    {
                        return Add;
                    }
                case UserGroupPermissionType.Update:
                    {
                        return Update;
                    }
                case UserGroupPermissionType.Delete:
                    {
                        return Delete;
                    }
                default:
                    {
                        return false;
                    }
            }
        }
    }
}
