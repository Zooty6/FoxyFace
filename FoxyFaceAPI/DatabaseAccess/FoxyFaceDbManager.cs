using System;
using DatabaseAccess.Repositories;

namespace DatabaseAccess
{
    public class FoxyFaceDbManager
    {
        public FoxyFaceDB FoxyFaceDb { get; }
        public UserRepository UserRepository { get; }
        public RatingRepository RatingRepository { get; }
        public CommentRepository CommentRepository { get; }
        public PostRepository PostRepository { get; }

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
            FoxyFaceDb = new FoxyFaceDB(connectionString);
            
            UserRepository = new UserRepository(FoxyFaceDb);
            RatingRepository = new RatingRepository(FoxyFaceDb);
            CommentRepository = new CommentRepository(FoxyFaceDb);
            PostRepository = new PostRepository(FoxyFaceDb);
            
            FoxyFaceDb.Open();
        }

        public void Close()
        {
            if (FoxyFaceDb.IsOpen())
                FoxyFaceDb.Dispose();
        }

        public static FoxyFaceDbManager Initialize(string connectionString)
        {
            if (instance != null)
                throw new ArgumentException("Instance was already created");

            return instance = new FoxyFaceDbManager(connectionString);
        }
    }
}