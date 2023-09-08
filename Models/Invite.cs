using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Build.Evaluation;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TheBugTracker.Models
{
    public class Invite
    {
        private DateTime _invited;
        private DateTime? _joined;

        public int Id { get; set; }
        public DateTime InviteDate {
            get => _invited;
            set => _invited = value.ToUniversalTime();
        }
        public DateTime? JoinDate
        {
            get => _joined;
            set => _joined = value.HasValue ? value.Value.ToUniversalTime() : null;
        }
        public Guid CompanyToken { get; set; }
        public int CompanyId { get; set; }
        public int? ProjectId { get; set; }
        [Required]
        public string? InvitorId { get; set; }

        public string? InviteeId { get; set; }
        [Required]
        public string? InviteeEmail { get; set; }
        [Required]
        public string? InviteeFirstName { get; set; }
        [Required]
        public string? InviteeLastName { get; set; }
        public string? Message { get; set; }

        public bool IsValid { get; set; }

        //Nav Prop
        public virtual Company? Company { get; set; }
        public virtual Project? Project { get; set; }
        public virtual BTUser? Invitor { get; set; }
        public virtual BTUser? Invitee { get; set; }

    }
}
