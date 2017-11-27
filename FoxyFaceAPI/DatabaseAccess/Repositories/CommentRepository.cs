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

        public Comment Create(int postId, int userId, string text)
        {
            int id = (int) FoxyFaceDb.ExecuteNonQuery("INSERT INTO comment VALUES (@postId, @userId, @text, @date)", 
                new MySqlParameter("postID", postId), new MySqlParameter("userId", userId), 
                new MySqlParameter("text", text));
            
            return FindById(id);
        }
        
        public Comment FindById(int commentId)
        {
            DataTable resultDataTable = FoxyFaceDb.ExecuteReader("SELECT * FROM comment WHERE Comment_id = @commentId",
                new MySqlParameter("commentId", commentId));

            return new Comment(Convert.ToInt32(resultDataTable.Rows[0]["Comment_id"]), Convert.ToInt32(resultDataTable.Rows[0]["post_id"]), 
                Convert.ToInt32(resultDataTable.Rows[0]["user_id"]), (string)resultDataTable.Rows[0]["text"], (DateTime)resultDataTable.Rows[0]["date"]);
        }

        public List<Comment> FindByPost(int postId)
        {
            var comments = new List<Comment>();
            DataTable resultDataTable = FoxyFaceDb.ExecuteReader("SELECT * FROM comment WHERE post_id = @postid",
                new MySqlParameter("postid", postId));
            foreach (DataRow row in resultDataTable.Rows)
            {
                comments.Add(new Comment(Convert.ToInt32(row["Comment_id"]), Convert.ToInt32(row["post_id"]), 
                    Convert.ToInt32(row["user_id"]), (string)row["text"], (DateTime)row["date"]));
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