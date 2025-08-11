namespace ProjectManagementAPI.Services
{
    public class Response
    {
        public class Error
        {
            public String Type { get; set; }
            public String Message { get; set; }
        }

        public Response()
        {
            Errors = new List<Error>();
        }

        public bool Success { get; set; }
        public List<Error> Errors { get; set; }

        public Object? Body { get; set; }
        
    }
}
