using TheBugTracker.Models;
using TheBugTracker.Services.Interfaces;

namespace TheBugTracker.Services
{
    public class BTProjectService : IBTProjectService
    {
        public Task AddMembersToProjectAsync(IEnumerable<string>? userIds, int? projectId, int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddMemberToProjectAsync(BTUser? member, int? projectId)
        {
            throw new NotImplementedException();
        }

        public Task AddProjectAsync(Project project)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddProjectManagerAsync(string? userId, int? projectId)
        {
            throw new NotImplementedException();
        }

        public Task ArchiveProjectAsync(Project? project, int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Project>> GetAllProjectsByCompanyIdAsync(int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Project>> GetArchivedProjectsByCompanyIdAsync(int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task<Project> GetProjectByIdAsync(int? projectId, int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task<BTUser> GetProjectManagerAsync(int? projectId)
        {
            throw new NotImplementedException();
        }

        public Task<List<BTUser>> GetProjectMembersByRoleAsync(int? projectId, string? roleName, int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProjectPriority>> GetProjectPrioritiesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Project>?> GetUserProjectsAsync(string? userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveMemberFromProjectAsync(BTUser? member, int? projectId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveMembersFromProjectAsync(int? projectId, int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveProjectManagerAsync(int? projectId)
        {
            throw new NotImplementedException();
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
