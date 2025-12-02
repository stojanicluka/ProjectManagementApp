using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementAPI.DTO;
using ProjectManagementAPI.Models;
using ProjectManagementAPI.Services;
using ProjectManagementAPI.Services.Exceptions;

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

        [Authorize(Roles = "ADMIN,MANAGER")]
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDTO dto)
        {
            try
            {
                return Ok(new APIResponse(await _service.CreateAsync(dto)));
            }
            catch (APIException ex)
            {
                List<APIResponse.Error> errors = new List<APIResponse.Error> { new APIResponse.Error { Type = ex.getType(), Message = ex.Message } };
                return BadRequest(new APIResponse(false, errors, null));
            }
        }

        [Authorize(Roles = "ADMIN,MANAGER,TEAM_MEMBER")]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> FetchProject(int id)
        {
            try
            {
                return Ok(new APIResponse(await _service.FetchAsync(User.Identity.Name, id)));
            }
            catch (APIException ex)
            {
                List<APIResponse.Error> errors = new List<APIResponse.Error> { new APIResponse.Error { Type = ex.getType(), Message = ex.Message } };
                return BadRequest(new APIResponse(false, errors, null));
            }
        }

        [Authorize(Roles = "ADMIN,MANAGER,TEAM_MEMBER")]
        [HttpGet]
        public async Task<IActionResult> FetchAllProjects()
        {
            try
            {
                return Ok(new APIResponse(await _service.FetchAllAsync(User.Identity.Name)));
            }
            catch (APIException ex)
            {
                List<APIResponse.Error> errors = new List<APIResponse.Error> { new APIResponse.Error { Type = ex.getType(), Message = ex.Message } };
                return BadRequest(new APIResponse(false, errors, null));
            }
        }

        [Authorize(Roles = "ADMIN,MANAGER")]
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] PatchDTO dto)
        {
            try
            {
                await _service.UpdateAsync(id, dto);
                return Ok(new APIResponse());
            }
            catch (APIException ex)
            {
                List<APIResponse.Error> errors = new List<APIResponse.Error> { new APIResponse.Error { Type = ex.getType(), Message = ex.Message } };
                return BadRequest(new APIResponse(false, errors, null));
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(new APIResponse());
            }
            catch (APIException ex)
            {
                List<APIResponse.Error> errors = new List<APIResponse.Error> { new APIResponse.Error { Type = ex.getType(), Message = ex.Message } };
                return BadRequest(new APIResponse(false, errors, null));
            }
        }
    }
}
