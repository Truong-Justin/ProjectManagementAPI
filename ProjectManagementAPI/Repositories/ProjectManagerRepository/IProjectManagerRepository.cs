using ProjectManagementAPI.Models.People;
using ProjectManagementAPI.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjectManagementAPI.Repositories.ProjectManagerRepository
{
    // Interface defines a contract that requires
    // the ProjectManagerRepository class to implement all
    // declared methods defined here
    public interface IProjectManagerRepository
	{
		Task<IEnumerable<ProjectManager>> GetAllProjectManagersAsync();
		IEnumerable<SelectListItem> GetProjectManagerNames(IEnumerable<ProjectManager> projectManagers);
		Task<ProjectManager> GetProjectManagerByIdAsync(int id);
		Task<IEnumerable<Project>> GetAllProjectsForManagerAsync(int projectManagerId);
		Task AddProjectManagerAsync(ProjectManager projectManager);
		Task UpdateProjectManagerAsync(int projectManagerId, string phone, string zip, string address);
		Task DeleteProjectManagerAsync(int id);
	}
}

