using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRM.Models;
using System.Data.SqlClient;
using HRM.Common;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;

namespace HRM.Services
{
    public class DepartmentService
    {
        private readonly string _connectionString;

        public DepartmentService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("ConnectionString");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Department> GetDepartments()
        {
            List<Department> listDept = new List<Department>();

            string sql = "SELECT * FROM Department";

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
                            Department dept = new Department();
                            dept.Id = DBUtils.GetInt(reader, "Id");
                            dept.Name = DBUtils.GetString(reader, "Name");
                            listDept.Add(dept);
                        }
                    }
                }
            }
            return listDept;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Department> GetAllDepartment()
        {
            List<Department> listDept = new List<Department>();

            string sql = "SELECT * FROM Department AS DP LEFT JOIN Staff AS STF ON DP.LeaderStaffId = STF.ID";

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
                            Department dept = new Department();
                            dept.Id = DBUtils.GetInt(reader, "Id");
                            dept.ParentId = DBUtils.GetInt(reader, "ParentId");
                            dept.LeaderStaffId = DBUtils.GetInt(reader, "LeaderStaffId");
                            dept.Name = DBUtils.GetString(reader, "Name");
                            dept.LeaderName = DBUtils.GetString(reader, "LastName") + ' ' + DBUtils.GetString(reader, "FirstName");
                            listDept.Add(dept);
                        }
                    }
                }
            }
            return listDept;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public Department GetDepartmentByID(int departmentId)
        {
            Department dept = new Department();
            string sql = "SELECT DP.*, STF.FirstName, STF.LastName FROM Department AS DP LEFT JOIN Staff AS STF ON DP.LeaderStaffId = STF.ID WHERE DP.ID = @ID";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    command.Parameters.Add("@ID", SqlDbType.Int).Value = departmentId;
                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            dept.Id = DBUtils.GetInt(reader, "Id");
                            dept.ParentId = DBUtils.GetInt(reader, "ParentId");
                            dept.LeaderStaffId = DBUtils.GetInt(reader, "LeaderStaffId");
                            dept.Name = DBUtils.GetString(reader, "Name");
                            dept.LeaderName = DBUtils.GetString(reader, "LastName") + ' ' + DBUtils.GetString(reader, "FirstName");
                        }
                    }
                    conn.Close();
                }
            }
            return dept;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int RegisterDepartment(Department info)
        {
            int result = 0;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO DEPARTMENT ");
            strSql.Append(" (");
            strSql.Append("       ID");
            strSql.Append("     , PARENTID");
            strSql.Append("     , NAME ");
            strSql.Append("     , LEADERSTAFFID ");
            strSql.Append("     , CREATEID ");
            strSql.Append("     , CREATEDATE ");
            strSql.Append("     , UPDATEID ");
            strSql.Append("     , UPDATEDATE ");
            strSql.Append(" ) VALUES ");
            strSql.Append(" (");
            strSql.Append("     @ID");
            strSql.Append("     ,  @PARENTID");
            strSql.Append("     , @NAME ");
            strSql.Append("     , @LEADERSTAFFID ");
            strSql.Append("     , @CREATEID ");
            strSql.Append("     , @CREATEDATE ");
            strSql.Append("     , @UPDATEID ");
            strSql.Append("     , @UPDATEDATE ");
            strSql.Append(" )");

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(strSql.ToString(), conn))
                {
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = GetmaxId() + 1;
                    cmd.Parameters.Add("@PARENTID", SqlDbType.Int).Value = info.ParentId;
                    cmd.Parameters.Add("@NAME", SqlDbType.NVarChar).Value = info.Name;
                    cmd.Parameters.Add("@LEADERSTAFFID", SqlDbType.Int).Value = info.LeaderStaffId;
                    cmd.Parameters.Add("@CREATEID", SqlDbType.Int).Value = info.CreateId;
                    cmd.Parameters.Add("@CREATEDATE", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add("@UPDATEID", SqlDbType.Int).Value = info.UpdateId;
                    cmd.Parameters.Add("@UPDATEDATE", SqlDbType.DateTime).Value = DateTime.Now;
                    try
                    {
                        conn.Open();
                        result = cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex){
                        Console.WriteLine(ex);
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
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Department> SearchDepartments(string name)
        {
            List<Department> listDepartment = new List<Department>();

            StringBuilder strSql = new StringBuilder();
            string sql = "SELECT DP.*, STF.FirstName, STF.LastName FROM Department AS DP LEFT JOIN Staff AS STF ON DP.LeaderStaffId = STF.ID";
            if (name != string.Empty) {
                sql += " WHERE dbo.fConvertToUnSign(DP.Name) LIKE  N'%";
                sql += "" + convertToUnSign(name) + "%'";
            }
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Department dept = new Department();
                            dept.Id = DBUtils.GetInt(reader, "Id");
                            dept.ParentId = DBUtils.GetInt(reader, "ParentId");
                            dept.LeaderStaffId = DBUtils.GetInt(reader, "LeaderStaffId");
                            dept.Name = DBUtils.GetString(reader, "Name");
                            dept.LeaderName = DBUtils.GetString(reader, "LastName") + ' ' + DBUtils.GetString(reader, "FirstName");
                            listDepartment.Add(dept);
                        }
                    }
                    conn.Close();
                }
            }
            return listDepartment;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int UpdateDepartment(Department info)
        {
            int result = 0;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE DEPARTMENT ");
            strSql.Append("   SET PARENTID = @PARENTID ");
            strSql.Append("      ,NAME = @NAME ");
            strSql.Append("      ,LEADERSTAFFID = @LEADERSTAFFID ");
            strSql.Append("      ,UPDATEDATE = @UPDATEDATE ");
            strSql.Append("      ,UPDATEID = @UPDATEID ");
            strSql.Append(" WHERE ID = @ID ");

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(strSql.ToString(), conn))
                {
                    cmd.Parameters.Add("@PARENTID", SqlDbType.Int).Value = info.ParentId;
                    cmd.Parameters.Add("@NAME", SqlDbType.NVarChar).Value = info.Name;
                    cmd.Parameters.Add("@LEADERSTAFFID", SqlDbType.Int).Value = info.LeaderStaffId;
                    cmd.Parameters.Add("@UPDATEDATE", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add("@UPDATEID", SqlDbType.Int).Value = info.UpdateId;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = info.Id;
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
        /// <param name="departmentID"></param>
        /// <returns></returns>
        public int DeleteDepartment(int departmentID)
        {
            int result = 0;
            string sql = "DELETE DEPARTMENT WHERE ID = @ID";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = departmentID;
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
        /// <param name="departmentID"></param>
        /// <returns></returns>
        public int DeleteDepartmentChild(int departmentID)
        {
            int result = 0;
            string sql = "DELETE DEPARTMENT WHERE PARENTID = @PARENTID";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@PARENTID", SqlDbType.Int).Value = departmentID;
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
        /// <returns></returns>
        private int GetmaxId()
        {
            string sql = "SELECT MAX(ID)FROM DEPARTMENT";
            int max = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    max = (int)cmd.ExecuteScalar();
                    conn.Close();
                }  
            }
            return max;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Department FindDepartmentByName(string name)
        {
            return GetDepartments().SingleOrDefault(depart => depart.Name == name);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string convertToUnSign(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentID"></param>
        /// <returns></returns>
        public bool checkDepartmentHasStaff(int departmentID)
        {
            string sql = "SELECT DEPARTMENTID FROM STAFF WHERE DEPARTMENTID = @DEPARTMENTID";
            bool result = false;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    cmd.Parameters.Add("@DEPARTMENTID", SqlDbType.Int).Value = departmentID;
                    var reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        result = true;
                    }
                    else
                    {
                        result =  false;
                    }
                    conn.Close();
                }
            }
            return result;
        }
    }
}
