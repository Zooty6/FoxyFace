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
        public SessionRepository SessionRepository { get; set; }

        private static string connectionString;
        
        private static FoxyFaceDbManager instance;

        public static FoxyFaceDbManager GetNewConnection
        {
            get
            {
                if (string.IsNullOrEmpty(connectionString))
                    throw new ArgumentException("Call FoxyFaceDbManager#Initialize first");

                return new FoxyFaceDbManager(connectionString);
            }
        }

        private FoxyFaceDbManager(string connectionString)
        {
            FoxyFaceDb = new FoxyFaceDB(connectionString);
            
            UserRepository = new UserRepository(FoxyFaceDb);
            RatingRepository = new RatingRepository(FoxyFaceDb);
            CommentRepository = new CommentRepository(FoxyFaceDb);
            PostRepository = new PostRepository(FoxyFaceDb);
            SessionRepository = new SessionRepository(FoxyFaceDb);
            
            FoxyFaceDb.Open();
        }

        public void Close()
        {
            if (FoxyFaceDb.IsOpen())
                FoxyFaceDb.Dispose();
        }

        public static void Initialize(string conString)
        {
            if (!string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Instance was already created");

            FoxyFaceDbManager.connectionString = conString;
        }
    }
}