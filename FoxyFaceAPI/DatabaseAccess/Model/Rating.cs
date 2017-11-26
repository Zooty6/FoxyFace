﻿using System;

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

        public Rating(int id, int postId, int userId)
        {
            Id = id;
            Post = new Lazy<Post>(() => FoxyFaceDbManager.Instance.PostRepository.GetPost(postId));
            User = new Lazy<User>(() => FoxyFaceDbManager.Instance.UserRepository.GetUser(userId));
        }
        
        public Rating(int id, Lazy<Post> post, Lazy<User> user)
        {
            Id = id;
            Post = post;
            User = user;
        }
        
        public Rating(int id, Post post, User user)
        {
            Id = id;
            Post = new Lazy<Post>(() => post);
            User = new Lazy<User>(() => user);
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