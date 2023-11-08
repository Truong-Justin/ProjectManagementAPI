using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectManagementAPI.Models;

namespace ProjectManagementAPI.Repositories.BugRepository
{
    // Interface defines a contract that requires
    // the BugRepository class to implement all
    // declared methods defined here
	public interface IBugRepository
	{
        Task<IEnumerable<Bug>> GetAllBugsAsync();
        Task<Bug> GetBugByIdAsync(int bugId);
        Task AddBugAsync(Bug bug);
        Task UpdateBugAsync(int bugId, string description, string priority, string assignment);
        Task DeleteBugAsync(int bugId);
    }
}

