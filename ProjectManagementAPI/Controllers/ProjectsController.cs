using ProjectManagementAPI.Repositories;
using ProjectManagementAPI.Models;
using ProjectManagementAPI.Models.People;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace ProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;


        // IProjectsRepository service is dependency-injected
        // into class constructor so the entire ProjectsController
        // class can implement the methods defined by the supplied
        // IProjectRepository interface
        public ProjectsController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }


        [Route("GetAllProjects")]
        [HttpGet]
        public async Task<ActionResult> GetAllProjects()
        {
            IEnumerable<Project> projects = await _projectRepository.GetAllProjectsAsync();
            return Ok(projects);
        }


        [Route("GetProjectById")]
        [HttpGet]
        public async Task<ActionResult> GetProjectById(int projectId)
        {
            Project project = await _projectRepository.GetProjectByIdAsync(projectId);
            if (project.ProjectId == 0)
            {
                return NotFound("A project with the given ID doesn't exist.");
            }

            return Ok(project);
        }


        [Route("GetAllBugsForProject")]
        [HttpGet]
        public async Task<ActionResult> GetAllBugsForProject(int projectId)
        {
            Project project = await _projectRepository.GetProjectByIdAsync(projectId);
            if (project.ProjectId == 0)
            {
                return NotFound("A project with the given ID doesn't exist.");
            }

            IEnumerable<Bug> bugsForProject = await _projectRepository.GetAllBugsForProjectAsync(projectId);
            return Ok(bugsForProject);
        
        }


        [Route("GetAllEmployeesForProject")]
        [HttpGet]
        public async Task<ActionResult> GetAllEmployeesForProject(int projectId)
        {
            Project project = await _projectRepository.GetProjectByIdAsync(projectId);
            if (project.ProjectId == 0)
            {
                return NotFound("A project with the given ID doesn't exist.");
            }

            IEnumerable<Employee> employeesForProject = await _projectRepository.GetAllEmployeesForProjectAsync(projectId);
            return Ok(employeesForProject);
        }


        [Route("GetProjectTitles")]
        [HttpGet]
        public async Task<ActionResult> GetProjectTitles()
        {
            IEnumerable<Project> projects = await _projectRepository.GetAllProjectsAsync();
            IEnumerable<SelectListItem> projectTitles = _projectRepository.GetProjectTitles(projects);
            return Ok(projectTitles);
        }


        [Route("AddProject")]
        [HttpPost]
        public async Task<ActionResult> AddProject(Project project)
        {
            await _projectRepository.AddProjectAsync(project);
            return Ok("A new project has been added.");
        }


        [Route("UpdateProject")]
        [HttpPut]
        public async Task<ActionResult> UpdateProject(int projectId, string projectTitle, string description, string priority, int projectManagerId)
        {
            Project project = await _projectRepository.GetProjectByIdAsync(projectId);
            if (project.ProjectId == 0)
            {
                return NotFound("A project with the given ID doesn't exist.");
            }

            await _projectRepository.UpdateProjectAsync(projectId, projectTitle, description, priority, projectManagerId);
            return Ok("The project has been updated.");
        }


        [Route("DeleteProject")]
        [HttpDelete]
        public async Task<ActionResult> DeleteProjectAsync(int projectId)
        {
            Project project = await _projectRepository.GetProjectByIdAsync(projectId);
            if (project.ProjectId == 0)
            {
                return NotFound("A project with the given ID doesn't exist.");
            }

            await _projectRepository.DeleteProjectAsync(projectId);
            return Ok("The project has been deleted.");
        }
    }
}

    
 

