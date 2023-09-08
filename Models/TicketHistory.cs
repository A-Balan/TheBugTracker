using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TheBugTracker.Models
{
    public class TicketHistory
    {
        private DateTime _created;
        public int Id { get; set; }

        public int TicketId { get; set; }

        [StringLength(200, ErrorMessage = "The {0} must be at most {1} characters long.")]
        public string? PropertyName { get; set; }
        public string? Description { get; set; }
        public DateTime Created
        {
            get => _created;
            set => _created = value.ToUniversalTime();
        }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }

        [Required]
        public string? UserId { get; set; }

        //--Nav Properties--
        public virtual Ticket? Ticket { get; set; }
        public virtual BTUser? User { get; set; }


    }
}
