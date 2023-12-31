﻿using Microsoft.EntityFrameworkCore;
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

        public async Task AssignTicketAsync(int? ticketId, string? userId)
        {
            try
            {
                if (ticketId != null && !string.IsNullOrEmpty(userId))
                {
                    BTUser? btUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                    Ticket? ticket = await GetTicketByIdAsync(ticketId, btUser!.CompanyId);

                    if (ticket != null && btUser != null)
                    {
                        ticket!.DeveloperUserId = userId;
                        //todo-set ticket status to "development" with lookupservice
                        await UpdateTicketAsync(ticket);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Ticket>> GetAllTicketsByCompanyIdAsync(int? companyId)
        {

            //IEnumerable<Ticket> tickets = _context.Tickets?.Where(t => t.Project.CompanyId == companyId);
           // return tickets.ToList();

            try
            {
                List<Ticket> tickets = await _context.Projects
                    .Where(p => p.CompanyId == companyId)
                    .Include(t => t.Tickets).ThenInclude(t=>t.Comments).ThenInclude(c=>c.User)
                    .Include(p=>p.Tickets).ThenInclude(t=>t.Attachments)
                    .Include(p=>p.Tickets).ThenInclude(t=>t.History)
                    .SelectMany(p=>p.Tickets)
                    .ToListAsync();
                return tickets;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public async Task<Ticket> GetTicketAsNoTrackingAsync(int? ticketId, int? companyId)
        {
            try
            {
                Ticket? ticket = await _context.Tickets
                    .Include(t => t.Project)
                    .ThenInclude(p =>p!.Company)
                    .Include(t=>t.Attachments)
                    .Include(t=>t.Comments)
                    .Include(t=>t.DeveloperUser)
                    .Include(t=>t.History)
                    .Include(t=>t.SubmitterUser)
                    .Include(t=>t.TicketPriority)
                    .Include(t=>t.TicketStatus)
                    .Include(t=>t.TicketType)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t=>t.Id == ticketId && t.Project!.CompanyId == companyId && t.Archived == false);

                return ticket!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<TicketAttachment?> GetTicketAttachmentByIdAsync(int? ticketAttachmentId)
        {
            throw new NotImplementedException();
        }

        public async Task<Ticket> GetTicketByIdAsync(int? ticketId, int? companyId)
        {
            try
            {
                Ticket? ticket = new();

                if (ticketId != null && companyId != null)
                {
                   ticket = await _context.Tickets
                        .Where(t => t.Project!.CompanyId == companyId && t.Archived == false  && t.ArchivedByProject == false)
                           .Include(t => t.Project)
                                  .ThenInclude(p => p!.Company)
                                  .Include(t => t.DeveloperUser)
                                  .Include(t => t.SubmitterUser)
                                  .Include(t => t.TicketPriority)
                                  .Include(t => t.TicketStatus)
                                  .Include(t => t.TicketType)
                                  .Include(t => t.Comments)
                                  .Include(t => t.Attachments)
                                  .Include(t => t.History)
                                  .FirstOrDefaultAsync(t => t.Id == ticketId);
                }
                return ticket!;
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

        public async Task UpdateTicketAsync(Ticket? ticket)
        {
            try
            {
                if (ticket != null)
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
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
