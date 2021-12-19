using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRM.Models;
using System.Data.SqlClient;
using HRM.Common;

namespace HRM.Services
{
    public class PositionService
    {
        private readonly string _connectionString;

        public PositionService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("ConnectionString");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Position> GetPositions()
        {
            List<Position> listPos = new List<Position>();

            string sql = "SELECT * FROM Position";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    conn.Open();

                    var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Position pos = new Position();
                            pos.ID = DBUtils.GetInt(reader, "Id");
                            pos.Name = DBUtils.GetString(reader, "Name");
                            listPos.Add(pos);
                        }
                    }
                }
            }

            return listPos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDBNull(object value)
        {
            return DBNull.Value.Equals(value);
        }
    }
}
