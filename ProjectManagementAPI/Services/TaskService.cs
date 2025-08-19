using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementAPI.DTO;
using ProjectManagementAPI.Models;
using ProjectManagementAPI.Models.Enums;
using ProjectManagementAPI.Services.Exceptions;
using System.Text.Json;

namespace ProjectManagementAPI.Services
{
    public class TaskService
    {
        private AppDBContext _dbContext;
        private UserManager<ApplicationUser> _userManager;

        public TaskService(AppDBContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<Project?> FindProject(int projectId)
        {
            return await _dbContext.Projects.FindAsync(projectId);
        }

        public async Task<IntegerIdDTO> CreateTaskAsync(CreateTaskDTO dto)
        {
            Project? project = await FindProject(dto.ProjectId);
            if (project == null) 
                throw new ProjectNotFoundException("Project with id " + dto.ProjectId.ToString() + " not found");

            ApplicationUser? user = await _userManager.FindByIdAsync(dto.userId);
            if (user == null)
                throw new UserNotFoundException("User with id " + dto.userId.ToString() + " not found");


            ProjectTask task = new ProjectTask(dto.Title, dto.Description, dto.Deadline, dto.Priority, dto.Status, project, user);
            _dbContext.Tasks.Add(task);
            if (await _dbContext.SaveChangesAsync() == 0)
                throw new DatabaseException("Error when writing to database");

            return new IntegerIdDTO { Id = task.Id };
        }

        public async Task UpdateTaskAsync(int taskId, PatchDTO dto, string username)
        {
            ProjectTask? task = await _dbContext.Tasks.FindAsync(taskId);
            if (task == null)
                throw new TaskNotFoundException("Task with id " + taskId.ToString() + " not found");

            ApplicationUser? appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null)
                throw new UserNotFoundException("Internal error: Authentified user not found");

            string role = (await _userManager.GetRolesAsync(appUser)).First();

            if (role == null)
                throw new RoleNotFoundException("Internal error: Authentified user not found");

            if (role == "TEAM_MEMBER" && task.AssignedTo.UserName != username)
                throw new UnauthorizedException("User not authorized to update task");



            foreach (PatchDTO.Patch p in dto.Patches)
            {
                switch (p.Field)
                {
                    case "Title":
                        task.Title = ((JsonElement)p.Value).Deserialize<String>();
                        break;
                    case "Description":
                        task.Description = ((JsonElement)p.Value).Deserialize<String>();
                        break;
                    case "Deadline":
                        task.Deadline = ((JsonElement)p.Value).Deserialize<DateTime>();
                        break;
                    case "Priority":
                        task.Priority = ((JsonElement)p.Value).Deserialize<Priority>();
                        break;
                    case "Status":
                        task.Status = ((JsonElement)p.Value).Deserialize<Status>();
                        break;
                    case "UserId":
                        String userId = ((JsonElement)p.Value).Deserialize<String>();
                        ApplicationUser? user = await _userManager.FindByIdAsync(userId);
                        if (user == null)
                            throw new UserNotFoundException("User with ID " + (String)p.Value + " not found");
                        task.AssignedTo = user;
                        break;
                    default:
                        throw new FieldUpdateNotAllowedException("Field " +  p.Field + " can't be modified");
                }

            }


            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(int taskId)
        {
            ProjectTask? task = await _dbContext.Tasks.FindAsync(taskId);
            if (task == null)
                throw new TaskNotFoundException("Task with id " + taskId.ToString() + " not found");

            _dbContext.Tasks.Remove(task);
            if (await _dbContext.SaveChangesAsync() == 0)
                throw new DatabaseException("Error when deleting from database");
        }

        public async Task<GetTaskDTO> GetTaskAsync(string username, int taskId)
        {
            IQueryable<ProjectTask> query = _dbContext.Tasks.Include(task => task.AssignedTo).Include(task => task.Project)
                .Where(task => task.Id == taskId);

            if (query.Count() == 0)
                throw new TaskNotFoundException("Task with id " + taskId.ToString() + " not found");


            ProjectTask task = await query.FirstAsync();

            ApplicationUser? user = await _userManager.FindByNameAsync(username);
            string role = (await _userManager.GetRolesAsync(user)).FirstOrDefault("INACTIVE");
            if (task.AssignedTo != user && role != "ADMIN" && role != "MANAGER")
                throw new UnauthorizedException("User not authorized to fetch task not assigned to him");
                
            return new GetTaskDTO
            {
                Deadline = task.Deadline,
                Priority = task.Priority,
                Description = task.Description,
                Id = task.Id,
                Status = task.Status,
                Title = task.Title,
                UserId = task.AssignedTo.Id,
                ProjectId = task.Project.Id
            };
        }

        public async Task<List<GetTaskDTO>> GetAllProjectUserTasksAsync(int? projectId, String? userId, Status? status)
        {
            IQueryable<ProjectTask> query = _dbContext.Tasks;

            if (projectId != null)
                query = query.Where(task => task.Project.Id == projectId);

            if (userId != null)
                query = query.Where(task => task.AssignedTo.Id == userId);

            if (status != null)
                query = query.Where(task => task.Status == status);

            return await query.Select(task => new GetTaskDTO
            {
                Deadline = task.Deadline,
                Priority = task.Priority,
                Description = task.Description,
                Id = task.Id,
                Status = task.Status,
                Title = task.Title,
                UserId = task.AssignedTo.Id,
                ProjectId = task.Project.Id
            }).ToListAsync();
        }
    }
}
