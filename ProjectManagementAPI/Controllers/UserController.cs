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
        public async Task<IActionResult> LoginAsync(LoginDTO lDTO)
        {
            return NotFound("Debug: Endpoint not implemented");
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateUserAsync(String id, UserDTO uDTO)
        {
            switch (await _userService.UpdateUserAsync(id, uDTO))
            {
                case UserService.UserUpdateResult.USERNAME_EXISTS:
                    return Conflict("Username already exists");
                case UserService.UserUpdateResult.USER_NOT_FOUND:
                    return NotFound("User not found");
                case UserService.UserUpdateResult.WRONG_EMAIL_FORMAT:
                    return BadRequest("Wrong email format");
                default:
                    return Ok();
            }
        }

        [HttpPut]
        [Route("password/change/{id}")]
        public async Task<IActionResult> ChangePasswordAsync(String id, PasswordDTO pDTO)
        {
            switch (await _userService.ChangePasswordAsync(id, pDTO))
            {
                case UserService.PasswordChangeResult.USER_NOT_FOUND:
                    return NotFound("User not found");
                case UserService.PasswordChangeResult.WRONG_CURRENT_PASSWORD:
                    return BadRequest("Wrong current password");
                default:
                    return Ok();
            }
        }

        [HttpPut]
        [Route("password/reset/{id}")]
        public async Task<IActionResult> ResetPasswordAsync(String id, PasswordDTO pDTO)
        {
            switch (await _userService.ResetPasswordAsync(id, pDTO))
            {
                case UserService.PasswordChangeResult.USER_NOT_FOUND:
                    return NotFound("User not found");
                default:
                    return Ok();
            }
        }

        [HttpPut]
        [Route("role/{userID}/{roleID}")]
        public async Task<IActionResult> AssignRoleAsync(String userID, String roleID)
        {
            switch (await _userService.AssignRoleAsync(userID, roleID))
            {
                case UserService.RoleAssignmentResult.USER_NOT_FOUND:
                    return NotFound("User not found");
                case UserService.RoleAssignmentResult.ROLE_NOT_FOUND:
                    return NotFound("Role not found");
                default:
                    return Ok();
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUserAsync(String id)
        {
            return Ok(await _userService.DeleteUserAsync(id));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> FetchUserAsync(String id)
        {
            UserDTO uDTO = await _userService.FetchUserAsync(id);
            if (uDTO == null)
                return NotFound("User not found");
            return Ok(uDTO);
        }

        [HttpGet]
        public async Task<IActionResult> FetchAllUsersAsync()
        {
            return Ok(await _userService.FetchAllUsersAsync());
        }

        [HttpGet]
        [Route("role")]
        public async Task<IActionResult> FetchAllRolesAsync()
        {
            return Ok(await _userService.FetchAllRolesAsync());
        }
    }
}
