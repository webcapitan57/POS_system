using System.Collections.Generic;
using POC.BL.Domain.delivery;
using POC.BL.Domain.user;

namespace UI_MVC.Models.Moderate
{
    public class ShowDeliveriesViewModel
    {
        public Group Group { get; set; }
        public Teacher Teacher { get; set; }
        public IList<Answer> Answers { get; set; }
        public List<Tag> Filters { get; set; }

        //Needed for grid presentation
        public IList<List<Answer>> Rows { get; set; }
        public IList<Answer> LikedAnswers { get; set; }
    }
}