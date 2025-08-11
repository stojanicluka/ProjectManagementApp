using ProjectManagementAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementAPI.DTO
{
    public class GetTaskDTO
    {
        public int Id { get; set; }
        public String Title { get; set; }

        public String Description { get; set; }

        public DateTime Deadline { get; set; }

        public Priority Priority { get; set; }

        public Status Status { get; set; }

        public String userId { get; set; }
    }
}
