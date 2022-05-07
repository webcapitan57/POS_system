using System.Collections.Generic;

namespace UI_MVC.Models
{
    public class SideQuestionDTO
    {
        public int SideQuestionId { get; set; }
        public string Question { get; set; }
        public List<SideQuestionOptionDTO> SideQuestionOptions { get; set; }
    }
}