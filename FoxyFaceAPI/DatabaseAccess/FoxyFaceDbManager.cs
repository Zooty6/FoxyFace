using System;
using DatabaseAccess.Repositories;

namespace DatabaseAccess
{
    public class FoxyFaceDbManager
    {
        private FoxyFaceDB foxyFaceDb;
        public  UserRepository UserRepository{ get; }
        public  RatingRepository RatingRepository{ get; }
        public CommentRepository CommentRepository{ get; }
        public PostRepository PostRepository{ get; }

        private static FoxyFaceDbManager instance;

        public static FoxyFaceDbManager Instance
        {
            get
            {
                if (instance == null)
                    throw new ArgumentException("Call FoxyFaceDbManager#Initialize first");

                return instance;
            }
        }

        private FoxyFaceDbManager(string connectionString)
        {
            foxyFaceDb = new FoxyFaceDB(connectionString);
            UserRepository = new UserRepository(foxyFaceDb);
            RatingRepository = new RatingRepository(foxyFaceDb);
            CommentRepository = new CommentRepository(foxyFaceDb);
            PostRepository = new PostRepository(foxyFaceDb);
        }

        public static FoxyFaceDbManager Initialize(string connectionString)
        {
            if (instance != null)
                throw new ArgumentException("Instance was already created");
            
            instance = new FoxyFaceDbManager(connectionString);
        }
    }
}