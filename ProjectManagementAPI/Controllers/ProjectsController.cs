using ProjectManagementAPI.Repositories;
using ProjectManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        public async Task<ActionResult<Project>> GetProjectById(int id)
        {
            Project project = await _projectRepository.GetProjectByIdAsync(id);
            if (project.ProjectId == 0)
            {
                return NotFound();
            }

            return project;
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
        public async Task<ActionResult<Project>> UpdateProject(Project project, int id)
        {
            project = await _projectRepository.GetProjectByIdAsync(id);
            if (project.ProjectId == 0)
            {
                return NotFound();
            }

            await _projectRepository.UpdateProjectAsync(project);
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

    


