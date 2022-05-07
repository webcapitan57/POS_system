using System.Collections.Generic;
using POC.BL.Domain.delivery;
using POC.BL.Domain.profile;
using POC.BL.Domain.user;

namespace UI_MVC.Models.Moderate
{
    public class ModerateDeliveriesViewModel
    {
        public Group Group { get; set; }
        public Teacher Teacher { get; set; }
        public IList<TaskDelivery> Deliveries { get; set; }
        public List<Tag> Filters { get; set; }
        public IList<Answer> LikedAnswers { get; set; }
        public List<StudentProfileQuestion> StudentProfileQuestions { get; set; }
    }
}