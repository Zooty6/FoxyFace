using System;
using System.Data;
using DatabaseAccess.Model;
using MySql.Data.MySqlClient;

namespace DatabaseAccess.Repositories
{
    public class SessionRepository : Repository
    {
        public SessionRepository(FoxyFaceDB foxyFaceDb) : base(foxyFaceDb)
        {
        }

        public Session Create(User user)
        {
            return Create(user.Id);
        }

        public Session Create(int userId)
        {
            string generatedToken = Session.generateToken();
            long sessionId = FoxyFaceDb.ExecuteNonQuery("INSERT INTO session (user_id, token) VALUES (@userId, @token)",
                new MySqlParameter("userId", userId), new MySqlParameter("token", generatedToken));
            return new Session((int)sessionId, userId, generatedToken);
        }

        public Session FindById(int sessionId)
        {
            DataTable resultDataTable = FoxyFaceDb.ExecuteReader("SELECT * FROM session WHERE Session_id = @sessionId",
                new MySqlParameter("sessionId", sessionId));
            if (resultDataTable.Rows.Count == 1)
            {
                DataRow row = resultDataTable.Rows[0];
                return new Session(Convert.ToInt32(row["Session_id"]), Convert.ToInt32(row["user_id"]), (string)row["token"]);
            }
            return null;
        }

        public Session FindByToken(string token)
        {
            DataTable resultDataTable = FoxyFaceDb.ExecuteReader("SELECT * FROM session WHERE token = @token",
                new MySqlParameter("token", token));
            if (resultDataTable.Rows.Count == 1)
            {
                DataRow row = resultDataTable.Rows[0];
                return new Session(Convert.ToInt32(row["Session_id"]), Convert.ToInt32(row["user_id"]), (string)row["token"]);
            }
            return null;
        }
    }
}