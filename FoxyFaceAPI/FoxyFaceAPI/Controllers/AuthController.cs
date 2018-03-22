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
        public JsonResult Register(string username, string password, string email, string passwordRetype)
        {
            //Console.WriteLine(username + "/" + password + "/" + email);
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(passwordRetype))
            {
                return Json(new
                {
                    success = false,
                    error = ErrorObjects.ParametersAreNotValid
                });
            }
            if (password != passwordRetype)
            {
                return Json(new
                {
                    success = false,
                    error = ErrorObjects.PasswordsDontMatch
                });
            }
            
            if (FoxyFaceDbManager.GetNewConnection.UserRepository.FindByName(username) != null)
            {
                return Json(new
                {
                    success = false,
                    error = ErrorObjects.UsernameAlreadyExists
                });
            }
            if (FoxyFaceDbManager.GetNewConnection.UserRepository.FindByEmail(email) != null)
            {
                return Json(new
                {
                    success = false,
                    error = ErrorObjects.EmailAlreadyExists
                });
            }
            if (HasSpecialCharacter(username))
            {
                return Json(new
                {
                    success = false,
                    error = ErrorObjects.WrongUserName
                });
            }
            
            FoxyFaceDbManager.GetNewConnection.UserRepository.Create(username, password, email);
            return Login(username, password);
        }
        
        private static bool HasSpecialCharacter(string str) {
            return str.Any(c => !(c >= '0' && c <= '9' || c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z'));
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

            User user = FoxyFaceDbManager.GetNewConnection.UserRepository.FindByName(username);
            if (user == null)
            {
                return Json(new
                {
                    error = ErrorObjects.LoginError
                });
            }
            
            if (!user.IsPasswordCorrect(password))
            {
                return Json(new
                {
                    error = ErrorObjects.LoginError
                });
            }

            Session session = FoxyFaceDbManager.GetNewConnection.SessionRepository.Create(user);
            return Json(new
            {
                token = session.Token
            });
        }
        
        // POST api/auth/logout
        [HttpPost("logout")]
        public JsonResult Logout(string token)
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

            FoxyFaceDbManager.GetNewConnection.SessionRepository.Delete(session);
            return Json(new
            {
                success = true
            });
        }
        // POST api/auth/changePassword
        [HttpPost("changePassword")]
        public JsonResult ChangePassword(string oldPassword, string newPassword, string token)
        {
            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(token))
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

            if (!session.User.Value.IsPasswordCorrect(oldPassword))
            {
                return Json(new
                {
                    success = false,
                    error = ErrorObjects.PasswordError
                });
            }
            
            FoxyFaceDbManager.GetNewConnection.UserRepository.ChangePassword(session.User.Value, newPassword);
            return Json(new
            {
                success = true
            });
        }
    }
}