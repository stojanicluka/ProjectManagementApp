namespace ProjectManagementAPI.Services.Exceptions
{
    public class TaskNotFoundException : Exception 
    {
        public TaskNotFoundException(String message) :base(message) { }
    }
}
