using System.Collections.Generic;
using POC.BL.Domain.delivery;
using POC.BL.Domain.setup;

namespace POC.BL.Domain.task
{
    public class SetTask : Task
    {
        public SetUp SetUp { get; set; }
        public ICollection<SetTaskDelivery> SetTaskDeliveries { get; set; }
    }
}