using System;

namespace FoxyFaceAPI
{
    public static class ErrorObjects
    {
        // general - 0XX
        public static readonly ErrorObject ParametersAreNotValid = new ErrorObject(001, "Parameters aren't valid");
        public static readonly ErrorObject TokenNotValid = new ErrorObject(002, "Token is not valid");
        
        // register - 1XX
        public static readonly ErrorObject UsernameAlreadyExists = new ErrorObject(101, "Username already exists");
        public static readonly ErrorObject EmailAlreadyExists = new ErrorObject(102, "Email already exists");
        
        //login - 2xx
        public static readonly ErrorObject LoginError = new ErrorObject(201, "Username or password is wrong");
        
        //change password - 3xx
        public static readonly ErrorObject PasswordError = new ErrorObject(301, "Password is wrong");
    }

    public class ErrorObject
    {
        public string Description { get; }
        public int ErrorId { get; }

        public ErrorObject(int errorId, string description)
        {
            Description = description ?? throw new ArgumentNullException(nameof(description));
            ErrorId = errorId;
        }
    }
}