using ProjectManagementAPI.Repositories;
using ProjectManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace ProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectsController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }


        [Route("GetAllProjects")]
        [HttpGet]
        public async Task<IEnumerable<Project>> GetAllProjects()
        {
            return await _projectRepository.GetAllProjectsAsync();
        }


        [Route("GetProjectById")]
        [HttpGet]
        public async Task<ActionResult<Project>> GetProjectById(int projectId)
        {
            Project project = await _projectRepository.GetProjectByIdAsync(projectId);
            if (project.ProjectId == 0)
            {
                return NotFound();
            }

            return project;
        }


        [Route("GetProjectTitles")]
        [HttpGet]
        public async Task<IEnumerable<SelectListItem>> GetProjectTitles()
        {
            IEnumerable<Project> projects = await _projectRepository.GetAllProjectsAsync();
            return _projectRepository.GetProjectTitles(projects);
        }


        [Route("AddProject")]
        [HttpPost]
        public async Task<ActionResult<Project>> AddProject(Project project)
        {
            await _projectRepository.AddProjectAsync(project);
            return Ok();
        }


        [Route("UpdateProject")]
        [HttpPut]
        public async Task<ActionResult<Project>> UpdateProject(int projectId, DateOnly date, string projectTitle, string description, string priority, int projectManagerId)
        {
            Project project = await _projectRepository.GetProjectByIdAsync(projectId);
            if (project.ProjectId == 0)
            {
                return NotFound();
            }

            await _projectRepository.UpdateProjectAsync(projectId, date, projectTitle, description, priority, projectManagerId);
            return Ok();
        }


        [Route("DeleteProject")]
        [HttpDelete]
        public async Task<ActionResult<Project>> DeleteProjectAsync(int id)
        {
            Project project = await _projectRepository.GetProjectByIdAsync(id);
            if (project.ProjectId == 0)
            {
                return NotFound();
            }

            await _projectRepository.DeleteProjectAsync(id);
            return Ok();
        }
    }
}

    


