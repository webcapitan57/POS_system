using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POC.BL.Domain.delivery;
using POC.BL.Domain.setup;

namespace UI_MVC.Models
{
    public class PhotoViewModel
    {
        [Required(ErrorMessage = "Gelieve een foto te kiezen")]
        public IFormFile Image { get; set; }
        
        public IList<Photo> Photos {get; set; }
        public int StudentId { get; set; }
        public string GroupCode { get; set; }
        public int? DeliveryId { get; set; }
        public int? PhotoQuestionId { get; set; }
        public string PreviousAction { get; set; }

        public int SetupId { get; set; }
        public string SetupName { get; set; }
    }
}