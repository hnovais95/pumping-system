using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using PumpingSystem.Common;
using PumpingSystem.Common.Utilities;

namespace PumpingSystem.Server.Repository
{
    public class ProcessChartDao
    {
        private string _ConnectionString;

        public ProcessChartDao() : this(Properties.Resources.ConnectionString) { }
        public ProcessChartDao(string connectionString)
        {
            _ConnectionString = connectionString;
        }

        public void Insert(ProcessChart processChart, int timeout)
        {
            using (var conn = new SqlConnection(_ConnectionString))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    using (var cmd = new SqlCommand())
                    {
                        StringBuilder sql = new StringBuilder("insert into process_chart(");
                        sql.Append(" creation_date");
                        sql.Append(", process_chart");
                        sql.Append(") values (");
                        sql.Append(" CURRENT_TIMESTAMP");
                        sql.Append(", @processchart");
                        sql.Append(")");

                        cmd.Parameters.AddWithValue("@processchart", Utilities.Compress(Utilities.Serialize(processChart)));

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

        public List<ProcessChart> GetByPeriod(DateTime startDate, DateTime endDate, int timeout)
        {
            using (SqlConnection conn = new SqlConnection(_ConnectionString))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    using (var cmd = new SqlCommand())
                    {
                        StringBuilder sql = new StringBuilder("select");
                        sql.Append(" process_chart");
                        sql.Append(" from process_chart");
                        sql.Append(" where creation_date between @startdate and @enddate");

                        cmd.Parameters.AddWithValue("@startdate", startDate);
                        cmd.Parameters.AddWithValue("@enddate", endDate);

                        cmd.Connection = conn;
                        cmd.CommandTimeout = timeout;
                        cmd.CommandText = sql.ToString();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            List<ProcessChart> processCharts = new List<ProcessChart>();
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0)) processCharts.Add(GetProcessoChart(reader));
                            }
                            
                            if (processCharts.Count > 0)
                            {
                                return processCharts;
                            }
                        }
                    }
                }
                // TODO: Exception por falta de conexão
            }
            return null;
        }

        private ProcessChart GetProcessoChart(IDataReader reader)
        {
            Byte[] serializedProcessChart = (Byte[])reader.GetValue(0);
            return (ProcessChart)Utilities.ObjDeserialize(Utilities.Decompress(serializedProcessChart));
        }
    }
}
