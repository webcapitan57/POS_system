using Microsoft.AspNetCore.Http;

namespace UI_MVC.Models
{
    public class UploadPhotoDTO
    {
        public IFormFile Image { get; set; }
    }
}