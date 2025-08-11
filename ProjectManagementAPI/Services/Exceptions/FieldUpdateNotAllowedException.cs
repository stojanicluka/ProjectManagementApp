namespace ProjectManagementAPI.Services.Exceptions
{
    public class FieldUpdateNotAllowedException : APIException
    {
        public new String Type { get { return "FieldUpdateNotAllowedException"; } }
        public FieldUpdateNotAllowedException(String message) :base(message){ }
    }
}
