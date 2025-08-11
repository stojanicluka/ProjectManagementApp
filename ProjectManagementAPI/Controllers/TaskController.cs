using Microsoft.AspNetCore.Mvc;
using ProjectManagementAPI.DTO;
using ProjectManagementAPI.Services;
using ProjectManagementAPI.Services.Exceptions;

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
                List<APIResponse.Error> errors = new List<APIResponse.Error> { new APIResponse.Error { Type = ex.Type, Message = ex.Message } };
                return NotFound(new APIResponse(false, errors, null));
            }

        }

        [HttpPatch]
        [Route("{taskId}")]
        public async Task<IActionResult> UpdateTaskAsync(int projectId, int taskId, PatchTaskDTO dto)
        {
            try
            {
                await _taskService.UpdateTaskAsync(projectId, taskId, dto);
            }
            catch (APIException ex)
            {
                List<APIResponse.Error> errors = new List<APIResponse.Error> { new APIResponse.Error { Type = ex.Type, Message = ex.Message } };
                return NotFound(new APIResponse(false, errors, null));
            }
        }



    }
}
