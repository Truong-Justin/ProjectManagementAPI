using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectManagementAPI.Models;
using ProjectManagementAPI.Models.People;
using ProjectManagementAPI.Repositories.ProjectManagerRepository;


namespace ProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    public class ProjectManagerController : ControllerBase
    {
        private readonly IProjectManagerRepository _projectManagerRepository;


        // IProjectManager service is dependency-injected
        // into class constructor so the entire ProjectManagerController
        // class can implement the methods defined by the supplied
        // IProjectManager interface
        public ProjectManagerController(IProjectManagerRepository projectManagerRepository)
        {
            _projectManagerRepository = projectManagerRepository;
        }


        [Route("GetAllProjectManagers")]
        [HttpGet]
        public async Task<IEnumerable<ProjectManager>> GetAllProjectManagers()
        {
            return await _projectManagerRepository.GetAllProjectManagersAsync();
        }


        [Route("GetProjectManagerById")]
        [HttpGet]
        public async Task<ActionResult<ProjectManager>> GetProjectManager(int id)
        {
            ProjectManager projectManager = await _projectManagerRepository.GetProjectManagerByIdAsync(id);
            if (projectManager.ProjectManagerId == 0)
            {
                return NotFound();
            }

            return projectManager;
        }


        [Route("GetAllProjectsForManager")]
        [HttpGet]
        public async Task<IEnumerable<Project>> GetAllProjectsForManager(int projectManagerId)
        {
            return await _projectManagerRepository.GetAllProjectsForManagerAsync(projectManagerId);
        }


        [Route("GetProjectManagerNames")]
        [HttpGet]
        public async Task<IEnumerable<SelectListItem>> GetProjectManagerNames()
        {
            IEnumerable<ProjectManager> projectManagers = await _projectManagerRepository.GetAllProjectManagersAsync();
            return _projectManagerRepository.GetProjectManagerNames(projectManagers);
        }


        [Route("AddProjectManager")]
        [HttpPost]
        public async Task<ActionResult<ProjectManager>> AddProjectManager(ProjectManager projectManager)
        {
            await _projectManagerRepository.AddProjectManagerAsync(projectManager);
            return Ok();
        }


        [Route("UpdateProjectManager")]
        [HttpPut]
        public async Task<ActionResult<ProjectManager>> UpdateProjectManager(int projectManagerId, string firstName, string lastName, DateOnly hiredate, string phone, string zip, string address)
        {
            ProjectManager projectManager = await _projectManagerRepository.GetProjectManagerByIdAsync(projectManagerId);
            if (projectManager.ProjectManagerId == 0)
            {
                return NotFound();
            }

            await _projectManagerRepository.UpdateProjectManagerAsync(projectManagerId, firstName, lastName, hiredate, phone, zip, address);
            return Ok();
        }


        [Route("DeleteProjectManager")]
        [HttpDelete]
        public async Task<ActionResult<ProjectManager>> DeleteProjectManager(int id)
        {
            ProjectManager projectManager = await _projectManagerRepository.GetProjectManagerByIdAsync(id);
            if (projectManager.ProjectManagerId == 0)
            {
                return NotFound();
            }

            await _projectManagerRepository.DeleteProjectManagerAsync(id);
            return Ok();
        }
    }
}

