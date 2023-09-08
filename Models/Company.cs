using Microsoft.Build.Evaluation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBugTracker.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at most {1} characters long.")]
        public string? Name { get; set; }
        public string? Description { get; set; }

        //ImageFormFile(IFormFile)-avatar img
        public byte[]? ImageFileData { get; set; }

        //ImageFileData(byte[])
        public string? ImageFileType { get; set; }

        [NotMapped]
        //ImageFileType(string)-it's like the envelope the image in, so we don't store it
        public IFormFile? ImageFormFile { get; set; }

        //--Nav Properties--
        //projects
        //members
        //invites
        public virtual ICollection<BTUser> Members { get; set; } = new HashSet<BTUser>();
        public virtual ICollection<Project> Projects { get; set; } = new HashSet<Project>();
        public virtual ICollection<Invite> Invites { get; set; } = new HashSet<Invite>();
    }
}
