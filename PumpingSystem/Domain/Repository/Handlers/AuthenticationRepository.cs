using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace PumpingSystem.Domain.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private string _ConnectionString;

        public AuthenticationRepository() : this(Properties.Resources.ConnectionString) { }

        public AuthenticationRepository(string connectionString)
        {
            _ConnectionString = connectionString;
        }

        public void Insert(string username, string password, int timeout)
        {
            using (var conn = new SqlConnection(_ConnectionString))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    using (var cmd = new SqlCommand())
                    {
                        StringBuilder sql = new StringBuilder("insert into [dbo].[system_user](");
                        sql.Append(" creation_date,");
                        sql.Append(" user_name,");
                        sql.Append(" password");
                        sql.Append(") values (");
                        sql.Append(" CURRENT_TIMESTAMP,");
                        sql.Append(" @username,");
                        sql.Append(" @password");
                        sql.Append(")");

                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        cmd.Connection = conn;
                        cmd.CommandTimeout = timeout;
                        cmd.CommandText = sql.ToString();

                        if (cmd.ExecuteNonQuery() == 0)
                            throw new Exception("Error recording the record in the table.");
                    }
                }
                // TODO: Exception por falta de conexão
            }
        }

        public bool CheckIfItExistsByUsernameAndPassword(string username, string password, int timeout)
        {
            using (SqlConnection conn = new SqlConnection(_ConnectionString))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    using (var cmd = new SqlCommand())
                    {
                        StringBuilder sql = new StringBuilder("select");
                        sql.Append(" user_name");
                        sql.Append(" from [dbo].[system_user]");
                        sql.Append(" where user_name = @username");
                        sql.Append(" and password = @password");

                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        cmd.Connection = conn;
                        cmd.CommandTimeout = timeout;
                        cmd.CommandText = sql.ToString();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0)) return true;
                            }
                        }
                    }
                }
                // TODO: Exception por falta de conexão
            }
            return false;
        }
    }
}
