using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementAPI.DTO;
using ProjectManagementAPI.Services;

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
        public async Task<IActionResult> RegisterUser(UserDTO uDTO)
        {
            switch (await _userService.RegisterUserAsync(uDTO))
            {
                case UserService.RegistrationResult.USERNAME_EXISTS:
                    return Conflict("Username already exists");
                case UserService.RegistrationResult.WRONG_EMAIL_FORMAT:
                    return BadRequest("Wrong email address format");
                default:
                    return Ok();
            }
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
    }
}
