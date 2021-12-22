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
    public class ChangePasswordService
    {
        private readonly string _connectionString;

        public ChangePasswordService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("ConnectionString");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public int ChangePassword(Account account)
        {
            string sql = "UPDATE Account SET Password = @Password WHERE  UserName = @UserName";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    command.Parameters.AddWithValue("@Password", DBUtils.EncryptPassword(account.Password));
                    command.Parameters.AddWithValue("@Username", account.UserName);
                    var rows = command.ExecuteNonQuery();

                    conn.Close();

                    return rows;
                }
            }
        }

        /// <summary>
        /// Check Current Password Valid
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string CheckPassword(Account account)
        {
            string curPassword = string.Empty;

            string sql = "SELECT Password FROM Account WHERE UserName = @UserName";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    conn.Open();

                    command.Parameters.AddWithValue("@UserName", account.UserName);

                    var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            curPassword = DBUtils.GetString(reader, "Password").Trim();
                        }
                    }

                    return curPassword;
                }
            }
        }
    }
}

