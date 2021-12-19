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
    public class RoleAccessService
    {
        private readonly string _connectionString;
        public RoleAccessService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("ConnectionString");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listRole"></param>
        /// <returns></returns>
        public int InsertUpdateRole(List<Role> listRole)
        {
            int resultRows = 0;

            foreach (var role in listRole)
            {
                if (role.IsInsert)
                {
                    string sql = "INSERT INTO Role (Id, Name, CreateDate, UpdateDate) VALUES (@Id, @Name, @CreateDate, @UpdateDate)";

                    using (SqlConnection conn = new SqlConnection(_connectionString))
                    {
                        using (SqlCommand command = new SqlCommand(sql, conn))
                        {
                            conn.Open();

                            command.Parameters.AddWithValue("@Id", role.Id);
                            command.Parameters.AddWithValue("@Name", role.Name);
                            command.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                            command.Parameters.AddWithValue("@UpdateDate", DateTime.Now);

                            var rows = command.ExecuteNonQuery();

                            conn.Close();

                            resultRows++;
                        }
                    }
                }
                else
                {
                    string sql = "UPDATE Role SET Name = @Name, CreateDate = @CreateDate, UpdateDate = @UpdateDate WHERE Id = @Id";

                    using (SqlConnection conn = new SqlConnection(_connectionString))
                    {
                        using (SqlCommand command = new SqlCommand(sql, conn))
                        {
                            conn.Open();

                            command.Parameters.AddWithValue("@Name", role.Name);
                            command.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                            command.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                            command.Parameters.AddWithValue("@Id", role.Id);

                            var rows = command.ExecuteNonQuery();

                            conn.Close();

                            resultRows++;
                        }
                    }
                }

            }
            return resultRows;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listRoleAccess"></param>
        /// <returns></returns>
        public int SaveRoleAccess(List<RoleAccess> listRoleAccess)
        {
            int resultRows = 0;

            foreach (var roleAccess in listRoleAccess)
            {
                string sql = "INSERT INTO RoleAccess (RoleId, AccessId) VALUES (@RoleId, @AccessId)";

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(sql, conn))
                    {
                        conn.Open();

                        command.Parameters.AddWithValue("@RoleId", roleAccess.RoleId);
                        command.Parameters.AddWithValue("@AccessId", roleAccess.AccessId);

                        var rows = command.ExecuteNonQuery();

                        conn.Close();

                        resultRows++;
                    }
                }
            }
            return resultRows;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool DeleteRoleAccess()
        {
            try
            {
                string sql = "DELETE FROM RoleAccess";

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(sql, conn))
                    {
                        conn.Open();
                        command.ExecuteNonQuery();
                        conn.Close();
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListRoleDelete"></param>
        /// <returns></returns>
        public bool DeleteRole(List<Role> ListRoleDelete)
        {
            try
            {
                int listRoleDel = 0;
                foreach (var role in ListRoleDelete)
                {
                    listRoleDel = role.Id;
                    string sqlAccount = "DELETE FROM Role WHERE Role.Id IN (@listRoleDel)";

                    using (SqlConnection conn = new SqlConnection(_connectionString))
                    {
                        using (SqlCommand command = new SqlCommand(sqlAccount.ToString(), conn))
                        {
                            conn.Open();
                            command.Parameters.AddWithValue("@listRoleDel", listRoleDel);
                            command.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
