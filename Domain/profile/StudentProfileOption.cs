using System.ComponentModel.DataAnnotations;

namespace POC.BL.Domain.profile
{
    public class StudentProfileOption:ProfileOption
    {
        [Required]
        public StudentMCProfileQuestion StudentMcProfileQuestion { get; set; }
    }
}