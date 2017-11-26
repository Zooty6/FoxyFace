using System.Collections.Generic;
using System.Data;
using DatabaseAccess.Model;
using MySql.Data.MySqlClient;

namespace DatabaseAccess.Repositories
{
    public class RatingRepository : Repository
    {
        public RatingRepository(FoxyFaceDB foxyFaceDb) : base(foxyFaceDb)
        {
        }

        public void Create(Rating rating)
        {
            FoxyFaceDb.ExecuteNonQuery("INSERT INTO rating VALUES(post_id = @pid, user_id = @uid, stars = @stars)", 
                new MySqlParameter("pid", rating.Post.Value.Id), new MySqlParameter("uid", rating.User.Value.Id), new MySqlParameter("stars", rating.Stars));
        }

        public List<Rating> GetRating(int postId)
        {
            Post post = null;
            List<User> users = new List<User>();
            List<Rating> rates = new List<Rating>();
            DataTable executeReader = FoxyFaceDb.ExecuteReader(
                "SELECT * FROM rating WHERE rating.Post_id = @pid",
                new MySqlParameter("pid", postId));
            if (executeReader.Rows.Count != 0)
                post = PostRepository.getPost((int)executeReader.Rows[0]["post_id"]);
            
            //TODO laizy load users
            foreach (DataRow row in executeReader.Rows)
            {
                rates.Add(new Rating(row["Rating.Rating_id"], post, users)); 
            }
            
            return rates;
        }
    }
}