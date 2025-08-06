using ProjectManagementAPI.Models.Enums;

namespace ProjectManagementAPI.Models
{
    public class Task
    {

        public Task(int id, string title, string description, DateTime deadline, Priority priority, Status status, Project project) 
        {
            Title = title;
            Description = description;
            Deadline = deadline;
            Priority = priority;
            Status = status;
            Project = project;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public Priority Priority {  get; set; }
        public Status Status {  get; set; }

        public Project Project { get; set; }
    }
}
