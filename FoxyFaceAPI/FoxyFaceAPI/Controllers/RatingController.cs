using DatabaseAccess;
using DatabaseAccess.Model;
using Microsoft.AspNetCore.Mvc;

namespace FoxyFaceAPI.Controllers
{
    [Route("api/[controller]")]
    public class RatingController : Controller
    {
        [HttpPost]
        public JsonResult Post(int postId, int rating, string token)
        {
            if (rating <= 0 || rating > 5 || string.IsNullOrEmpty(token))
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

            if (FoxyFaceDbManager.Instance.PostRepository.FindById(postId) == null)
            {
                return Json(new
                {
                    error = ErrorObjects.WrongPostId
                });
            }

            int ratingId = FoxyFaceDbManager.Instance.RatingRepository.Create(postId, session.User.Value.Id, rating).Id;
            return Json(new
            {
                ratingId
            });
        }
    }
}