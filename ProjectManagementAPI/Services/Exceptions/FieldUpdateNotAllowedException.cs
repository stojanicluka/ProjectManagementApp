namespace ProjectManagementAPI.Services.Exceptions
{
    public class FieldUpdateNotAllowedException : Exception
    {
        public FieldUpdateNotAllowedException(String message) :base(message){ }
    }
}
