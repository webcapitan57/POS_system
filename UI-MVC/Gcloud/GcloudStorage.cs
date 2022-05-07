using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Api.Gax;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
using Google.Cloud.Vision.V1;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Object = Google.Apis.Storage.v1.Data.Object;

namespace UI_MVC.Gcloud
{
    public class GcloudStorage
    {
        private readonly GcloudOptions _options;
        


        public GcloudStorage(IOptions<GcloudOptions> options)
        {
            _options = options.Value;
            
        }


        public void UploadImage(string localPath , string objectName,string type="Images/" )
        {
            GoogleCredential credentials = GoogleCredential.FromFile(_options.ServiceAccountKeyFile);
            var storage = StorageClient.Create(credentials);
            using var fileStream = File.OpenRead(localPath);
            objectName = type + objectName;
            storage.UploadObject(_options.BucketName, objectName, null, fileStream);
            
            
        }

        public  void DeleteImage( string objectName,string type="Images/")
        {
            GoogleCredential credentials = GoogleCredential.FromFile(_options.ServiceAccountKeyFile);
            var storage = StorageClient.Create(credentials);
            storage.DeleteObject(_options.BucketName,type+objectName);
        }

        public string GetRandomGif(string type)
        { 
            GoogleCredential credentials = GoogleCredential.FromFile(_options.ServiceAccountKeyFile);
            var rand = new Random();
           
            
            var storage = StorageClient.Create(credentials);
            
            var gifs = storage.ListObjects(_options.BucketName,"gifs/"+type);
            var randomNumber = rand.Next(1,gifs.Count());
            var chosenGif = "";
            foreach (var storageObject in gifs)
            {
                if (storageObject.Name.Equals("gifs/"+type+"/"+randomNumber+".gif"))
                {
                    chosenGif = storageObject.Name;
                    break;
                }
            }
            
            return chosenGif;
        }
        
        public async Task<Boolean> CheckForFace(string photo)
        {
            var image = Image.FromUri("https://storage.googleapis.com/" + _options.BucketName + "/Images/" + photo);
            var credential = Path.GetFullPath(_options.ServiceAccountKeyFile);
            var client = await new ImageAnnotatorClientBuilder
            {
                CredentialsPath = _options.ServiceAccountKeyFile
            }.BuildAsync();

             var labels = await client.DetectFacesAsync(image);

            //if labels.count 0 it means that vision api didn't find a face in the picture
            return labels.Count != 0;
        }

    }
}