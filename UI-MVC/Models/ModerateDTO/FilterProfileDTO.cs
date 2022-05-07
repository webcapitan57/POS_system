using System.Collections.Generic;

namespace UI_MVC.Models.ModerateDTO
{
    public class FilterProfileDTO
    {
        public string Name { get; set; }
        public int UserId { get; set; }
        public Dictionary<string, string> Filters { get; set; }
    }
}