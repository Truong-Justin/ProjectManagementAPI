using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectManagementAPI.Models;

namespace ProjectManagementAPI.Repositories.BugRepository
{
	public interface IBugRepository
	{
        Task<IEnumerable<Bug>> GetAllBugsAsync();
        Task<Bug> GetBugByIdAsync(int bugId);
        Task AddBugAsync(Bug bug);
        Task UpdateBugAsync(int bugId, DateOnly date, string description, string priority, string assignment);
        Task DeleteBugAsync(int bugId);
    }
}

