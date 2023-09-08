using Microsoft.Build.Evaluation;
using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations;

namespace TheBugTracker.Models
{
    public class Notification
    {
        private DateTime _created;
        public int Id { get; set; }
        public int? ProjectId { get; set; }
        public int? TicketId { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at most {1} characters long.")]
        public string? Title { get; set; }
        [Required]
        public string? Message { get; set; }
        public DateTime Created {
            get => _created;
            set => _created = value.ToUniversalTime();
        }
        [Required]
        public string? SenderId { get; set; }
        [Required]
        public string? RecipientId { get; set; }
        public int NotificationTypeId { get; set; }
        public bool HasBeenReviewed { get; set; }

        //--Nav Properties--
        //NotificationType?
        // Ticket?
        // Project?
        //Sender?
        //Recipient?
        public virtual NotificationType? NotificationType { get; set; }
        public virtual Ticket? Ticket { get; set; }
        public virtual Project? Project { get; set; }
        public virtual BTUser? Recipient { get; set; }
        public virtual BTUser? Sender { get; set; }




    }
}
