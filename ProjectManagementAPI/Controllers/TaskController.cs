using Microsoft.AspNetCore.Mvc;
using ProjectManagementAPI.DTO;
using ProjectManagementAPI.Services;
using ProjectManagementAPI.Services.Exceptions;
using System.Threading.Tasks;

namespace ProjectManagementAPI.Controllers
{
    [ApiController]
    [Route("projects/{projectId}/tasks")]
    public class TaskController : ControllerBase
    {
        private TaskService _taskService;

        public TaskController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaskAsync(int projectId, CreateTaskDTO dto)
        {
            try
            {
                await _taskService.CreateTaskAsync(projectId, dto);
                return Ok(new APIResponse());
            }
            catch (APIException ex)
            {
                List<APIResponse.Error> errors = new List<APIResponse.Error> { new APIResponse.Error { Type = ex.getType(), Message = ex.Message } };
                return BadRequest(new APIResponse(false, errors, null));
            }

        }

        [HttpPatch]
        [Route("{taskId}")]
        public async Task<IActionResult> UpdateTaskAsync(int projectId, int taskId, PatchTaskDTO dto)
        {
            try
            {
                await _taskService.UpdateTaskAsync(projectId, taskId, dto);
                return Ok(new APIResponse());
            }
            catch (APIException ex)
            {
                List<APIResponse.Error> errors = new List<APIResponse.Error> { new APIResponse.Error { Type = ex.getType(), Message = ex.Message } };
                return BadRequest(new APIResponse(false, errors, null));
            }
        }

        [HttpDelete]
        [Route("{taskId}")]
        public async Task<IActionResult> DeleteTaskAsync(int projectId, int taskId)
        {
            try
            {
                await _taskService.DeleteTaskAsync(projectId, taskId);
                return Ok(new APIResponse());
            }
            catch (APIException ex)
            {
                List<APIResponse.Error> errors = new List<APIResponse.Error> { new APIResponse.Error { Type = ex.getType(), Message = ex.Message } };
                return BadRequest(new APIResponse(false, errors, null));
            }
        }

        [HttpGet]
        [Route("{taskId}")]
        public async Task<IActionResult> GetTaskAsync(int projectId, int taskId)
        {
            try
            {
                return Ok(new APIResponse(true, new List<APIResponse.Error>(), await _taskService.GetTaskAsync(projectId, taskId)));
            }
            catch (APIException ex)
            {
                List<APIResponse.Error> errors = new List<APIResponse.Error> { new APIResponse.Error { Type = ex.getType(), Message = ex.Message } };
                return BadRequest(new APIResponse(false, errors, null));
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAllProjectTasksAsync(int projectId)
        {
            try
            {
                return Ok(new APIResponse(true, new List<APIResponse.Error>(), await _taskService.GetAllProjectTasksAsync(projectId)));
            }
            catch (APIException ex)
            {
                List<APIResponse.Error> errors = new List<APIResponse.Error> { new APIResponse.Error { Type = ex.getType(), Message = ex.Message } };
                return BadRequest(new APIResponse(false, errors, null));
            }
        }



    }
}
