﻿using System.Collections.Generic;
using DatabaseAccess;
using DatabaseAccess.Model;
using Microsoft.AspNetCore.Mvc;

namespace FoxyFaceAPI.Controllers
{
    [Route("api/[controller]")]
    public class PostController : Controller
    {
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
        public JsonResult Post(string title, string description, object file, string token)
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

            Post post = FoxyFaceDbManager.Instance.PostRepository.Create(session.User.Value, title, description, path);
            return Json(new
            {
                postId = post.Id
            });
        }
    }
}