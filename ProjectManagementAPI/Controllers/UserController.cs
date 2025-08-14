using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementAPI.DTO;
using ProjectManagementAPI.Services;
using ProjectManagementAPI.Services.Exceptions;

namespace ProjectManagementAPI.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private UserService _userService;

        public UserController(UserService userService) {
            _userService = userService;
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUserAsync(RegisterUserDTO dto)
        {
            try
            {
                return Ok(new APIResponse(await _userService.RegisterUserAsync(dto)));
            }
            catch (APIException ex)
            {
                List<APIResponse.Error> errors = new List<APIResponse.Error> { new APIResponse.Error { Type = ex.getType(), Message = ex.Message } };
                return BadRequest(new APIResponse(false, errors, null));
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(LoginDTO dto)
        {
            try
            {
                return Ok(new APIResponse(await _userService.LoginAsync(dto)));
            }
            catch (APIException ex)
            {
                List<APIResponse.Error> errors = new List<APIResponse.Error> { new APIResponse.Error { Type = ex.getType(), Message = ex.Message } };
                return BadRequest(new APIResponse(false, errors, null));
            }
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> UpdateUserAsync(String id, PatchDTO dto)
        {
            try
            {
                await _userService.UpdateUserAsync(id, dto);
                return Ok(new APIResponse());
            }
            catch (APIException ex)
            {
                List<APIResponse.Error> errors = new List<APIResponse.Error> { new APIResponse.Error { Type = ex.getType(), Message = ex.Message } };
                return BadRequest(new APIResponse(false, errors, null));
            }
        }

        [HttpPut]
        [Route("password/change/{id}")]
        public async Task<IActionResult> ChangePasswordAsync(String id, ChangePasswordDTO dto)
        {
            try
            {
                await _userService.ChangePasswordAsync(id, dto);
                return Ok(new APIResponse());
            }
            catch (APIException ex)
            {
                List<APIResponse.Error> errors = new List<APIResponse.Error> { new APIResponse.Error { Type = ex.getType(), Message = ex.Message } };
                return BadRequest(new APIResponse(false, errors, null));
            }
        }

        [HttpPut]
        [Route("password/reset/{id}")]
        public async Task<IActionResult> ResetPasswordAsync(String id, ChangePasswordDTO dto)
        {
            try
            {
                await _userService.ResetPasswordAsync(id, dto);
                return Ok(new APIResponse());
            }
            catch (APIException ex)
            {
                List<APIResponse.Error> errors = new List<APIResponse.Error> { new APIResponse.Error { Type = ex.getType(), Message = ex.Message } };
                return BadRequest(new APIResponse(false, errors, null));
            }
        }

        [HttpPut]
        [Route("role/{userID}")]
        public async Task<IActionResult> AssignRoleAsync(String userID, StringIdDTO dto)
        {
            try
            {
                await _userService.AssignRoleAsync(userID, dto);
                return Ok(new APIResponse());
            }
            catch (APIException ex)
            {
                List<APIResponse.Error> errors = new List<APIResponse.Error> { new APIResponse.Error { Type = ex.getType(), Message = ex.Message } };
                return BadRequest(new APIResponse(false, errors, null));
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUserAsync(String id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return Ok(new APIResponse());
            }
            catch (APIException ex)
            {
                List<APIResponse.Error> errors = new List<APIResponse.Error> { new APIResponse.Error { Type = ex.getType(), Message = ex.Message } };
                return BadRequest(new APIResponse(false, errors, null));
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> FetchUserAsync(String id)
        {
            try
            {   
                return Ok(await _userService.FetchUserAsync(id));
            }
            catch (APIException ex)
            {
                List<APIResponse.Error> errors = new List<APIResponse.Error> { new APIResponse.Error { Type = ex.getType(), Message = ex.Message } };
                return BadRequest(new APIResponse(false, errors, null));
            }
        }

        [HttpGet]
        public async Task<IActionResult> FetchAllUsersAsync()
        {
            try
            {
                return Ok(await _userService.FetchAllUsersAsync());
            }
            catch (APIException ex)
            {
                List<APIResponse.Error> errors = new List<APIResponse.Error> { new APIResponse.Error { Type = ex.getType(), Message = ex.Message } };
                return BadRequest(new APIResponse(false, errors, null));
            }
        }

        [HttpGet]
        [Route("role")]
        public async Task<IActionResult> FetchAllRolesAsync()
        {
            try
            {
                return Ok(await _userService.FetchAllRolesAsync());
            }
            catch (APIException ex)
            {
                List<APIResponse.Error> errors = new List<APIResponse.Error> { new APIResponse.Error { Type = ex.getType(), Message = ex.Message } };
                return BadRequest(new APIResponse(false, errors, null));
            }
        }
    }
}
