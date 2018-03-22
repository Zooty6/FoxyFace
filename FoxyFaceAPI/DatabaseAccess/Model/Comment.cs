using System;
using Google.Protobuf.WellKnownTypes;

namespace DatabaseAccess.Model
{
    public class Comment
    {
        public int Id { get; }
        public Lazy<Post> Post { get; }
        public Lazy<User> User { get; }
        public string Text { get; set; }
        public DateTime Date { get; }

        internal Comment(int id, int postId, int userId, string text, DateTime date = new DateTime())
            : this(id, new Lazy<Post>(() => FoxyFaceDbManager.GetNewConnection.PostRepository.FindById(postId)),
                new Lazy<User>(() => FoxyFaceDbManager.GetNewConnection.UserRepository.FindById(userId)), text, date)
        {
        }

        internal Comment(Lazy<Post> post, Lazy<User> user, string text, DateTime date)
        {
            Post = post ?? throw new ArgumentNullException(nameof(post));
            User = user ?? throw new ArgumentNullException(nameof(user));
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Date = date;
        }
        
        internal Comment( int postId, int userId, string text, DateTime date = new DateTime())
            : this(new Lazy<Post>(() => FoxyFaceDbManager.GetNewConnection.PostRepository.FindById(postId)),
                new Lazy<User>(() => FoxyFaceDbManager.GetNewConnection.UserRepository.FindById(userId)), text, date)
        {
        }

        internal Comment(int id, Lazy<Post> post, Lazy<User> user, string text, DateTime date = new DateTime())
        {
            Id = id;
            Post = post;
            User = user;
            Text = text;
            Date = date;
        }

        internal Comment(int id, Post post, User user, string text, DateTime date = new DateTime())
            : this(id, new Lazy<Post>(() => post), new Lazy<User>(() => user), text, date)
        {
        }

        public override string ToString() =>
            $"{nameof(Id)}: {Id}, {nameof(Post)}: {Post}, {nameof(User)}: {User}, {nameof(Text)}: {Text}, {nameof(Date)}: {Date}";

        protected bool Equals(Comment other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Comment) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}