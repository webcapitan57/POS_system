using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using POC.BL.Domain.setup;
using POC.BL.Domain.task;

namespace POC.BL.Domain.user
{
    public class GroupInfo
    {
        public int GroupInfoId { get; set; }
        public int TotalPhotos { get; set; }
        public int TotalStudents { get; set; }
        
        public string? TeacherName { get; set; }
        public DateTime CreationDate { get; set; }
        public Group Group { get; set; }
        public SetUp SetUp { get; set; }
    }
    
    
}