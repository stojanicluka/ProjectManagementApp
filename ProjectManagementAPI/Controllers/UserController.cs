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
                case UserService.RegistrationResult.SUCCESS:
                    return Ok();
                case UserService.RegistrationResult.USERNAME_EXISTS:
                    return Conflict("Username already exists");
                case UserService.RegistrationResult.WRONG_EMAIL_FORMAT:
                    return BadRequest("Wrong email address format");
            }
        }


    }
}
