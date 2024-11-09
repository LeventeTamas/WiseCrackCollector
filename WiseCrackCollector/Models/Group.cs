using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WiseCrackCollector.Models
{
    [Table("Groups")]
    public class Group
    {
        [Key, StringLength(100)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<Wisecrack> Wisecracks = new List<Wisecrack>();

        [Required, ForeignKey("OwnerId")]
        public AppUser Owner { get; set; }

        public List<UserGroupPermissionSet> userGroupPermissionSets;
    }
}
