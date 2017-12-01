using System;

namespace FoxyFaceAPI
{
    public static class ErrorObjects
    {
        // general - 0XX
        public static readonly ErrorObject ParametersAreNotValid = new ErrorObject(001, "Parameters aren't valid");
        
        // register - 1XX
        public static readonly ErrorObject UsernameAlreadyExists = new ErrorObject(101, "Username already exists");
        public static readonly ErrorObject EmailAlreadyExists = new ErrorObject(102, "Email already exists");
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