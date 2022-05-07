using System;
using System.Collections.Generic;
using POC.BL.Domain.setup;

namespace POC.BL.Domain.delivery
{
    public class Photo
    {
        public int PhotoId { get; set; }
        public string Picture { get; set; }

        public ICollection<Answer>? Answers { get; set; }
        public TaskDelivery? TaskDelivery { get; set; }

        public SetUp? SetUp { get; set; }
    }
}