using System;
using System.IO;
using System.Net.Security;

namespace DatabaseAccess.Model
{
    public class User
    {
        public int Id { get; }
        public string Username { get; }
        public string Password{ get; }
        public string Email{ get; }
        public string Salt { get; }

        internal User(string username, string password, string email)
        {
            PasswordHasher.Encrypt(out string encodedPassword, out string generatedSalt, password ?? throw new ArgumentNullException(nameof(password)));
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Password = encodedPassword;
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Salt = generatedSalt;
        }

        internal User(int id, string username, string password, string email)
        {
            Id = id;
            PasswordHasher.Encrypt(out string encodedPassword, out string generatedSalt, password ?? throw new ArgumentNullException(nameof(password)));
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Password = encodedPassword;
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Salt = generatedSalt;
        }

        internal User(int id, string username, string password, string email, string salt)
        {
            Id = id;
            Salt = salt?? throw new ArgumentNullException(nameof(salt));
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            Email = email ?? throw new ArgumentNullException(nameof(email));
        }

        public bool IsPasswordCorrect(string password)
        {
            return PasswordHasher.Compare(Password, Salt, password);
        }

        protected bool Equals(User other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((User) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Username)}: {Username}, {nameof(Password)}: {Password}, {nameof(Email)}: {Email}";
        }
    }
}