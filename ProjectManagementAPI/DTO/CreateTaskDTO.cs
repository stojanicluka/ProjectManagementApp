using ProjectManagementAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementAPI.DTO
{
    public class CreateTaskDTO
    {
        [Required]
        public String Title { get; set; }

        [Required]
        public String Description { get; set; }
        
        [Required]
        public DateTime Deadline { get; set; }

        [Required]
        public Priority Priority { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        public String userId { get; set; }

        [Required]
        public int ProjectId { get; set; }
    }
}
