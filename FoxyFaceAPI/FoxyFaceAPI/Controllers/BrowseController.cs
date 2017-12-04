using System;
using System.Collections.Generic;
using DatabaseAccess;
using DatabaseAccess.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoxyFaceAPI.Controllers
{
    [Route("api/[controller]")]
    public class BrowseController : Controller
    {
        private Random random;

        public BrowseController()
        {
            random = new Random();
        }
        
        [HttpGet]
        public JsonResult Get(int offset, int amount, string orderBy, string order, string token)
        {
            if (string.IsNullOrEmpty(orderBy) || string.IsNullOrEmpty(token) || 
                offset < 0 || amount < 0 || amount > 50 || orderBy != "date" || (order != "asc" && order != "desc"))
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
            
            
            List<Post> posts = FoxyFaceDbManager.Instance.PostRepository.FindPosts(offset, amount, orderBy);
            return Json(new
            {
                posts
            });
        }
    }
}