using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;

namespace TheBugTracker.Models
{
    public class TicketAttachment
    {
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

        [NotMapped]
        public IFormFile? FormFile { get; set; }
        public byte[]? FileData { get; set; }

        public string? FileType { get; set; }

        //--Nav Properties--
        public virtual Ticket? Ticket { get; set; }
        public virtual BTUser? BTUser { get; set; }
    }
}
