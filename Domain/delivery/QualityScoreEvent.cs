using System.ComponentModel.DataAnnotations;
using POC.BL.Domain.user;

namespace POC.BL.Domain.delivery
{
    public class QualityScoreEvent
    {
        public int QualityScoreEventId { get; set; }
        public int Score { get; set; }
        public QualityScoreEventType EventType { get; set; }
        
        public Answer Answer { get; set; }
        public Teacher Teacher { get; set; }
    }
}