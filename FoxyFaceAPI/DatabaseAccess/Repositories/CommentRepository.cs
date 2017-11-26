using System;
using System.Collections.Generic;
using System.Data;
using DatabaseAccess.Model;
using MySql.Data.MySqlClient;

namespace DatabaseAccess.Repositories
{
    public class CommentRepository : Repository 
    {
        public CommentRepository(FoxyFaceDB foxyFaceDb) : base(foxyFaceDb)
        {
        }

        public void Save(Comment comment)
        {
            FoxyFaceDb.ExecuteNonQuery("INSERT INTO comment VALUES (@postId, @userId, @text, @date)", 
                new MySqlParameter("postID", comment.Post.Value.Id), new MySqlParameter("userId", comment.User.Value.Id), 
                new MySqlParameter("text", comment.Text), new MySqlParameter("date", comment.Date));
        }

        public List<Comment> FindByPost(int postId)
        {
            var comments = new List<Comment>();
            DataTable resultDataTable = FoxyFaceDb.ExecuteReader("SELECT * FROM comment WHERE post_id = @postid",
                new MySqlParameter("postid", postId));
            foreach (DataRow row in resultDataTable.Rows)
            {
                comments.Add(new Comment((int)row["Comment_id"], (int)row["post_id"], (int)row["user_id"], (string)row["text"], (DateTime)row["date"]));
            }

            return comments;
        }

        public void UpdateText(string newText, int id)
        {
            FoxyFaceDb.ExecuteNonQuery("UPDATE comment SET text = @text WHERE Comment_id = @id", 
                new MySqlParameter("text", newText), new MySqlParameter("id", id));
        }

        public void Delete(Comment comment)
        {
            FoxyFaceDb.ExecuteNonQuery("DELETE FROM comment WHERE Comment_id = @id", new MySqlParameter("id", comment.Id));
        }
    }
}