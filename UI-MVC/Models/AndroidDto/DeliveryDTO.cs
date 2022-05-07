using System.Collections.Generic;

namespace UI_MVC.Models.AndroidDto
{
    public class DeliveryDTO
    {
        public int TaskDeliveryId { get; set; }
        public int TaskId { get; set; }
        public string GroupCode { get; set; }
        public int StudentId { get; set; }
        public string Discriminator { get; set; }
        public string TaskTitle { get; set; }
        public IList<AnswerDTO> Answers { get; set; }
        public IList<TagDTO> Tags { get; set; }
    }
}