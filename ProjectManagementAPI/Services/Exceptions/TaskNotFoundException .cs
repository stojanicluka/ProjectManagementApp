namespace ProjectManagementAPI.Services.Exceptions
{
    public class TaskNotFoundException : APIException 
    {
        public new String Type { get { return "TaskNotFoundException"; } }
        public TaskNotFoundException(String message) :base(message) { }
    }
}
