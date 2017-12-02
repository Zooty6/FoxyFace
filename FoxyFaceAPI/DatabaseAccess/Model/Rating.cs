using System;

namespace DatabaseAccess.Model
{
    public class Rating
    {
        public int Id { get; }
        public Lazy<Post> Post { get; }
        public Lazy<User> User { get; }

        private int stars;

        public int Stars
        {
            get => stars;
            set
            {
                if (value < 0 || value > 5)
                {
                    throw new ArgumentException("Stars can only be between 0 and 5");
                }
                stars = value;
            }
        }

        internal Rating(int id, int postId, int userId, int stars)
            : this(id, new Lazy<Post>(() => FoxyFaceDbManager.Instance.PostRepository.FindById(postId)),
                new Lazy<User>(() => FoxyFaceDbManager.Instance.UserRepository.FindById(userId)), stars)
        {
        }

        internal Rating(int id, Lazy<Post> post, Lazy<User> user, int stars)
        {
            Id = id;
            Post = post;
            User = user;
            Stars = stars;
        }

        internal Rating(int id, Post post, User user, int stars)
            : this(id, new Lazy<Post>(() => post), new Lazy<User>(() => user), stars)
        {
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Post)}: {Post}, {nameof(User)}: {User}, {nameof(Stars)}: {Stars}";
        }

        protected bool Equals(Rating other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Rating) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}