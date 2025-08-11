using Microsoft.AspNetCore.Identity;
using ProjectManagementAPI.DTO;
using ProjectManagementAPI.Models;
using ProjectManagementAPI.Models.Enums;
using ProjectManagementAPI.Services.Exceptions;

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

        public async Task CreateTaskAsync(int projectId, CreateTaskDTO dto)
        {
            Project? project = await FindProject(projectId);
            if (project == null) 
                throw new ProjectNotFoundException("Project with " + projectId.ToString() + " not found");

            ApplicationUser? user = await _userManager.FindByIdAsync(dto.userId);
            if (user == null)
                throw new UserNotFoundException("User with id " + dto.userId.ToString() + " not found");


            ProjectTask task = new ProjectTask(dto.Title, dto.Description, dto.Deadline, dto.Priority, dto.Status, project, user);
            _dbContext.Tasks.Add(task);
            if (_dbContext.SaveChanges() == 0)
                throw new DatabaseException("Error when writing to database");
        }

        public async Task UpdateTaskAsync(int projectId, int taskId, PatchTaskDTO dto)
        {
            if (await FindProject(projectId) == null)
                throw new ProjectNotFoundException("Project with " + projectId.ToString() + " not found");

            ProjectTask? task = await _dbContext.Tasks.FindAsync(taskId);
            if (task == null)
                throw new TaskNotFoundException("Task with id " + taskId.ToString() + " not found");

            foreach (PatchTaskDTO.Patch p in dto.Patches)
            {
                switch (p.Field)
                {
                    case "Title":
                        task.Title = (String)p.Value;
                        break;
                    case "Description":
                        task.Description = (String)p.Value;
                        break;
                    case "Deadline":
                        task.Deadline = (DateTime)p.Value;
                        break;
                    case "Priority":
                        task.Priority = (Priority)p.Value;
                        break;
                    case "Status":
                        task.Status = (Status)p.Value;
                        break;
                    case "UserId":
                        ApplicationUser? user = await _userManager.FindByIdAsync((String)p.Value);
                        if (user == null)
                            throw new UserNotFoundException("User with ID " + (String)p.Value + " not found");
                        task.AssignedTo = user;
                        break;
                    default:
                        throw new FieldUpdateNotAllowedException("Field " +  p.Field + " can't be modified");
                }

            }


            if (_dbContext.SaveChanges() == 0)
                throw new DatabaseException("Error when writing to database");
        }

        public async Task DeleteTaskAsync(int projectId, int taskId)
        {
            if (await FindProject(projectId) == null)
                throw new ProjectNotFoundException("Project with " + projectId.ToString() + " not found");


            ProjectTask? task = await _dbContext.Tasks.FindAsync(taskId);
            if (task == null)
                throw new TaskNotFoundException("Task with id " + taskId.ToString() + " not found");

            _dbContext.Tasks.Remove(task);
            if (await _dbContext.SaveChangesAsync() == 0)
                throw new DatabaseException("Error when deleting from database");
        }

        public async Task<GetTaskDTO> GetTaskAsync(int projectId, int taskId)
        {
            if (await FindProject(projectId) == null)
                throw new ProjectNotFoundException("Project with " + projectId.ToString() + " not found");


            ProjectTask? task = await _dbContext.Tasks.FindAsync(taskId);
            if (task == null)
                throw new TaskNotFoundException("Task with id " + taskId.ToString() + " not found");

            return new GetTaskDTO 
            { 
                Id = task.Id,
                Description = task.Description,
                Deadline = task.Deadline,
                Priority = task.Priority,
                Status = task.Status,
                Title = task.Title,
                userId = task.AssignedTo.Id
            };
        }
    }
}
