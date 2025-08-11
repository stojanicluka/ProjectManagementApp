using System.ComponentModel.DataAnnotations;

namespace ProjectManagementAPI.DTO
{
    public class PatchTaskDTO
    {
        public class Patch
        {
            public string Field;
            public Object Value;
        }

        [Required]
        public List<Patch> Patches { get; set; }

    }
}
