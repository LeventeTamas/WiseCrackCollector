using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WiseCrackCollector.Models
{
    [Table("Wisecracks")]
    public class Wisecrack
    {
        [Key, StringLength(100)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [StringLength(100)]
        public string SaidBy { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int Views { get; set; } = 0;

        [Required, ForeignKey("OwnerId")]
        public AppUser Owner { get; set; }

        [ForeignKey("GroupId")]
        public Group Group { get; set; }

    }
}
