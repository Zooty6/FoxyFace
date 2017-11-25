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
        
        
        private FoxyFaceDbManager(string connectionString)
        {
            foxyFaceDb = new FoxyFaceDB(connectionString);
            UserRepository = new UserRepository(foxyFaceDb);
            RatingRepository = new RatingRepository(foxyFaceDb);
            CommentRepository = new CommentRepository(foxyFaceDb);
            PostRepository = new PostRepository(foxyFaceDb);
        }
    }
}