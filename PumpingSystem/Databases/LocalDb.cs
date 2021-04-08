using System;
using System.Data.SqlClient;
using System.Data;

namespace PumpingSystem.Databases
{
    public class LocalDb
    {
        private string _ConnectionString;

        public LocalDb(string connectionString)
        {
            _ConnectionString = connectionString;
        }

        public void Command(string sql, int timeout)
        {
            using (SqlConnection conn = new SqlConnection(_ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandTimeout = timeout;
                if (cmd.ExecuteNonQuery() == 0)
                    throw new Exception("Error recording the record in the table.");
            }
        }

        public DataTable Query(string sql, int timeout)
        {
            using (SqlConnection conn = new SqlConnection(_ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandTimeout = timeout;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                        return GetDataTable(reader);
                }
            }
            return null;
        }

        private DataTable GetDataTable(IDataReader reader)
        {
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }
    }
}
