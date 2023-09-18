using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace TheBugTracker.Models
{
    public class TicketComment
    {
        private DateTime _created;
		private DateTime? _updated;
	
        public int Id { get; set; }
        

        public DateTime Created
        {
            get => _created;
            set => _created = value.ToUniversalTime();
        }

		public DateTime? Updated
		{
			get => _updated;
			set => _updated = value.HasValue ? value.Value.ToUniversalTime() : null;
		}
		[Display(Name = "Update Reason")]
		[StringLength(200, ErrorMessage = "The {0} must be at most {1} characters long.")]
		public string? UpdateReason { get; set; }

		[Required]
		[Display(Name = "Comment")]
		[StringLength(5000, ErrorMessage = "The {0} must be at most {1} characters long.", MinimumLength = 2)]
		public string? Comment { get; set; }

		public int TicketId { get; set; }
        public string? UserId { get; set; }

        //--Nav Properties--
        public virtual Ticket? Ticket { get; set; }
        public virtual BTUser? User { get; set; }
    }
}
