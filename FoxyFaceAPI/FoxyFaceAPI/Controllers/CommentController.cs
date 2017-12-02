using DatabaseAccess;
using DatabaseAccess.Model;
using Microsoft.AspNetCore.Mvc;

namespace FoxyFaceAPI.Controllers
{
    [Route("api/[controller]")]
    public class CommentController : Controller
    {
        [HttpPost]
        public JsonResult Post(int postId, string text, string token)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(token))
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

            Comment comment = FoxyFaceDbManager.Instance.CommentRepository.Create(postId, session.User.Value.Id, text);
            return Json(new
            {
                commentId = comment.Id
            });
        }
    }
}