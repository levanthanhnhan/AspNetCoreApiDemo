using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using HRM.Models;
using HRM.Common;
using System.Data;
using System.Text;

namespace HRM.Services
{
    public class AccountService
    {
        private readonly string _connectionString;

        public AccountService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("ConnectionString");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Account> GetAccounts()
        {
            List<Account> listAccount = new List<Account>();

            string sql = "SELECT * FROM Account";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    conn.Open();

                    var reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Account account = new Account();
                            account.UserName = DBUtils.GetString(reader, "UserName").Trim();
                            account.Password = DBUtils.GetString(reader, "Password").Trim();
                            account.StaffId = DBUtils.GetInt(reader, "StaffId");
                            account.RoleId = DBUtils.GetInt(reader, "RoleId");
                            account.Email = DBUtils.GetString(reader, "Email").Trim();
                            account.CreateDate = DBUtils.GetDateTime(reader, "CreateDate");
                            account.UpdateDate = DBUtils.GetDateTime(reader, "UpdateDate");
                            account.RefreshToken = DBUtils.GetString(reader, "RefreshToken");
                            account.TokenExpired = DBUtils.GetDateTime(reader, "TokenExpired");

                            listAccount.Add(account);
                        }
                    }
                }
            }

            return listAccount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public int UpdateAccount(Account account)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE ACCOUNT ");
            strSql.Append("   SET USERNAME = @USERNAME ");
            strSql.Append("      ,PASSWORD = @PASSWORD ");
            strSql.Append("      ,STAFFID = @STAFFID ");
            strSql.Append("      ,ROLEID = @ROLEID ");
            strSql.Append("      ,CREATEDATE = @CREATEDATE ");
            strSql.Append("      ,UPDATEDATE = @UPDATEDATE ");
            strSql.Append("      ,EMAIL = @EMAIL ");
            strSql.Append(" WHERE USERNAME = @USERNAME ");
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand updateCommand = conn.CreateCommand();
                SqlTransaction tran;
                tran = conn.BeginTransaction();
                updateCommand.Connection = conn;
                updateCommand.Transaction = tran;
                updateCommand.CommandText = strSql.ToString();
                int upd = 0;
                try
                {
                    updateCommand.Parameters.Add("@USERNAME", SqlDbType.NVarChar).Value = account.UserName;
                    updateCommand.Parameters.Add("@EMAIL", SqlDbType.NVarChar).Value = account.Email;
                    updateCommand.Parameters.Add("@PASSWORD", SqlDbType.NVarChar).Value = DBUtils.EncryptPassword(account.Password);
                    updateCommand.Parameters.Add("@STAFFID", SqlDbType.Int).Value = account.StaffId;
                    updateCommand.Parameters.Add("@ROLEID", SqlDbType.Int).Value = account.RoleId;
                    updateCommand.Parameters.Add("@CREATEDATE", SqlDbType.DateTime).Value = DateTime.Now;
                    updateCommand.Parameters.Add("@UPDATEDATE", SqlDbType.DateTime).Value = DateTime.Now;
                    upd = updateCommand.ExecuteNonQuery();
                    tran.Commit();
                }
                catch (SqlException Ex)
                {
                    tran.Rollback();
                    throw Ex;
                }
                finally
                {
                    conn.Close();
                }

                return upd;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newAccount"></param>
        public void CreateAccount(Account newAccount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO Account ");
            strSql.Append(" (");
            strSql.Append("     UserName ");
            strSql.Append("     , Email ");
            strSql.Append("     , Password ");
            strSql.Append("     , StaffId ");
            strSql.Append("     , RoleId ");
            strSql.Append("     , CreateDate ");
            strSql.Append("     , UpdateDate ");
            strSql.Append(" ) VALUES ");
            strSql.Append(" (");
            strSql.Append("     @USERNAME ");
            strSql.Append("     , @EMAIL ");
            strSql.Append("     , @PASSWORD ");
            strSql.Append("     , @STAFFID ");
            strSql.Append("     , @ROLEID ");
            strSql.Append("     , @CREATEDATE ");
            strSql.Append("     , @UPDATEDATE ");
            strSql.Append(" )");

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand updateCommand = conn.CreateCommand();
                SqlTransaction tran;
                tran = conn.BeginTransaction();
                updateCommand.Connection = conn;
                updateCommand.Transaction = tran;
                updateCommand.CommandText = strSql.ToString();
                try
                {
                    updateCommand.Parameters.Add("@USERNAME", SqlDbType.NVarChar).Value = newAccount.UserName;
                    updateCommand.Parameters.Add("@EMAIL", SqlDbType.NVarChar).Value = newAccount.Email;
                    updateCommand.Parameters.Add("@PASSWORD", SqlDbType.NVarChar).Value = DBUtils.EncryptPassword(newAccount.Password);
                    updateCommand.Parameters.Add("@STAFFID", SqlDbType.Int).Value = newAccount.StaffId;
                    updateCommand.Parameters.Add("@ROLEID", SqlDbType.Int).Value = newAccount.RoleId;
                    updateCommand.Parameters.Add("@CREATEDATE", SqlDbType.DateTime).Value = DateTime.Now;
                    updateCommand.Parameters.Add("@UPDATEDATE", SqlDbType.DateTime).Value = DateTime.Now;
                    updateCommand.ExecuteNonQuery();
                    tran.Commit();
                }
                catch (SqlException Ex)
                {
                    tran.Rollback();
                    throw Ex;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public Account FindAccountByUserName(string userName)
        {
            return GetAccounts().SingleOrDefault(account => account.UserName == userName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Account> GetAccountList()
        {
            List<Account> listAccount = new List<Account>();

            string sql = "SELECT Account.*, CONCAT(Staff.LastName, ' ', Staff.FirstName) AS StaffName, Role.Name AS RoleName ";
            sql += "FROM Account ";
            sql += "LEFT JOIN Staff ON  Account.StaffId = Staff.Id ";
            sql += "LEFT JOIN Role ON Account.RoleId = Role.Id ";
            sql += "ORDER BY Staff.Id ASC";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    conn.Open();

                    var reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Account account = new Account();
                            account.UserName = DBUtils.GetString(reader, "UserName").Trim();
                            account.Password = DBUtils.DecryptPassword(DBUtils.GetString(reader, "Password").Trim());
                            account.StaffId = DBUtils.GetInt(reader, "StaffId");
                            account.StaffName = DBUtils.GetString(reader, "StaffName");
                            account.RoleId = DBUtils.GetInt(reader, "RoleId");
                            account.RoleName = DBUtils.GetString(reader, "RoleName");
                            account.Email = DBUtils.GetString(reader, "Email").Trim();
                            account.CreateDate = DBUtils.GetDateTime(reader, "CreateDate");
                            account.UpdateDate = DBUtils.GetDateTime(reader, "UpdateDate");
                            account.RefreshToken = DBUtils.GetString(reader, "RefreshToken");
                            account.TokenExpired = DBUtils.GetDateTime(reader, "TokenExpired");

                            listAccount.Add(account);
                        }
                    }
                }
            }

            return listAccount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public int DeleteAccount(string userName)
        {
            int result = 0;
            string sql = "DELETE Account WHERE UserName = @userName";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@userName", SqlDbType.NVarChar).Value = userName;
                    try
                    {
                        conn.Open();
                        result = cmd.ExecuteNonQuery();
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newAccount"></param>
        public void SignUp(Account newAccount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO Account ");
            strSql.Append(" (");
            strSql.Append("       UserName ");
            strSql.Append("     , Password ");
            strSql.Append(" ) VALUES ");
            strSql.Append(" (");
            strSql.Append("       @USERNAME ");
            strSql.Append("     , @PASSWORD ");
            strSql.Append(" )");

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand updateCommand = conn.CreateCommand();
                SqlTransaction tran;
                tran = conn.BeginTransaction();
                updateCommand.Connection = conn;
                updateCommand.Transaction = tran;
                updateCommand.CommandText = strSql.ToString();
                try
                {
                    updateCommand.Parameters.Add("@USERNAME", SqlDbType.NVarChar).Value = newAccount.UserName;
                    updateCommand.Parameters.Add("@PASSWORD", SqlDbType.NVarChar).Value = DBUtils.EncryptPassword(newAccount.Password);
                    updateCommand.Parameters.Add("@CREATEDATE", SqlDbType.DateTime).Value = DateTime.Now;
                    updateCommand.Parameters.Add("@UPDATEDATE", SqlDbType.DateTime).Value = DateTime.Now;
                    updateCommand.ExecuteNonQuery();
                    tran.Commit();
                }
                catch (SqlException Ex)
                {
                    tran.Rollback();
                    throw Ex;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
