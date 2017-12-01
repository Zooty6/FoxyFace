

using System;
using System.ComponentModel.Design;
using System.Data;
using System.Text;
using DatabaseAccess.Model;
using MySql.Data.MySqlClient;

namespace DatabaseAccess.Repositories
{
    public class UserRepository : Repository
    {
        public UserRepository(FoxyFaceDB foxyFaceDb) : base(foxyFaceDb)
        {
        }
        
        public void ChangePassword(User user, string password)
        {
            ChangePassword(user.Id, password);
        }
        
        public void ChangePassword(int id, string password)
        {
            PasswordHasher.Encrypt(out string encodedPassword, out string generatedSalt, password);
            
            FoxyFaceDb.ExecuteNonQuery("UPDATE user SET password = @password, salt = @salt WHERE User_id = @id", new MySqlParameter("password", encodedPassword), new MySqlParameter("salt", generatedSalt), new MySqlParameter("id", id));
        }

        public User Create(string username, string unencryptedPassword, string email)
        {
            PasswordHasher.Encrypt(out string encodedPassword, out string generatedSalt, unencryptedPassword);
            
            int id = (int) FoxyFaceDb.ExecuteNonQuery("INSERT INTO user (username, password, email, salt) VALUES(@name, @password, @email, @salt)", new MySqlParameter("name", username), new MySqlParameter("password", encodedPassword), new MySqlParameter("email", email) ,new MySqlParameter("salt", generatedSalt));

            return FindById(id);
        }

        public User FindByName(string user)
        {
            DataTable resultTable = FoxyFaceDb.ExecuteReader("SELECT * FROM user WHERE username = @username",
                new MySqlParameter("username", user));
            if (resultTable.Rows.Count == 1)
            {
                return new User(Convert.ToInt32(resultTable.Rows[0]["user_id"]), (string)resultTable.Rows[0]["username"], 
                    (string)resultTable.Rows[0]["password"], (string)resultTable.Rows[0]["email"], (string)resultTable.Rows[0]["salt"]);
            }
            return null;
        }

        public User FindByEmail(string email)
        {
            DataTable resultTable = FoxyFaceDb.ExecuteReader("SELECT * FROM user WHERE email = @email",
                new MySqlParameter("email", email));
            if (resultTable.Rows.Count == 1)
            {
                return new User(Convert.ToInt32(resultTable.Rows[0]["user_id"]), (string)resultTable.Rows[0]["username"], 
                    (string)resultTable.Rows[0]["password"], (string)resultTable.Rows[0]["email"], (string)resultTable.Rows[0]["salt"]);
            }
            return null;
        }
        
        public User FindById(int id)
        {
            DataTable resultTable = FoxyFaceDb.ExecuteReader("SELECT * FROM user WHERE User_id = @id", new MySqlParameter("id", id));
            if (resultTable.Rows.Count == 0)
                return null;
            return new User(Convert.ToInt32(resultTable.Rows[0]["User_id"]), (string)resultTable.Rows[0]["username"], (string)resultTable.Rows[0]["password"], (string)resultTable.Rows[0]["email"], (string)resultTable.Rows[0]["salt"]);
        }
    }
}