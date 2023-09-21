using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBugTracker.Models
{
    public class BTUser : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        [StringLength(100, ErrorMessage = "The {0} must be at most {1} characters long.")]
        public string? FirstName { get; set; }
        
        [Required]
        [Display(Name = "Last Name")]
        [StringLength(100, ErrorMessage = "The {0} must be at most {1} characters long.")]
        public string? LastName { get; set; }

        [NotMapped]
        [Display(Name = "Full Name")]
        public string FullName { get { return $"{FirstName} {LastName}"; } }

        //ImageFormFile(IFormFile)-avatar img
        public byte[]? ImageData { get; set; }

        //ImageFileData(byte[])
        public string? ImageType { get; set; }

        [NotMapped]
        //ImageFileType(string)-it's like the envelope the image in, so we don't store it
        public IFormFile? ImageFile { get; set; }

        public int CompanyId { get; set; }

        //-- Navigation properties --
        public virtual Company? Company { get; set; }
        public virtual ICollection<Project> Projects { get; set; } = new HashSet<Project>();

    }
}
