using HRM.Common;
using HRM.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Services
{
    public class AccessService
    {
        private readonly string _connectionString;
        public AccessService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("ConnectionString");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Access> GetAccesss()
        {
            List<Access> listAccess = new List<Access>();

            SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = conn;
            sqlCmd.CommandText = "SELECT * FROM Access";

            try
            {
                SqlDataReader reader = sqlCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Access access = new Access();
                        access.Id = DBUtils.GetInt(reader, "ID");
                        access.Name = DBUtils.GetString(reader, "Name");

                        listAccess.Add(access);
                    }
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return listAccess;
        }
    }
}
