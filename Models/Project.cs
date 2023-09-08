using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TheBugTracker.Models
{
    public class Project
    {
        private DateTime _created;
        private DateTime _startdate;
        private DateTime _enddate;
        public int Id { get; set; }
        public int CompanyId { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at most {1} characters long.")]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        public DateTime Created
        {
            get => _created;
            set => _created = value.ToUniversalTime();
        }
        public DateTime StartDate
        {
            get => _startdate;
            set => _startdate = value.ToUniversalTime();
        }
        public DateTime EndDate
        {
            get => _enddate;
            set => _enddate = value.ToUniversalTime();
        }
        public int ProjectPriorityId { get; set; }

        //ImageFormFile(IFormFile)-avatar img
        public byte[]? ImageFileData { get; set; }

        //ImageFileData(byte[])
        public string? ImageFileType { get; set; }

        [NotMapped]
        //ImageFileType(string)-it's like the envelope the image in, so we don't store it
        public IFormFile? ImageFormFile { get; set; }

        public bool Archived { get; set; }

        //Nav Prop
        public virtual Company? Company { get; set; }
        public virtual ProjectPriority? ProjectPriority { get; set; }
        public virtual ICollection<BTUser> Members { get; set; } = new HashSet<BTUser>();

        public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}
