namespace ProjectManagementAPI.Services
{
    public class APIResponse
    {
        public class Error
        {
            public String Type { get; set; }
            public String Message { get; set; }
        }

        public APIResponse()
        {
            Success = true;
            Errors = new List<Error>();
            Body = null;
        }

        public APIResponse(bool success, List<Error> errors, Object? body)
        {
            Success = success;
            Errors = errors;
            Body = body;
        }

        public bool Success { get; set; }
        public List<Error> Errors { get; set; }

        public Object? Body { get; set; }
        
    }
}
