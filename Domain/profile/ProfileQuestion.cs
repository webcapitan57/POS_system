using System;
using System.ComponentModel.DataAnnotations;
using POC.BL.Domain.setup;

namespace POC.BL.Domain.profile
{
    public abstract class ProfileQuestion
    {
        public int ProfileQuestionId { get; set; }
        public string Description { get; set; }
        public string? Question { get; set; }
        public bool IsRequired { get; set; }
        [Required]
        public SetUp SetUp { get; set; }
    }
}