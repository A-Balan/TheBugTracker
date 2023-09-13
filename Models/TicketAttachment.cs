using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheBugTracker.Extensions;
using TheBugTracker.Services;
namespace TheBugTracker.Models
{
    public class TicketAttachment
    {
        [NotMapped]
        [DisplayName("Select a file")]
        [DataType(DataType.Upload)]
        [MaxFileSize(1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".doc", ".docx", ".xls", ".xlsx", ".pdf" })]
        public IFormFile FormFile { get; set; }

        private DateTime _created;
        public int Id { get; set; }
        public string? Description { get; set; }
        public DateTime Created
        {
            get => _created;
            set => _created = value.ToUniversalTime();
        }
        public int TicketId { get; set; }
        [Required]
        public string? BTUserId { get; set; }

        public string? FileName { get; set; }

        public byte[]? FileData { get; set; }

        public string? FileType { get; set; }

        //--Nav Properties--
        public virtual Ticket? Ticket { get; set; }
        public virtual BTUser? BTUser { get; set; }
    }
}
