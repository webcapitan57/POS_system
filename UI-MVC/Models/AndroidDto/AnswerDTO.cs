using System.Collections.Generic;

namespace UI_MVC.Models.AndroidDto
{
    public class AnswerDTO
    {
        public int AnswerId { get; set; }
        public int AnsweredQuestionId { get; set; }
        public string Photo { get; set; }
        public List<SideAnswerDTO> SideAnswers { get; set; }
    }
}