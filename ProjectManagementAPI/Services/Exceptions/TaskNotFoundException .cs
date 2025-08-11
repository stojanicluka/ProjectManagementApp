namespace ProjectManagementAPI.Services.Exceptions
{
    public class TaskNotFoundException : APIException 
    {
        public TaskNotFoundException(String message) :base(message) { }

        public override String getType()
        {
            return "TaskNotFoundException";
        }
    }
}
