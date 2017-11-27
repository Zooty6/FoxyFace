

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
            PasswordHasher.Encrypt(out byte[] encodedPassword, out byte[] generatedSalt, password);
            
            FoxyFaceDb.ExecuteNonQuery("UPDATE user SET password = @password, salt = @salt WHERE id = @id", new MySqlParameter("password", Convert.ToBase64String(encodedPassword)), new MySqlParameter("salt", Convert.ToBase64String(generatedSalt)), new MySqlParameter("id", id));
        }

        public User Create(string username, string unencryptedPassword, string email)
        {
            PasswordHasher.Encrypt(out byte[] encodedPassword, out byte[] generatedSalt, unencryptedPassword);
            
            int id = (int) FoxyFaceDb.ExecuteNonQuery("INSERT INTO user VALUES(@name, @password, @email, @salt)", new MySqlParameter("name", username), new MySqlParameter("password", encodedPassword), new MySqlParameter("email", email) ,new MySqlParameter("salt", generatedSalt));

            return FindById(id);
        }

        public User FindById(int id)
        {
            DataTable resultTable = FoxyFaceDb.ExecuteReader("SELECT * FROM user WHERE id = @id", new MySqlParameter("id", id));
            if (resultTable.Rows.Count == 0)
                return null;
            return new User((int)resultTable.Rows[0]["user_id"], (string)resultTable.Rows[0]["username"], (string)resultTable.Rows[0]["password"], (string)resultTable.Rows[0]["email"], (string)resultTable.Rows[0]["salt"]);
        }
    }
}