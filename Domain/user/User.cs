using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using POC.BL.Domain.delivery;

namespace POC.BL.Domain.user
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public string? UserAccountId { get; set; }
        public UserAccount UserAccount { get; set; }

        public IList<FilterProfile> FilterProfiles { get; set; }
        public ICollection<QualityScoreEvent> QualityScoreEvents { get; set; }
    }
}