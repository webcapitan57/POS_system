using POC.BL.Domain.delivery;
using POC.BL.Domain.setup;

namespace UI_MVC.Models
{
    public class ShowQuestionsViewModel
    {
        public TaskDelivery Delivery { get; set; }
        public SetUp SetUp { get; set; }
    }
}