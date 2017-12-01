using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseAccess;
using DatabaseAccess.Model;
using DatabaseAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace FoxyFaceAPI.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        // POST api/auth/regsiter
        [HttpPost("register")]
        public JsonResult Register(string username, string password, string email)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                return Json(new
                {
                    success = false,
                    error = ErrorObjects.ParametersAreNotValid
                });
            }
            
            // 1. check if user exists
            if (FoxyFaceDbManager.Instance.UserRepository.FindByName(username) != null)
            {
                // 2. if exists -> return success: false, error: user exists
                return Json(new
                {
                    success = false,
                    error = ErrorObjects.UsernameAlreadyExists
                });
            }
            if (FoxyFaceDbManager.Instance.UserRepository.FindByEmail(email) != null)
            {
                return Json(new
                {
                    success = false,
                    error = ErrorObjects.EmailAlreadyExists
                });
                // 2. if exists -> return success: false, error: user exists
            }
            // 3. if not -> insert user into database, return success: true
            FoxyFaceDbManager.Instance.UserRepository.Create(username, password, email);
            return Json(new
            {
                success = true
            });
            
        }
        
        // POST api/auth/login
        [HttpPost("login")]
        public JsonResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return Json(new
                {
                    success = false,
                    error = ErrorObjects.ParametersAreNotValid
                });
            }

            User user = FoxyFaceDbManager.Instance.UserRepository.FindByName(username);
            if (user == null)
            {
                return Json(new
                {
                    error = ErrorObjects.LoginError
                });
            }
            
            if (!user.CanLogIn(password))
            {
                return Json(new
                {
                    error = ErrorObjects.LoginError
                });
            }

            Session session = FoxyFaceDbManager.Instance.SessionRepository.Create(user);
            return Json(new
            {
                token = session.Token
            });
        }
        
        // POST api/auth/logout
        [HttpPost("logout")]
        public JsonResult Logout(string token)
        {
            Session session = FoxyFaceDbManager.Instance.SessionRepository.FindByToken(token);
            if (session == null)
            {
                return Json(new
                {
                    success = false,
                    error = ErrorObjects.TokenNotValid
                });
            }

            FoxyFaceDbManager.Instance.SessionRepository.Delete(session);
            return Json(new
            {
                success = true
            });
        }
    }
}