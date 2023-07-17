using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectManagementAPI.Models;

namespace ProjectManagementAPI.Repositories
{
	public interface IProjectRepository
	{
		Task<IEnumerable<Project>> GetAllProjectsAsync();
        IEnumerable<SelectListItem> GetProjectTitles(IEnumerable<Project> projects);
        Task<Project> GetProjectByIdAsync(int id);
        Task AddProjectAsync(Project project);
		Task UpdateProjectAsync(int projectId, DateOnly date, string projectTitle, string description, string priority, int projectManagerId);
		Task DeleteProjectAsync(int id);
    }
}

