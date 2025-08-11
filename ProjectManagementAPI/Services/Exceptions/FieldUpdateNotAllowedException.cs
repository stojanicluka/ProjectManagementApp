namespace ProjectManagementAPI.Services.Exceptions
{
    public class FieldUpdateNotAllowedException : APIException
    {
        public FieldUpdateNotAllowedException(String message) :base(message){ }

        public override String getType()
        {
            return "FieldUpdateNotAllowedException";
        }
    }
}
