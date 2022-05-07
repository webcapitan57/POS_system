using System.Collections.Generic;
using POC.BL.Domain.delivery;
using POC.DAL.EF.SC.DAL.EF;

namespace UI_MVC.Models.Task
{
    public class ShowPhotoModel
    {
        public string Url { get; set; }
        public int PhotoId { get; set; }
    }
}