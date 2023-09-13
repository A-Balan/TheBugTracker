using Microsoft.EntityFrameworkCore;
using TheBugTracker.Data;
using TheBugTracker.Models;
using TheBugTracker.Services.Interfaces;

namespace TheBugTracker.Services
{
    public class BTTicketService : IBTTicketService
    {
        private readonly ApplicationDbContext _context;

        public BTTicketService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddTicketAsync(Ticket? ticket)
        {
            if (ticket == null) { return; }

            try
            {
                //saves it to database
                _context.Add(ticket!);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
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

        public async Task<Ticket> GetTicketByIdAsync(int? ticketId, int? companyId)
        {
            try
            {
                return await _context.Tickets
                              .Include(t => t.DeveloperUser)
                              .Include(t => t.SubmitterUser)
                              .Include(t => t.Project)
                              .Include(t => t.TicketPriority)
                              .Include(t => t.TicketStatus)
                              .Include(t => t.TicketType)
                              .Include(t => t.Comments)
                              .Include(t => t.Attachments)
                              .Include(t => t.History)
                              .FirstOrDefaultAsync(t => t.Id == ticketId);
            }
            catch (Exception)
            {

                throw;
            }
        }

		public async Task<TicketAttachment> GetTicketAttachmentByIdAsync(int ticketAttachmentId)
		{
			try
			{
				TicketAttachment ticketAttachment = await _context.TicketAttachments
																  .Include(t => t.BTUser)
																  .FirstOrDefaultAsync(t => t.Id == ticketAttachmentId);
				return ticketAttachment;
			}
			catch (Exception)
			{

				throw;
			}
		}

		public Task<BTUser?> GetTicketDeveloperAsync(int? ticketId, int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TicketPriority>> GetTicketPrioritiesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Ticket>> GetTicketsByProjectIdAsync(int? projectId)
        {
            List<Ticket> tickets = await _context.Tickets
                                                 .Where(t => t.ProjectId == projectId)
                                                 .Include(t => t.Attachments)
                                                 .Include(t => t.Project)
                                                 .Include(t => t.TicketType)
                                                 .Include(t => t.TicketStatus)
                                                 .Include(t => t.DeveloperUser)
                                                 .Include(t => t.SubmitterUser)
                                                 .ToListAsync();

            return tickets;
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
        public async Task AddTicketAttachmentAsync(TicketAttachment ticketAttachment)
        {
            try
            {
                await _context.AddAsync(ticketAttachment);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
