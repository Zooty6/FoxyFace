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

        public Comment(int id, int postId, int userId, string text, DateTime date = new DateTime())
            : this(id, new Lazy<Post>(() => FoxyFaceDbManager.Instance.PostRepository.GetPost(postId)),
                new Lazy<User>(() => FoxyFaceDbManager.Instance.UserRepository.GetUser(userId)), text, date)
        {
        }

        public Comment(int id, Lazy<Post> post, Lazy<User> user, string text, DateTime date = new DateTime())
        {
            Id = id;
            Post = post;
            User = user;
            Text = text;
            Date = date;
        }

        public Comment(int id, Post post, User user, string text, DateTime date = new DateTime())
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