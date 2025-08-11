using ProjectManagementAPI.Models.Enums;

namespace ProjectManagementAPI.DTO
{
    public class CreateTaskDTO
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public DateTime Deadline { get; set; }

        public Priority Priority { get; set; }
        public Status Status { get; set; }
        public int userId { get; set; }
    }
}
