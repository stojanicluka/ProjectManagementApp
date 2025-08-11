using Microsoft.AspNetCore.Mvc;
using ProjectManagementAPI.DTO;
using ProjectManagementAPI.Services;
using ProjectManagementAPI.Services.Exceptions;
using System.Threading.Tasks;

namespace ProjectManagementAPI.Controllers
{
    [ApiController]
    [Route("tasks")]
    public class TaskController : ControllerBase
    {
        private TaskService _taskService;

        public TaskController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaskAsync(CreateTaskDTO dto)
        {
            try
            {
                await _taskService.CreateTaskAsync(dto);
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
        public async Task<IActionResult> UpdateTaskAsync(int taskId, PatchDTO dto)
        {
            try
            {
                await _taskService.UpdateTaskAsync(taskId, dto);
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
        public async Task<IActionResult> DeleteTaskAsync(int taskId)
        {
            try
            {
                await _taskService.DeleteTaskAsync(taskId);
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
        public async Task<IActionResult> GetTaskAsync(int taskId)
        {
            try
            {
                return Ok(new APIResponse(true, new List<APIResponse.Error>(), await _taskService.GetTaskAsync(taskId)));
            }
            catch (APIException ex)
            {
                List<APIResponse.Error> errors = new List<APIResponse.Error> { new APIResponse.Error { Type = ex.getType(), Message = ex.Message } };
                return BadRequest(new APIResponse(false, errors, null));
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAllProjectTasksAsync([FromQuery] int? projectId = null, [FromQuery] String? userId = null)
        {
            try
            {
                return Ok(new APIResponse(true, new List<APIResponse.Error>(), await _taskService.GetAllProjectUserTasksAsync(projectId, userId)));
            }
            catch (APIException ex)
            {
                List<APIResponse.Error> errors = new List<APIResponse.Error> { new APIResponse.Error { Type = ex.getType(), Message = ex.Message } };
                return BadRequest(new APIResponse(false, errors, null));
            }
        }



    }
}
