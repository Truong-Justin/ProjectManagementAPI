using ProjectManagementAPI.Models.People;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjectManagementAPI.Repositories.ProjectManagerRepository
{
	public interface IProjectManagerRepository
	{
		Task<IEnumerable<ProjectManager>> GetAllProjectManagersAsync();
		IEnumerable<SelectListItem> GetProjectManagerNames(IEnumerable<ProjectManager> projectManagers);
		Task<ProjectManager> GetProjectManagerByIdAsync(int id);
		Task AddProjectManagerAsync(ProjectManager projectManager);
		Task UpdateProjectManagerAsync(int projectManagerId, string firstName, string lastName, DateOnly hiredate, string phone, string zip, string address);
		Task DeleteProjectManagerAsync(int id);
	}
}

