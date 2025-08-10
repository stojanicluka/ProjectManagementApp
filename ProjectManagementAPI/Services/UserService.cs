using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectManagementAPI.DTO;
using ProjectManagementAPI.Models;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace ProjectManagementAPI.Services
{
    public class UserService
    {
        private static String emailPattern = "([a-z]|[A-Z]|[0-9])+@([a-z]|[A-Z]|[0-9])+\\.([a-z]|[A-Z])+";
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public enum RegistrationResult { SUCCESS, USERNAME_EXISTS, WRONG_EMAIL_FORMAT }
        public enum RoleAssignmentResult { SUCCESS, USER_NOT_FOUND, ROLE_NOT_FOUND, ASSIGNMENT_FAILED }
        public enum UserUpdateResult { SUCCESS, USER_NOT_FOUND, WRONG_EMAIL_FORMAT, USERNAME_EXISTS, USER_UPDATE_FAILED }
        public enum PasswordChangeResult { SUCCESS, USER_NOT_FOUND, WRONG_CURRENT_PASSWORD }

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<RegistrationResult> RegisterUserAsync(UserDTO uDTO)
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


        public async Task<RoleAssignmentResult> AssignRoleAsync(String userID, String roleID)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userID);
            if (user == null)
                return RoleAssignmentResult.USER_NOT_FOUND;

            IdentityRole? role = await _roleManager.FindByIdAsync(roleID);
            if (role == null)
                return RoleAssignmentResult.ROLE_NOT_FOUND;

            if (!(await _userManager.AddToRoleAsync(user, role.Name)).Succeeded)
                return RoleAssignmentResult.ASSIGNMENT_FAILED;

            return RoleAssignmentResult.SUCCESS;
        }

        public async Task<List<RoleDTO>> FetchAllRolesAsync()
        {
            return await _roleManager.Roles.Select(role => new RoleDTO()
            {
                Id = role.Id,
                Name = role.Name
            }).ToListAsync();
        }

        public async Task<UserUpdateResult> UpdateUserAsync(String id, UserDTO uDTO)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return UserUpdateResult.USER_NOT_FOUND;

            if (await _userManager.FindByNameAsync(uDTO.Username) != null)
                return UserUpdateResult.USERNAME_EXISTS;

            if (!Regex.IsMatch(uDTO.Email, emailPattern))
                return UserUpdateResult.WRONG_EMAIL_FORMAT;

            user.FirstName = uDTO.FirstName;
            user.LastName = uDTO.LastName;
            user.Email = uDTO.Email;
            user.UserName = uDTO.Username;

            await _userManager.UpdateAsync(user);

            return UserUpdateResult.SUCCESS;
        }

        public async Task<PasswordChangeResult> ChangePasswordAsync(String id, PasswordDTO pDTO)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return PasswordChangeResult.USER_NOT_FOUND;

            if (await _userManager.CheckPasswordAsync(user, pDTO.OldPassword))
                return PasswordChangeResult.WRONG_CURRENT_PASSWORD;

            await _userManager.ChangePasswordAsync(user, pDTO.OldPassword, pDTO.NewPassword);

            return PasswordChangeResult.SUCCESS;
        }

        public async Task<PasswordChangeResult> ResetPasswordAsync(String id, PasswordDTO pDTO)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return PasswordChangeResult.USER_NOT_FOUND;

            String token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, token, pDTO.NewPassword);

            return PasswordChangeResult.SUCCESS;
        }

        public async Task<bool> DeleteUserAsync(String id)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return false;

            await _userManager.DeleteAsync(user);
            return true;
        }

        public async Task<UserDTO> FetchUserAsync(String id)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return null;

            UserDTO uDTO = new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                Email = user.Email
            };

            IList<String> userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Count == 0)
            {
                IdentityRole role = await _roleManager.FindByNameAsync(userRoles[0]);
                uDTO.Role = new RoleDTO { Id = role.Id, Name = role.Name };
            }

            return uDTO;
        }

        public async Task<List<UserDTO>> FetchAllUsersAsync()
        {
            return await _userManager.Users.Select(user => new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                Email = user.Email

            }).ToListAsync();
        }
    }
}
