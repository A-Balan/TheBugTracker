using TheBugTracker.Models;
using TheBugTracker.Services.Interfaces;

namespace TheBugTracker.Services
{
    public class BTTicketService : IBTTicketService
    {
        public Task AddTicketAsync(Ticket? ticket)
        {
            throw new NotImplementedException();
        }

        public Task AddTicketAttachmentAsync(TicketAttachment? ticketAttachment)
        {
            throw new NotImplementedException();
        }

        public Task AddTicketCommentAsync(TicketComment? ticketComment)
        {
            throw new NotImplementedException();
        }

        public Task ArchiveTicketAsync(Ticket? ticket)
        {
            throw new NotImplementedException();
        }

        public Task AssignTicketAsync(int? ticketId, string? userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetAllTicketsByCompanyIdAsync(int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task<Ticket> GetTicketAsNoTrackingAsync(int? ticketId, int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task<TicketAttachment?> GetTicketAttachmentByIdAsync(int? ticketAttachmentId)
        {
            throw new NotImplementedException();
        }

        public Task<Ticket> GetTicketByIdAsync(int? ticketId, int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task<BTUser?> GetTicketDeveloperAsync(int? ticketId, int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TicketPriority>> GetTicketPrioritiesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetTicketsByUserIdAsync(string? userId, int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TicketStatus>> GetTicketStatusesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TicketType>> GetTicketTypesAsync()
        {
            throw new NotImplementedException();
        }

        public Task RestoreTicketAsync(Ticket? ticket)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTicketAsync(Ticket? ticket)
        {
            throw new NotImplementedException();
        }
    }
}
