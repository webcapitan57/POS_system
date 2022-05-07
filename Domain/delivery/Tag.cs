namespace POC.BL.Domain.delivery
{
    public class Tag
    {
        public short TagId { get; set; }
        public string Value { get; set; }
        
        //TO DO
        public string? Description { get; set; }
        public TaskDelivery? TaskDelivery { get; set; }
        public Answer? Answer { get; set; }
    }
}