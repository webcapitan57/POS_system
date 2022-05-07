using System.ComponentModel;

namespace UI_MVC.Models
{
    public class ProfileQuestionDTO
    {
        public string Question { get; set; }
        public string Description { get; set; }
        public bool IsRequired { get; set; }
    }
}