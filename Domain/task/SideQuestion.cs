using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using POC.BL.Domain.delivery;

namespace POC.BL.Domain.task
{
    public class SideQuestion
    {
        public int SideQuestionId { get; set; }

        public string? Question { get; set; }
        [Required]
        public PhotoQuestion PhotoQuestion { get; set; }
        
        public IList<SideQuestionOption> SideQuestionOptions { get; set; }

        public ICollection<SideAnswer> SideAnswers { get; set; }
        
    }
}