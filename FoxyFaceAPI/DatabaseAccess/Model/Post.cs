using System;

namespace DatabaseAccess.Model
{
    public class Post
    {
        public int Id { get; }
        public Lazy<User> User { get; }
        public string Title { get; }
        public string Description { get; set; }
        public string Path { get; }
        public DateTime Date { get; }

        internal Post(int id, int userId, string title, string description, string path, DateTime date = new DateTime())
            : this(id, new Lazy<User>(() => FoxyFaceDbManager.Instance.UserRepository.FindById(userId)), title,
                description, path, date)
        {
        }

        internal Post(int id, Lazy<User> user, string title, string description, string path,
            DateTime date = new DateTime())
        {
            Id = id;
            User = user;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Path = path ?? throw new ArgumentNullException(nameof(path));
            Date = date;
        }

        internal Post(int id, User user, string title, string description, string path, DateTime date = new DateTime())
            : this(id, new Lazy<User>(() => user), title, description, path, date)
        {
        }

        protected bool Equals(Post other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Post) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override string ToString()
        {
            return
                $"{nameof(Id)}: {Id}, {nameof(User)}: {User}, {nameof(Title)}: {Title}, {nameof(Description)}: {Description}, {nameof(Path)}: {Path}, {nameof(Date)}: {Date}";
        }
    }
}