using TheBugTracker.Models;
using TheBugTracker.Services.Interfaces;

namespace TheBugTracker.Services
{
    public class BTCompanyService : IBTCompanyService
    {
        public Task<Company> GetCompanyInfoAsync(int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Invite>> GetInvitesAsync(int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<BTUser>> GetMembersAsync(int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Project>> GetProjectsAsync(int? companyId)
        {
            throw new NotImplementedException();
        }
    }
}
