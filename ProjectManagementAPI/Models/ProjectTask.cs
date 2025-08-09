using Microsoft.AspNetCore.Identity;
using ProjectManagementAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementAPI.Models
{
    public class ProjectTask
    {

        public ProjectTask(string title, string description, DateTime deadline, Priority priority, Status status, Project project, IdentityUser assignedTo) 
        {
            Title = title;
            Description = description;
            Deadline = deadline;
            Priority = priority;
            Status = status;
            Project = project;
            AssignedTo = assignedTo;
        }

        private ProjectTask() { }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public Priority Priority {  get; set; }
        public Status Status {  get; set; }

        public Project Project { get; set; }

        [Required]
        public ApplicationUser AssignedTo { get; set; }
    }
}
