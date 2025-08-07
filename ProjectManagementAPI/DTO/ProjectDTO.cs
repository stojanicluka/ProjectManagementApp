using ProjectManagementAPI.Models.Enums;

namespace ProjectManagementAPI.DTO
{
    public class ProjectDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime? EndDate { get; set; }
        public Status Status { get; set; }
    }
}
