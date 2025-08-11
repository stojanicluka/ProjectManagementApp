using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectManagementAPI.DTO;
using ProjectManagementAPI.Models;
using ProjectManagementAPI.Models.Enums;
using ProjectManagementAPI.Services.Exceptions;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjectManagementAPI.Services
{
    public class UserService
    {
        private static String emailPattern = "([a-z]|[A-Z]|[0-9])+@([a-z]|[A-Z]|[0-9])+\\.([a-z]|[A-Z])+";
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<StringIdDTO> RegisterUserAsync(RegisterUserDTO dto)
        {
            if (await _userManager.FindByNameAsync(dto.Username) != null)
                throw new DuplicateUsernameException("Username " + dto.Username + " already exists");

            if (!Regex.IsMatch(dto.Email, emailPattern))
                throw new InvalidEmailFormatException("Email is not in valid format");

            ApplicationUser user = new ApplicationUser();
            user.UserName = dto.Username;
            user.Email = dto.Email;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;

            IdentityResult registrationResult = await _userManager.CreateAsync(user, dto.Password);
            if (!registrationResult.Succeeded)
                throw new RegistrationErrorException(registrationResult.Errors.Count() > 0 ? registrationResult.Errors.First().Description : "Unknown registration error");

            IdentityResult roleAssignmentResult = await _userManager.AddToRoleAsync(user, "INACTIVE");
            if (roleAssignmentResult.Errors.Count() > 0)
                throw new RegistrationErrorException(roleAssignmentResult.Errors.Count() > 0 ? roleAssignmentResult.Errors.First().Description : "Unknown registration error");

            return new StringIdDTO { Id = user.Id };
        }


        public async Task AssignRoleAsync(String userID, String roleID)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userID);
            if (user == null)
                throw new UserNotFoundException("User with ID " +  userID + " does not exist");

            IdentityRole? role = await _roleManager.FindByIdAsync(roleID);
            if (role == null)
                throw new RoleNotFoundException("Role with ID " + userID + " does not exist");

            IList<String> roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);

            if (!(await _userManager.AddToRoleAsync(user, role.Name)).Succeeded)
                throw new RoleAssignmentError("Unknown role assignment error");
        }

        public async Task<List<GetRoleDTO>> FetchAllRolesAsync()
        {
            return await _roleManager.Roles.Select(role => new GetRoleDTO()
            {
                Id = role.Id,
                Name = role.Name
            }).ToListAsync();
        }

        public async Task UpdateUserAsync(String id, PatchDTO dto)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new UserNotFoundException("User with ID " + id + " does not exist");


            foreach (PatchDTO.Patch p in dto.Patches)
            {
                switch (p.Field)
                {
                    case "FirstName":
                        user.FirstName = ((JsonElement)p.Value).Deserialize<String>();
                        break;
                    case "LastName":
                        user.LastName = ((JsonElement)p.Value).Deserialize<String>();
                        break;
                    case "Username":
                        user.UserName = ((JsonElement)p.Value).Deserialize<String>();
                        break;
                    case "Email":
                        String email = ((JsonElement)p.Value).Deserialize<String>();
                        if (!Regex.IsMatch(email, emailPattern))
                            throw new InvalidEmailFormatException("Email is not in valid format");
                        user.Email = email;
                        break;
                    default:
                        throw new FieldUpdateNotAllowedException("Field " + p.Field + " can't be modified");
                }
            }

            await _userManager.UpdateAsync(user);
        }

        public async Task ChangePasswordAsync(String id, PasswordDTO pDTO)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new UserNotFoundException("User with ID " + id + " does not exist");

            if (await _userManager.CheckPasswordAsync(user, pDTO.OldPassword))
                throw new WrongCurrentPasswordException("Wrong current password");

            IdentityResult result = await _userManager.ChangePasswordAsync(user, pDTO.OldPassword, pDTO.NewPassword);
            if (result.Errors.Count() > 0)
                throw new PasswordChangeError(result.Errors.First().Description);
        }

        public async Task ResetPasswordAsync(String id, PasswordDTO pDTO)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new UserNotFoundException("User with ID " + id + " does not exist");

            String token = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, token, pDTO.NewPassword);
            if (result.Errors.Count() > 0)
                throw new PasswordChangeError(result.Errors.First().Description);
        }

        public async Task DeleteUserAsync(String id)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new UserNotFoundException("User with ID " + id + " does not exist");

            if (!(await _userManager.DeleteAsync(user)).Succeeded)
                throw new DatabaseException("Error when deleting from a database");
        }

        public async Task<GetUserDTO> FetchUserAsync(String id)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new UserNotFoundException("User with ID " + id + " does not exist");

            GetUserDTO uDTO = new GetUserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                Email = user.Email
            };

            IList<String> userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Count != 0)
            {
                IdentityRole role = await _roleManager.FindByNameAsync(userRoles[0]);
                uDTO.Role = new GetRoleDTO { Id = role.Id, Name = role.Name };
            }

            return uDTO;
        }

        public async Task<List<GetUserDTO>> FetchAllUsersAsync()
        {
            List<ApplicationUser> users = await _userManager.Users.Select(user => user).ToListAsync();
            List<GetUserDTO> uDTOs = new List<GetUserDTO>();

            foreach (ApplicationUser user in users)
            {
                GetUserDTO uDTO = new GetUserDTO
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.UserName,
                    Email = user.Email
                };

                IList<String> roles = await _userManager.GetRolesAsync(user);
                if (roles.Count > 0)
                {
                    IdentityRole role = await _roleManager.FindByNameAsync(roles[0]);
                    uDTO.Role = new GetRoleDTO { Id = role.Id, Name = role.Name };    
                }

                uDTOs.Add(uDTO);

            }

            return uDTOs;
        }
    }
}
