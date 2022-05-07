using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POC.BL.Domain.task
{
    public class SideQuestionOption
    {
        public string? Value { get; set; }
        [Required]
        public SideQuestion MultipleChoiceSideQuestion { get; set; }

        public int SideQuestionOptionId { get; set; }
        
    }
}