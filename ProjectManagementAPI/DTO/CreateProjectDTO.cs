using ProjectManagementAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementAPI.DTO
{
    public class CreateProjectDTO
    {
        [Required]
        public String Title { get; set; }

        [Required]
        public String Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        [Required]
        public Status Status { get; set; }
    }
}
