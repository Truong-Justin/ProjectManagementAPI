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
        public async Task<IEnumerable<Bug>> GetAllBugs()
        {
            return await _bugRepository.GetAllBugsAsync();
        }


        [Route("GetBugById")]
        [HttpGet]
        public async Task<ActionResult<Bug>> GetBugById(int id)
        {
            Bug bug = await _bugRepository.GetBugByIdAsync(id);
            if (bug.BugId == 0)
            {
                return NotFound();
            }

            return bug;
        }


        [Route("AddBug")]
        [HttpPost]
        public async Task<ActionResult<Bug>> AddBug(Bug bug)
        {
            await _bugRepository.AddBugAsync(bug);
            return Ok();
        }

        [Route("UpdateBug")]
        [HttpPut]
        public async Task<ActionResult<Bug>> UpdateBug(int id, DateOnly date, string description, string priority, string assignment)
        {
            Bug bug = await _bugRepository.GetBugByIdAsync(id);
            if (bug.BugId == 0)
            {
                return NotFound();
            }

            await _bugRepository.UpdateBugAsync(id, date, description, priority, assignment);
            return Ok();
        }


        [Route("DeleteBug")]
        [HttpDelete]
        public async Task<ActionResult<Bug>> DeleteBug(int id)
        {
            Bug bug = await _bugRepository.GetBugByIdAsync(id);
            if (bug.BugId == 0)
            {
                return NotFound();
            }

            await _bugRepository.DeleteBugAsync(id);
            return Ok();
        }
    }
}

