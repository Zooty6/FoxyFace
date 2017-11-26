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

        public List<Rating> FindById(int postId)
        {
            List<Rating> rates = new List<Rating>();
            DataTable executeReader = FoxyFaceDb.ExecuteReader(
                "SELECT * FROM rating WHERE post_id = @pid",
                new MySqlParameter("pid", postId));
            
            
            foreach (DataRow row in executeReader.Rows)
            {
                rates.Add(new Rating((int)row["Rating_id"], (int)row["post_id"], (int)row["user_id"])); 
            }
            
            return rates;
        }
    }
}