﻿using System;
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

        public void Create(Post post, User user, int stars)
        {
            Create(post.Id, user.Id, stars);
        }

        public Rating Create(int postId, int userId, int stars)
        {
            int id = (int) FoxyFaceDb.ExecuteNonQuery(
                "INSERT INTO rating (post_id, user_id, stars) VALUES(@pid, @uid, @stars)",
                new MySqlParameter("pid", postId), new MySqlParameter("uid", userId),
                new MySqlParameter("stars", stars));

            return FindById(id);
        }

        public Rating FindById(int ratingId)
        {
            DataTable executeReader = FoxyFaceDb.ExecuteReader(
                "SELECT * FROM rating WHERE Rating_id = @rid",
                new MySqlParameter("rid", ratingId));

            return new Rating(Convert.ToInt32(executeReader.Rows[0]["Rating_id"]),
                Convert.ToInt32(executeReader.Rows[0]["post_id"]), Convert.ToInt32(executeReader.Rows[0]["user_id"]),
                Convert.ToInt32(executeReader.Rows[0]["stars"]));
        }

        public List<Rating> FindByPostId(int postId)
        {
            List<Rating> rates = new List<Rating>();
            DataTable executeReader = FoxyFaceDb.ExecuteReader(
                "SELECT * FROM rating WHERE post_id = @pid",
                new MySqlParameter("pid", postId));


            foreach (DataRow row in executeReader.Rows)
            {
                rates.Add(new Rating(Convert.ToInt32(row["Rating_id"]), Convert.ToInt32(row["post_id"]),
                    Convert.ToInt32(row["user_id"]), Convert.ToInt32(row["stars"])));
            }

            return rates;
        }
    }
}