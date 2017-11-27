using System;
using System.Collections.Generic;
using System.Data;
using DatabaseAccess.Model;
using MySql.Data.MySqlClient;

namespace DatabaseAccess.Repositories
{
    public class PostRepository : Repository
    {
        public PostRepository(FoxyFaceDB foxyFaceDb) : base(foxyFaceDb)
        {
        }

        public Post Create(int userId, string title, string description, string path)
        {
            long insertedId = FoxyFaceDb.ExecuteNonQuery("INSERT INTO post VALUES(@userid, @title, @desc, @path @date)", 
                new MySqlParameter("userid", userId), new MySqlParameter("title", title),
                new MySqlParameter("desc", description), new MySqlParameter("path", path), new MySqlParameter("date", DateTime.Now));
            return FindById((int) insertedId);
        }

        public void Delete(Post post)
        {
            Delete(post.Id);
        }

        public Post FindById(int id)
        {
            DataTable resultDataTable = FoxyFaceDb.ExecuteReader("SELECT * FROM post WHERE Post_id = @id", new MySqlParameter("id", id));
            if (resultDataTable.Rows.Count==0)
            {
                return null;
            }
            DataRow postRow = resultDataTable.Rows[0];
            return new Post((int) postRow["Post_id"], (int)postRow["user_id"], (string)postRow["title"], (string)postRow["description"],
                (string)postRow["path"], (DateTime)postRow["date"]);
        }

        public void UpdateTitle(string title, int id)
        {
            FoxyFaceDb.ExecuteNonQuery("UPDATE post SET title = @title WHERE Post_id = @id", new MySqlParameter("title", title), new MySqlParameter("id", id));
        }

        public void UpdateDescription(string description, int id)
        {
            FoxyFaceDb.ExecuteNonQuery("UPDATE post SET description = @desc WHERE Post_id = @id", new MySqlParameter("desc", description), new MySqlParameter("id", id));
        }

        public void Delete(int postId)
        {
            FoxyFaceDb.ExecuteNonQuery("DELETE FROM post WHERE id = @id", new MySqlParameter("id", postId));
        }
    }
}