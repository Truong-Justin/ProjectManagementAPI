using ProjectManagementAPI.Repositories.BugRepository;
using ProjectManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;


namespace ProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    public class BugsController : ControllerBase
    {
        private readonly IBugRepository _bugRepository;


        // IBugRepository service is dependency-injected
        // into class constructor so the entire BugsController
        // class can implement the methods defined by the supplied
        // IBugRepository interface
        public BugsController(IBugRepository bugRepository)
        {
            _bugRepository = bugRepository;
        }


        [Route("GetAllBugs")]
        [HttpGet]
        public async Task<ActionResult> GetAllBugs()
        {
            IEnumerable<Bug> bugs = await _bugRepository.GetAllBugsAsync();
            return Ok(bugs);
        }


        [Route("GetBugById")]
        [HttpGet]
        public async Task<ActionResult> GetBugById(int bugId)
        {
            Bug bug = await _bugRepository.GetBugByIdAsync(bugId);
            if (bug.BugId == 0)
            {
                return NotFound("A bug with the given ID doesn't exist.");
            }

            return Ok(bug);
        }


        [Route("AddBug")]
        [HttpPost]
        public async Task<ActionResult> AddBug(Bug bug)
        {
            await _bugRepository.AddBugAsync(bug);
            return Ok("A new bug has been added.");
        }


        [Route("UpdateBug")]
        [HttpPut]
        public async Task<ActionResult> UpdateBug(int bugId, string description, string priority, string assignment)
        {
            Bug bug = await _bugRepository.GetBugByIdAsync(bugId);
            if (bug.BugId == 0)
            {
                return NotFound("A bug with the given ID doesn't exist.");
            }

            await _bugRepository.UpdateBugAsync(bugId, description, priority, assignment);
            return Ok("The bug has been updated.");
        }


        [Route("DeleteBug")]
        [HttpDelete]
        public async Task<ActionResult> DeleteBug(int bugId)
        {
            Bug bug = await _bugRepository.GetBugByIdAsync(bugId);
            if (bug.BugId == 0)
            {
                return NotFound("A bug with the given ID doesn't exist.");
            }

            await _bugRepository.DeleteBugAsync(bugId);
            return Ok("The bug has been deleted.");
        }
    }
}

