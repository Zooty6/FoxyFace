using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using DatabaseAccess;
using DatabaseAccess.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace FoxyFaceAPI.Controllers
{
    [Route("api/[controller]")]
    public class PostController : Controller
    {
        private Random random;

        public PostController()
        {
            random = new Random();
        }
        
        [HttpGet]
        public JsonResult Get(int postId, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Json(new
                {
                    success = false,
                    error = ErrorObjects.ParametersAreNotValid
                });
            }
            Session session = FoxyFaceDbManager.GetNewConnection.SessionRepository.FindByToken(token);
            if (session == null)
            {
                return Json(new
                {
                    success = false,
                    error = ErrorObjects.TokenNotValid
                });
            }
            
            Post post = FoxyFaceDbManager.GetNewConnection.PostRepository.FindById(postId);
            if (post == null)
            {
                return Json(new
                {
                    error = ErrorObjects.WrongPostId
                });
            }

            List<Rating> listOfRatings = FoxyFaceDbManager.GetNewConnection.RatingRepository.FindByPostId(postId);
            List<Comment> listOfComments = FoxyFaceDbManager.GetNewConnection.CommentRepository.FindByPostId(postId);
            return Json(new
            {
                title = post.Title,
                description = post.Description,
                user = post.User.Value,
                path = post.Path,
                ratings = listOfRatings,
                comments = listOfComments
            });
        }
        
        [HttpPost]
        public async Task<JsonResult> Post(string title, string description, IFormFile file, string cameraBase64, string token)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(token) || (file == null && string.IsNullOrEmpty(cameraBase64)))
            {
                return Json(new
                {
                    success = false,
                    error = ErrorObjects.ParametersAreNotValid
                });
            }
            Session session = FoxyFaceDbManager.GetNewConnection.SessionRepository.FindByToken(token);
            if (session == null)
            {
                return Json(new
                {
                    success = false,
                    error = ErrorObjects.TokenNotValid
                });
            }


            string filename;
            MemoryStream tempMemoryStream = new MemoryStream();
            if (file != null)
            {
                file.CopyTo(tempMemoryStream);
                filename = file.FileName;
            }
            else
            {
                byte[] cameraBytes = Convert.FromBase64String(cameraBase64);
                tempMemoryStream.Write(cameraBytes, 0, cameraBytes.Length);
                filename = "camera";
            }
            tempMemoryStream.Seek(0, SeekOrigin.Begin);
            
            string blobPath;
            do
            {
                blobPath = session.User.Value.Username + "/" + random.Next() + "_" + filename;
            } while (CloudStorage.Instance.FileExists(blobPath));

            try
            {
                using (Image<Rgba32> image = Image.Load(tempMemoryStream))
                {
                    if (image.Width > image.Height)
                        image.Mutate(x => x.Resize(180, 180 * image.Height / image.Width));
                    else
                        image.Mutate(x => x.Resize(180 * image.Width / image.Height, 180));
                    
                    MemoryStream thumbnailTempMemoryStream = new MemoryStream();
                    image.Save(thumbnailTempMemoryStream, new JpegEncoder());
                    thumbnailTempMemoryStream.Seek(0, SeekOrigin.Begin);
                    CloudStorage.Instance.UploadFile(blobPath + "thumbnail.jpeg", thumbnailTempMemoryStream);
                }
            }
            catch (NotSupportedException)
            {
                return Json(new
                {
                    success = false,
                    error = ErrorObjects.NotAValidImage
                });
            }
            
            tempMemoryStream.Seek(0, SeekOrigin.Begin);
            Uri uri = CloudStorage.Instance.UploadFile(blobPath, tempMemoryStream).Result;
            

            Post post = FoxyFaceDbManager.GetNewConnection.PostRepository.Create(session.User.Value, title, description, uri.ToString());
            return Json(new
            {
                postId = post.Id
            });
        }
    }
}