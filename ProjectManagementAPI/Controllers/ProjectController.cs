using Microsoft.AspNetCore.Mvc;
using ProjectManagementAPI.DTO;
using ProjectManagementAPI.Models;
using ProjectManagementAPI.Services;

namespace ProjectManagementAPI.Controllers
{
    [ApiController]
    [Route("projects")]
    public class ProjectController : ControllerBase
    {
        private ProjectService _service;
        public ProjectController(ProjectService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] ProjectDTO pDTO)
        {
            return Ok(await _service.CreateAsync(pDTO));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> FetchProject(int id)
        {
            return Ok(await _service.FetchAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> FetchAllProjects()
        {
            return Ok(await _service.FetchAllAsync());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] ProjectDTO pDTO)
        {
            return Ok(await _service.UpdateAsync(id, pDTO));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            return Ok(await _service.DeleteAsync(id));
        }
    }
}
