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
        public async Task<ActionResult> GetAllProjectManagers()
        {
            IEnumerable<ProjectManager> projectManagers = await _projectManagerRepository.GetAllProjectManagersAsync();
            return Ok(projectManagers);
        }


        [Route("GetProjectManagerById")]
        [HttpGet]
        public async Task<ActionResult> GetProjectManager(int projectManagerId)
        {
            ProjectManager projectManager = await _projectManagerRepository.GetProjectManagerByIdAsync(projectManagerId);
            if (projectManager.ProjectManagerId == 0)
            {
                return NotFound("A project manager with the given ID doesn't exist.");
            }

            return Ok(projectManager);
        }


        [Route("GetAllProjectsForManager")]
        [HttpGet]
        public async Task<ActionResult> GetAllProjectsForManager(int projectManagerId)
        {
            ProjectManager projectManager = await _projectManagerRepository.GetProjectManagerByIdAsync(projectManagerId);
            if (projectManager.ProjectManagerId == 0)
            {
                return NotFound("A project manager with the given ID doesn't exist.");
            }

            IEnumerable<Project> projectsForManager = await _projectManagerRepository.GetAllProjectsForManagerAsync(projectManagerId);
            return Ok(projectsForManager);
        }


        [Route("GetProjectManagerNames")]
        [HttpGet]
        public async Task<ActionResult> GetProjectManagerNames()
        {
            IEnumerable<ProjectManager> projectManagers = await _projectManagerRepository.GetAllProjectManagersAsync();
            IEnumerable<SelectListItem> projectManagerNames = _projectManagerRepository.GetProjectManagerNames(projectManagers);
            return Ok(projectManagerNames);
        }


        [Route("AddProjectManager")]
        [HttpPost]
        public async Task<ActionResult> AddProjectManager(ProjectManager projectManager)
        {
            await _projectManagerRepository.AddProjectManagerAsync(projectManager);
            return Ok("A new project manager has been added.");
        }


        [Route("UpdateProjectManager")]
        [HttpPut]
        public async Task<ActionResult> UpdateProjectManager(int projectManagerId, string phone, string zip, string address)
        {
            ProjectManager projectManager = await _projectManagerRepository.GetProjectManagerByIdAsync(projectManagerId);
            if (projectManager.ProjectManagerId == 0)
            {
                return NotFound("A project manager with the given ID doesn't exist.");
            }

            await _projectManagerRepository.UpdateProjectManagerAsync(projectManagerId, phone, zip, address);
            return Ok("The project manager has been updated.");
        }


        [Route("DeleteProjectManager")]
        [HttpDelete]
        public async Task<ActionResult> DeleteProjectManager(int projectManagerId)
        {
            ProjectManager projectManager = await _projectManagerRepository.GetProjectManagerByIdAsync(projectManagerId);
            if (projectManager.ProjectManagerId == 0)
            {
                return NotFound("A project manager with the given ID doesn't exist.");
            }

            await _projectManagerRepository.DeleteProjectManagerAsync(projectManagerId);
            return Ok("The project manager has been deleted.");
        }
    }
}

