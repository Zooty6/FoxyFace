using System;

namespace FoxyFaceAPI
{
    public static class ErrorObjects
    {
        // general - 0XX
        public static readonly ErrorObject ParametersAreNotValid = new ErrorObject(001, "Parameters aren't valid");
        public static readonly ErrorObject TokenNotValid = new ErrorObject(002, "Token is not valid");
        
        // register - 10X
        public static readonly ErrorObject UsernameAlreadyExists = new ErrorObject(101, "Username already exists");
        public static readonly ErrorObject EmailAlreadyExists = new ErrorObject(102, "Email already exists");
        public static readonly ErrorObject WrongUserName = new ErrorObject(103, "Username can't contain special characters");
        
        //login - 11x
        public static readonly ErrorObject LoginError = new ErrorObject(111, "Username or password is wrong");
        
        //change password - 12x
        public static readonly ErrorObject PasswordError = new ErrorObject(121, "Password is wrong");
        
        // post - 20x
        public static readonly ErrorObject WrongPostId = new ErrorObject(201, "PostId is wrong");
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