using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TheBugTracker.Models
{
    public class TicketPriority
    {
        public int Id { get; set; }

        [StringLength(200, ErrorMessage = "The {0} must be at most {1} characters long.")]
        public string? Name { get; set; }
    }
}
