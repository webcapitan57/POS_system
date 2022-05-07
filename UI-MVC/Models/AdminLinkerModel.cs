using System.Collections.Generic;
using POC.BL.Domain.user;

namespace UI_MVC.Models
{
    public class AdminLinkerModel
    {
        public int SetUpId { get; set; }
        public ICollection<Admin> LinkedAdmins { get; set; }
        public ICollection<Admin> UnlinkedAdmins { get; set; }
    }
}