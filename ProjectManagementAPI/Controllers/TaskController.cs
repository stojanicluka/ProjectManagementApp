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
            catch (ProjectNotFoundException ex)
            {
                List<APIResponse.Error> errors = new List<APIResponse.Error> { new APIResponse.Error { Type = "ProjectNotFound", Message = "Project not found" } };
                return NotFound(new APIResponse(false, errors, null));
            }
            catch (UserNotFoundException ex)
            {
                List<APIResponse.Error> errors = new List<APIResponse.Error> { new APIResponse.Error { Type = "UserNotFound", Message = "User not found" } };
                return NotFound(new APIResponse(false, errors, null));
            }
            catch (DatabaseException ex)
            {
                List<APIResponse.Error> errors = new List<APIResponse.Error> { new APIResponse.Error { Type = "DatabaseError", Message = "Database error" } };
                return NotFound(new APIResponse(false, errors, null));
            }
        }



    }
}
