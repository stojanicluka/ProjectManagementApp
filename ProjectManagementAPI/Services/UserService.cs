using Microsoft.AspNetCore.Identity;
using ProjectManagementAPI.DTO;
using ProjectManagementAPI.Models;
using System.Text.RegularExpressions;

namespace ProjectManagementAPI.Services
{
    public class UserService
    {
        private static String emailPattern = "([a-z]|[A-Z]|[0-9])+@([a-z]|[A-Z]|[0-9])+\\.([a-z]|[A-Z])+";
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public enum RegistrationResult { SUCCESS, USERNAME_EXISTS, WRONG_EMAIL_FORMAT }

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<RegistrationResult> RegisterUser(UserDTO uDTO)
        {
            if (await _userManager.FindByNameAsync(uDTO.Username) != null)
                return RegistrationResult.USERNAME_EXISTS;

            if (!Regex.IsMatch(uDTO.Email, emailPattern))
                return RegistrationResult.WRONG_EMAIL_FORMAT;

            ApplicationUser user = new ApplicationUser();
            user.UserName = uDTO.Username;
            user.Email = uDTO.Email;
            user.FirstName = uDTO.FirstName;
            user.LastName = uDTO.LastName;

            await _userManager.CreateAsync(user, uDTO.Password);
            await _userManager.AddToRoleAsync(user, "INACTIVE");

            return RegistrationResult.SUCCESS;
        }


    }
}
