using Humanizer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.Design;
using TheBugTracker.Data;
using TheBugTracker.Models;
using TheBugTracker.Models.Enums;
using TheBugTracker.Services.Interfaces;

namespace TheBugTracker.Services
{
    public class BTProjectService : IBTProjectService
    {
		private readonly ApplicationDbContext _context;
        private readonly IBTRolesService _rolesService;

		public BTProjectService(ApplicationDbContext context, IBTRolesService rolesService)
		{
			_context = context;
			_rolesService = rolesService;
		}
		public async Task AddMembersToProjectAsync(IEnumerable<string>? userIds, int? projectId, int? companyId)
        {
            try
            {
                if (userIds != null)
                {
                    Project? project = await GetProjectByIdAsync(projectId, companyId);
                    foreach (string userId in userIds)
                    {
                        BTUser? bTUser = await _context.Users.FindAsync(userId);
                        if (project != null && bTUser != null)
                        {
                            bool IsOnProject = project.Members.Any(m=>m.Id == userId);
                            if(!IsOnProject)
                            {
                                project.Members.Add(bTUser);
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> AddMemberToProjectAsync(BTUser? member, int? projectId)
        {
            try
            {
                if (member != null && projectId != null)
                {
                    Project? project = await GetProjectByIdAsync(projectId, member.CompanyId);
                    if (project != null)
                    {
                        //proj instance must inlcude members to do following
                        bool IsOnProject = project.Members.Any(m => m.Id == member.Id);

                        if(!IsOnProject)
                        {
                            project.Members.Add(member);
                            await _context.SaveChangesAsync();
                            return true;
                        }
                    }
                } return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task AddProjectAsync(Project project)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddProjectManagerAsync(string? userId, int? projectId)
        {
            try
            {
                if (userId != null && projectId != null)
                {
                    BTUser? currentPM = await GetProjectManagerAsync(projectId);
                    BTUser? selectedPM = await _context.Users.FindAsync(userId);
                  
                    //remove current pm
                    if (currentPM != null)
                    {
                        await RemoveProjectManagerAsync(projectId);
                    }
                    //add new PM
                    try
                    {
                        await AddMemberToProjectAsync(selectedPM, projectId);
                        return true;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task ArchiveProjectAsync(Project? project, int? companyId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Project>> GetAllProjectsByCompanyIdAsync(int? companyId)
        {
            try
            {
                List<Project> projects = await _context.Projects.Where(p=>p.CompanyId == companyId).Include(p=>p.Members)
                    .Include(p=>p.ProjectPriority)
                    .Include(p=>p.Tickets)
                    .ToListAsync();
                return projects;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<List<Project>> GetArchivedProjectsByCompanyIdAsync(int? companyId)
        {
            throw new NotImplementedException();
        }

        public async Task<Project> GetProjectByIdAsync(int? projectId, int? companyId)
        {
            try
            {
                Project? project = new();
                if (projectId != null && companyId != null)
                {
                    project = await _context.Projects
                        .Include(p=>p.ProjectPriority)
                        .Include(p => p.Company)
                        .Include(p => p.Members)
                        .Include(p => p.Tickets)
                        .ThenInclude(t=>t.Comments)
                        .ThenInclude(c=>c.User)
                        .Include(p=>p.Tickets)
                        .ThenInclude(t=>t.Attachments)
                        .Include(p=>p.Tickets)
                        .ThenInclude(t =>t.History)
                        .FirstOrDefaultAsync(p=>p.Id == projectId && p.CompanyId == companyId);
                }
                return project!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<BTUser> GetProjectManagerAsync(int? projectId)
        {
            try
            {
                Project? project = await _context.Projects.Include(p=>p.Members).FirstOrDefaultAsync(p=>p.Id == projectId);
                if(project != null)
                    foreach (BTUser member in project.Members)
                    {
                        if  (await _rolesService.IsUserInRoleAsync(member, nameof(BTRoles.ProjectManager)))
                        {
                            return member;
                        }
                    }
                return null!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<BTUser>> GetProjectMembersByRoleAsync(int? projectId, string? roleName, int? companyId)
        {
            try
            {
                List<BTUser> members = new();
                if (projectId != null && companyId != null && !string.IsNullOrEmpty(roleName))
                {
                    Project? project = await GetProjectByIdAsync(projectId, companyId);
                    if (project != null)
                    {
                        foreach (BTUser member in project.Members)
                        {
                            if (await _rolesService.IsUserInRoleAsync(member, roleName))
                            {
                                members.Add(member);
                            }
                        }
                        //for testing purposes
                        List<string> developers = (await _rolesService.GetUsersInRoleAsync(roleName, companyId))?.Select(u => u.Id).ToList()!;
                        List<BTUser> users = project.Members.Where(m => developers.Contains(m.Id)).ToList();
                    }
                }
                return members;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<ProjectPriority>> GetProjectPrioritiesAsync()
        {
            return await _context.ProjectPriorities.ToListAsync();
        }

        public async Task<List<Project>> GetAllProjectsByPriorityAsync(int? companyId, string? priority)
        { try
            {
                List<Project> projects = await _context.Projects.Where(p => p.CompanyId == companyId && p.ProjectPriority!.Name == priority).Include(p => p.Members)
                    .Include(p => p.ProjectPriority)
                    .Include(p => p.Tickets)
                    .ToListAsync();
                return projects;
            }
            catch (Exception)
            {

                throw;
            }
        }

public async Task<ContentResult> GetProjectStatusBadgeAsync(int? projectId)
        {
            if (projectId == null)
                return null;

            int projectPriorityId = (await _context.Projects.Where(p => p.Id == projectId).FirstOrDefaultAsync()).ProjectPriorityId;

            string html = "";

            switch(projectPriorityId)
            {
                case 0:
                    html = "<span class='badge bg-success-bright text-success'>Low</span>";

                    break;
                case 1:
                    html = "<span class='badge bg-warning-bright text-warning'>Medium</span>";

                    break;
                case 2:
                    html = "<span class='badge bg-secondary-bright text-secondary'>High</span>";

                    break;
                case 3:
                    html = "<span class='badge bg-danger-bright text-danger'>Urgent</span>";
                    break;
                default:
                    html = "<span class='badge bg-danger-bright text-danger'>Not Found</span>";
                    break;
            }

            return new ContentResult
            {
                Content = html,
                ContentType = "text/html"
            };

        }

        public Task<List<Project>?> GetUserProjectsAsync(string? userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveMemberFromProjectAsync(BTUser? member, int? projectId)
        {
            try
            {
                if (member != null && projectId != null)
                {
                    Project? project = await GetProjectByIdAsync(projectId, member.CompanyId);
                    if (project != null)
                    {

                        bool IsOnProject = project.Members.Any(m => m.Id == member.Id);
                        if (IsOnProject)
                        {
                            project.Members.Remove(member);
                            await _context.SaveChangesAsync();
                            return true;
                        }
                    }
                } return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task RemoveMembersFromProjectAsync(int? projectId, int? companyId)
        {
            try
            {
                Project? project = await GetProjectByIdAsync(projectId, companyId);

                if (project != null)
                {
                    foreach (var member in project.Members)
                    {
                        if (!await _rolesService.IsUserInRoleAsync(member, nameof(BTRoles.ProjectManager)))
                        {
                            project.Members.Remove(member);
                        }
                    }
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task RemoveProjectManagerAsync(int? projectId)
        {
            try
            {
                if (projectId != null)
                {
                    Project? project = await _context.Projects.Include(p => p.Members).FirstOrDefaultAsync(p => p.Id == projectId);
                   
                    if (project != null)
                    {
                        foreach (BTUser member in project!.Members)
                        {
                            if (await _rolesService.IsUserInRoleAsync(member, nameof(BTRoles.ProjectManager)))
                            {
                                //remove BTUser from project
                                await RemoveMemberFromProjectAsync(member, projectId);
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task RestoreProjectAsync(Project? project, int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProjectAsync(Project? project)
        {
            throw new NotImplementedException();
        }
    }
}
