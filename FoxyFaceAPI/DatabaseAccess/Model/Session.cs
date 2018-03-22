using System;

namespace DatabaseAccess.Model
{
    public class Session
    {
        public int Id { get; set; }
        public Lazy<User> User { get; set; }
        public string Token { get; set; }

        public Session(User user)
        {
            User = new Lazy<User>(() => user);
            Token = generateToken();
        }

        public Session(int userId)
        {
            User = new Lazy<User>(() => FoxyFaceDbManager.GetNewConnection.UserRepository.FindById(userId));
            Token = generateToken();
        }

        public Session(int id, int userId, string token)
        {
            Id = id;
            User = new Lazy<User>(() => FoxyFaceDbManager.GetNewConnection.UserRepository.FindById(userId));
            Token = token ?? throw new ArgumentNullException(nameof(token));
        }

        public Session(int id, User user, string token)
        {
            Id = id;
            User = new Lazy<User>(() => user);
            Token = token ?? throw new ArgumentNullException(nameof(token));
        }

        public static string generateToken()
        {
            return Guid.NewGuid().ToString();
        }

        protected bool Equals(Session other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Session) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(User)}: {User}, {nameof(Token)}: {Token}";
        }
    }
}