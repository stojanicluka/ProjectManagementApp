using ProjectManagementAPI.Models.Enums;

namespace ProjectManagementAPI.Models
{
    public class Project
    {
        public Project(string title, string description, DateTime startDate, DateTime deadline, Status status)
        {
            Title = title;
            Description = description;
            StartDate = startDate;
            Deadline = deadline;
            EndDate = null;
            Status = status;
        }

        private Project() { }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Deadline {  get; set; }
        public DateTime? EndDate { get; set; }
        public Status Status { get; set; }
    }
}
