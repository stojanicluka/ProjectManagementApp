using Microsoft.AspNetCore.Identity;
using ProjectManagementAPI.DTO;
using ProjectManagementAPI.Models;
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
    }
}
