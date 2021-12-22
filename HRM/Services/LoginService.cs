using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Text;
using HRM.Models;
using System.Data;
using HRM.Common;
using System.Threading.Tasks;

namespace HRM.Services
{
    public class LoginService
    {
        private readonly string _connectionString;
        private readonly IConfiguration _config;
        public LoginService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("ConnectionString");
            _config = config;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Account Login(string userName, string password)
        {
            AccountService accountService = new AccountService(_config);
            return accountService.GetAccounts().SingleOrDefault(account => account.UserName == userName && account.Password == DBUtils.EncryptPassword(password));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Staff GetStaffNameByEmail(string email)
        {
            string sql = "SELECT Staff.Id, Staff.FirstName, Staff.LastName FROM Staff " +
                         "INNER JOIN Account ON Staff.Id = Account.StaffId " +
                         "WHERE Account.Email = @Email";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    conn.Open();

                    command.Parameters.AddWithValue("@Email", email);

                    var reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Staff staff = new Staff();
                            staff.Id = DBUtils.GetInt(reader, "Id");
                            staff.FirstName = DBUtils.GetString(reader, "FirstName").Trim();
                            staff.LastName = DBUtils.GetString(reader, "LastName").Trim();

                            return staff;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="staff"></param>
        /// <returns></returns>
        public int UpdatePasswordByEmail(string email, string password, Staff staff)
        {
            string sql = "UPDATE Account SET Password = @Password, UpdateId = @UpdateId, UpdateDate = @UpdateDate WHERE Email = @Email";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    conn.Open();

                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@UpdateId", staff.Id);
                    command.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                    var rows = command.ExecuteNonQuery();

                    Console.WriteLine("Update rows: " + rows);

                    conn.Close();

                    return rows;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <param name="expires"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public Task<int> UpdateRefreshTokenAsync(string refreshToken, DateTime expires, Account account)
        {
            string sql = "UPDATE Account SET RefreshToken = @RefreshToken, TokenExpired = @TokenExpired, UpdateId = @UpdateId, UpdateDate = @UpdateDate WHERE UserName = @UserName";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    conn.Open();

                    command.Parameters.AddWithValue("@RefreshToken", refreshToken);
                    command.Parameters.AddWithValue("@TokenExpired", expires);
                    command.Parameters.AddWithValue("@UpdateId", account.StaffId);
                    command.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                    command.Parameters.AddWithValue("@UserName", account.UserName);

                    var rows = command.ExecuteNonQueryAsync();

                    conn.Close();

                    return rows;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <param name="expires"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public int UpdateRefreshToken(string refreshToken, DateTime expires, Account account)
        {
            string sql = "UPDATE Account SET RefreshToken = @RefreshToken, TokenExpired = @TokenExpired, UpdateDate = @UpdateDate WHERE UserName = @UserName";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    conn.Open();

                    command.Parameters.AddWithValue("@RefreshToken", refreshToken);
                    command.Parameters.AddWithValue("@TokenExpired", expires);
                    command.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                    command.Parameters.AddWithValue("@UserName", account.UserName);

                    var rows = command.ExecuteNonQuery();

                    conn.Close();

                    return rows;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public Task<int> RevokeRefreshTokenAsync(string userName)
        {
            string sql = "UPDATE Account SET RefreshToken = NULL, TokenExpired = NULL  WHERE UserName = @UserName";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    conn.Open();

                    command.Parameters.AddWithValue("@UserName", userName);
                    var rows = command.ExecuteNonQueryAsync();

                    conn.Close();

                    return rows;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public int RevokeRefreshToken(string userName)
        {
            string sql = "UPDATE Account SET RefreshToken = NULL, TokenExpired = NULL  WHERE UserName = @UserName";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    conn.Open();

                    command.Parameters.AddWithValue("@UserName", userName);
                    var rows = command.ExecuteNonQuery();

                    conn.Close();

                    return rows;
                }
            }
        }
    }
}
