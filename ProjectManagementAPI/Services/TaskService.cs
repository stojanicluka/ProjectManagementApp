namespace ProjectManagementAPI.Services
{
    public class TaskService
    {
        private AppDBContext _dbContext;

        public enum TaskCreationResult { SUCCESS, USER_NOT_FOUND, FAILUREs }

        public TaskService(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TaskCreationResult> CreateTaskAsync()
    }
}
