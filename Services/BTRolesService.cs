using TheBugTracker.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using TheBugTracker.Models;

namespace TheBugTracker.Services
{
    public class BTRolesService : IBTRolesService
    {
        public Task<bool> AddUserToRoleAsync(BTUser? user, string? roleName)
        {
            throw new NotImplementedException();
        }

        public Task<List<IdentityRole>> GetRolesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>?> GetUserRolesAsync(BTUser? user)
        {
            throw new NotImplementedException();
        }

        public Task<List<BTUser>> GetUsersInRoleAsync(string? roleName, int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUserInRoleAsync(BTUser? member, string? roleName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveUserFromRoleAsync(BTUser? user, string? roleName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveUserFromRolesAsync(BTUser? user, IEnumerable<string>? roleNames)
        {
            throw new NotImplementedException();
        }
    }
}
