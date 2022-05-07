using System.ComponentModel.DataAnnotations;
using POC.BL.Domain.task;

namespace POC.BL.Domain.delivery
{
    public class SideAnswer
    {
        public int SideAnswerId { get; set; }
        [Required]
        public Answer Answer { get; set; }
        public SideQuestion AnsweredQuestion { get; set; }
        public string? GivenAnswer { get; set; }
    }
}