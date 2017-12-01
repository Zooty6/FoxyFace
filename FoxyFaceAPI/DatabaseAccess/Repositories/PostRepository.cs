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
        
        public Post Create(User user, string title, string description, string path)
        {
            return Create(user.Id, title, description, path);
        }

        public Post Create(int userId, string title, string description, string path)
        {
            long insertedId = FoxyFaceDb.ExecuteNonQuery("INSERT INTO post (user_id, title, description, path) VALUES(@userid, @title, @desc, @path)", 
                new MySqlParameter("userid", userId), new MySqlParameter("title", title),
                new MySqlParameter("desc", description), new MySqlParameter("path", path));
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
            Console.WriteLine(postRow["date"]);
            return new Post(Convert.ToInt32(postRow["Post_id"]), Convert.ToInt32(postRow["user_id"]), (string)postRow["title"], (string)postRow["description"],
                (string)postRow["path"], DateTimeUtils.ConverTo(postRow["date"]));
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