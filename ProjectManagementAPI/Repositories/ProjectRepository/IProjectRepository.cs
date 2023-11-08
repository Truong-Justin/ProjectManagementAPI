using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectManagementAPI.Models;
using ProjectManagementAPI.Models.People;

namespace ProjectManagementAPI.Repositories
{
    // Interface defines a contract that requires
    // the ProjectRepository class to implement all
    // declared methods defined here
    public interface IProjectRepository
	{
		Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<IEnumerable<Bug>> GetAllBugsForProjectAsync(int projectId);
        Task<IEnumerable<Employee>> GetAllEmployeesForProjectAsync(int projectId);
        IEnumerable<SelectListItem> GetProjectTitles(IEnumerable<Project> projects);
        Task<Project> GetProjectByIdAsync(int id);
        Task AddProjectAsync(Project project);
		Task UpdateProjectAsync(int projectId, string projectTitle, string description, string priority, int projectManagerId);
		Task DeleteProjectAsync(int id);
    }
}

