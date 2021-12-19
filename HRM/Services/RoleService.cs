using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Linq;
using HRM.Models;
using HRM.Common;
using System.Collections;
using System.Text;
using System.Data;

namespace HRM.Services
{
    public class RoleService
    {
        private readonly string _connectionString;
        public RoleService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("ConnectionString");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Role> GetRoles()
        {
            List<Role> listRole = new List<Role>();

            SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = conn;
            sqlCmd.CommandText = "SELECT * FROM Role";

            try
            {
                SqlDataReader reader = sqlCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Role role = new Role();
                        role.Id = DBUtils.GetInt(reader, "ID");
                        role.Name = DBUtils.GetString(reader, "Name");

                        listRole.Add(role);
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

            return listRole;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<RoleAccess> GetRoleAccesss()
        {
            List<RoleAccess> listRoleAccess = new List<RoleAccess>();

            SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = conn;
            sqlCmd.CommandText = "SELECT * FROM RoleAccess";

            try
            {
                SqlDataReader reader = sqlCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        RoleAccess roleAccess = new RoleAccess();
                        roleAccess.AccessId = DBUtils.GetInt(reader, "AccessId");
                        roleAccess.RoleId = DBUtils.GetInt(reader, "RoleId");
                        listRoleAccess.Add(roleAccess);
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

            return listRoleAccess;
        }

    }
}
