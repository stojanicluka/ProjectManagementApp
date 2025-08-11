using System.ComponentModel.DataAnnotations;

namespace ProjectManagementAPI.DTO
{
    public class PatchTaskDTO
    {
        public class Patch
        {
            [Required]
            public String Field { get; set; }
            public Object Value { get; set; }
        }

        [Required]
        public List<Patch> Patches { get; set; }

    }
}
