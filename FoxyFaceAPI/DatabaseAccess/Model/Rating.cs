using System;

namespace DatabaseAccess.Model
{
    public class Rating
    {
        public int Id { get; }
        public Post Post { get; set; }
        public User User { get; set; }

        private int stars;
        public int Stars
        {
            get => Stars;
            set
            {
                if (value < 0 || value > 5)
                {
                    throw new ArgumentException("Stars can only be between 0 and 5");
                }
                stars = value;
            }
        }

        public Rating(int id, Post post, User user)
        {
            Id = id;
            Post = post;
            User = user;
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
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Rating) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}