using System;
using System.Collections.Generic;
using System.IO;
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
            Session session = FoxyFaceDbManager.Instance.SessionRepository.FindByToken(token);
            if (session == null)
            {
                return Json(new
                {
                    success = false,
                    error = ErrorObjects.TokenNotValid
                });
            }
            
            Post post = FoxyFaceDbManager.Instance.PostRepository.FindById(postId);
            if (post == null)
            {
                return Json(new
                {
                    error = ErrorObjects.WrongPostId
                });
            }

            List<Rating> listOfRatings = FoxyFaceDbManager.Instance.RatingRepository.FindByPostId(postId);
            List<Comment> listOfComments = FoxyFaceDbManager.Instance.CommentRepository.FindByPostId(postId);
            return Json(new
            {
                title = post.Title,
                description = post.Description,
                imageUrl = post.Path,
                ratings = listOfRatings,
                comments = listOfComments
            });
        }
        
        [HttpPost]
        public async Task<JsonResult> Post(string title, string description, IFormFile file, string token)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(token))
            {
                return Json(new
                {
                    success = false,
                    error = ErrorObjects.ParametersAreNotValid
                });
            }
            Session session = FoxyFaceDbManager.Instance.SessionRepository.FindByToken(token);
            if (session == null)
            {
                return Json(new
                {
                    success = false,
                    error = ErrorObjects.TokenNotValid
                });
            }

            Console.WriteLine("Uploading file: " + file.FileName);
            
            var filePath = Path.Combine("temp", file.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create)) {
                await file.CopyToAsync(fileStream);
            }
            
            string blobPath;
            do
            {
                blobPath = session.User.Value.Username + "/" + random.Next() + "_" + file.FileName;
            } while (CloudStorage.Instance.FileExists(blobPath));
            
            using (Image<Rgba32> image = Image.Load(filePath))
            {
                image.Mutate(x => x.Resize(128, 128 * image.Height / image.Width));
                MemoryStream memoryStream = new MemoryStream();
                image.Save(memoryStream, new JpegEncoder());
                memoryStream.Seek(0, SeekOrigin.Begin);
                CloudStorage.Instance.UploadFile(blobPath + "thumbnail.jpeg", memoryStream);
            }
            Uri uri = CloudStorage.Instance.UploadFile(blobPath, file.OpenReadStream()).Result;
            

            Post post = FoxyFaceDbManager.Instance.PostRepository.Create(session.User.Value, title, description, uri.ToString());
            return Json(new
            {
                postId = post.Id
            });
        }
    }
}