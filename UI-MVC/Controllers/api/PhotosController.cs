using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using POC.BL.Domain.delivery;
using POC.BL.logic;
using POC.BL.logic.InterFaces;
using UI_MVC.Gcloud;
using UI_MVC.Models;
using UI_MVC.Models.Task;
using UI_MVC.Views.Shared;

namespace UI_MVC.Controllers
{
    [ApiController]
    [Route("/api/Photos")]
    public class PhotosController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;
        private readonly ISetupService _setupService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly GcloudStorage _storage;
        private readonly GcloudOptions _options;
        
        
        public PhotosController(IDeliveryService deliveryService,ISetupService setupService, 
            IWebHostEnvironment webHostEnvironment, GcloudStorage storage,IOptions<GcloudOptions> options)
            
        {
            _deliveryService = deliveryService;
            _setupService = setupService;
            _webHostEnvironment = webHostEnvironment;
            _options = options.Value;
            _storage = storage;

        }

        [HttpGet("{deliveryId}")]
        public IActionResult GetUploadedPhotos(int deliveryId, string type)
        {
            List<Photo> photos = new List<Photo>();
            photos.AddRange(_deliveryService.GetPhotosOfDelivery(deliveryId));
            
            
            if (photos.Count == 0) return NoContent();
            List<ShowPhotoModel> responsePhotos = new List<ShowPhotoModel>();
            foreach (var photo in photos)
            {
                responsePhotos.Add(new ShowPhotoModel()
                {
                    PhotoId = photo.PhotoId,
                    Url = "https://storage.googleapis.com/" + _options.BucketName + "/Images/" + photo.Picture
                    
                });
            }
            return Ok(responsePhotos);
        }

        [HttpGet("{deliveryId}/{photoQuestionId}")]
        public IActionResult GetAssignedPhotos(int deliveryId,int photoQuestionId)
        {
            List<Photo> photos = new List<Photo>();
            List<ShowPhotoModel> responsePhotos = new List<ShowPhotoModel>();
            photos.AddRange(_deliveryService.GetAssignedPhotos(deliveryId,photoQuestionId));
            foreach (var photo in photos)
            {
                responsePhotos.Add(new ShowPhotoModel()
                {
                    PhotoId = photo.PhotoId,
                    Url = "https://storage.googleapis.com/" + _options.BucketName + "/Images/" + photo.Picture
                    
                });
            }
            return Ok(responsePhotos);
        }

        
        [HttpPost("{type}/{Id}")]
        public async Task<IActionResult> UploadPhoto(string type,int id,[FromForm] UploadPhotoDTO images)
        {
            
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            var fileName = Guid.NewGuid() + "_" + images.Image.FileName;
            var filePath = Path.Combine(uploadsFolder, fileName);
            if (images.Image.Length>0)
            {
                try
                {
                    using (FileStream fileStream=new FileStream(filePath,FileMode.Create))
                    {
                        await images.Image.CopyToAsync(fileStream);
                        
                    } 
                    _storage.UploadImage(filePath,fileName);
                    System.IO.File.Delete(filePath);

                    if (type.ToLower().Equals("studentphoto"))
                    {
                        var deliveries = _deliveryService
                            .GetDeliveriesOfStudentForUploadPhoto(id).ToList();
                        Photo uploadedPhoto=_deliveryService.CreatePhoto(fileName, deliveries);
                        return Ok(new List<int>(){uploadedPhoto.PhotoId});
                    }

                    if (type.ToLower().Equals("setuplogo"))
                    {
                        _setupService.ChangeLogo(id,fileName);
                        return NoContent();
                    }

                    return StatusCode(404);

                }
                catch (Exception e)
                {
                    return StatusCode(500);
                }
            }
    
            return StatusCode(400);



        }



        [HttpDelete("{photoId}")]
        public IActionResult DeleteUploadedPhoto(int photoId)
        {
            Photo photo = _deliveryService.GetPhoto(photoId);
            List<Photo> photosList = _deliveryService.GetPhotosWithAnswers(photo.Picture).ToList();

            foreach (var p in photosList)
            {
                if (p.Answers.Count != 0)
                {
                    return Ok(false);
                }
            }
           _storage.DeleteImage(photo.Picture);
            _deliveryService.RemovePhotos(photosList);
            return Ok(true);
        }
        
        
        
    }
}