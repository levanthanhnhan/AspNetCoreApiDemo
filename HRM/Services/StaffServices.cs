using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HRM.Common;
using HRM.Models;
using Microsoft.Extensions.Configuration;

namespace HRM.Services
{
    public class StaffService
    {
        private readonly string _connectionString;

        public StaffService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("ConnectionString");
        }

        /// <summary>
        /// GetStaffs
        /// </summary>
        /// <returns></returns>
        public List<Staff> GetStaffs()
        {
            List<Staff> listStaff = new List<Staff>();

            string sql = "SELECT * FROM Staff ORDER BY Id desc";

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
                            Staff staff = new Staff();

                            staff.Id = DBUtils.GetInt(reader, "ID");
                            staff.FirstName = DBUtils.GetString(reader, "FirstName");
                            staff.LastName = DBUtils.GetString(reader, "LastName");
                            staff.DateOfBirth = DBUtils.GetDateTime(reader, "DateOfBirth");
                            staff.Sex = DBUtils.GetInt(reader, "Sex");
                            staff.Address = DBUtils.GetString(reader, "Address");
                            staff.Email = DBUtils.GetString(reader, "Email");
                            staff.Phone = DBUtils.GetString(reader, "Phone");
                            staff.ContractDate = DBUtils.GetDateTime(reader, "ContractDate");
                            staff.DepartmentId = DBUtils.GetInt(reader, "DepartmentId");
                            staff.PositionId = DBUtils.GetInt(reader, "PositionId");
                            staff.CreateDate = DBUtils.GetDateTime(reader, "CreateDate");
                            staff.UpdateDate = DBUtils.GetDateTime(reader, "UpdateDate");

                            listStaff.Add(staff);
                        }
                    }
                }
            }

            return listStaff;
        }

        /// <summary>
        /// GetRelationStaffs
        /// </summary>
        /// <param name="departmentID"></param>
        /// <param name="PositionId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<StaffList> GetRelationStaffs(int departmentID,int PositionId,string name)
        {
            List<StaffList> listRelationStaff = new List<StaffList>();

            string sql = "SELECT Staff.*, Department.Name as DepartName,Position.Name as PosName FROM Staff";
            sql += " LEFT JOIN Department ON Staff.DepartmentId = Department.Id";
            sql += " LEFT JOIN Position ON Staff.PositionId = Position.Id";
            sql += " WHERE";
            sql += " '1' = '1'";

            if (departmentID != 0)
            {
                sql += " AND";
                sql += " (Staff.DepartmentId =";
                sql += " '" + departmentID + "'";
                sql += " OR";
                sql += " Department.ParentId =";
                sql += " '" + departmentID + "')";
            }
            if (PositionId != 0)
            {
                sql += " AND";
                sql += " Staff.PositionId =";
                sql += " '" + PositionId + "'";
            }
            if (name != string.Empty)
            {
                sql += " AND";
                sql += " dbo.fConvertToUnSign(CONCAT(Staff.LastName,' ',Staff.FirstName)) LIKE N'%";
                sql += "" + ConvertToUnSign(name) + "%'";
            }
            sql += " ORDER BY Staff.Id Desc";

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
                            StaffList staff = new StaffList();
                            staff.Id = DBUtils.GetInt(reader,"ID");
                            staff.FirstName = DBUtils.GetString(reader, "FirstName");
                            staff.LastName = DBUtils.GetString(reader, "LastName");
                            staff.DateOfBirth = DBUtils.GetDateTime(reader, "DateOfBirth");
                            staff.SexName = (DBUtils.GetInt(reader, "Sex") == 1) ? "MALE" : "FEMALE";
                            staff.Address = DBUtils.GetString(reader, "Address");
                            staff.Phone = DBUtils.GetString(reader, "Phone");
                            staff.ContractDate = DBUtils.GetDateTime(reader, "ContractDate");
                            staff.DepartmentId = DBUtils.GetInt(reader, "DepartmentId");
                            staff.PositionId = DBUtils.GetInt(reader, "PositionId");
                            staff.CreateDate = DBUtils.GetDateTime(reader, "CreateDate");
                            staff.UpdateDate = DBUtils.GetDateTime(reader, "UpdateDate");
                            staff.DepartmentName = DBUtils.GetString(reader, "DepartName"); 
                            staff.PositionName = DBUtils.GetString(reader, "PosName");

                            listRelationStaff.Add(staff);
                        }
                    }
                }
            }

            return listRelationStaff;
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
        /// Get Staff By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Staff GetStaffById(int id)
        {
            string sql = "SELECT * FROM Staff WHERE Id=@Id";
            Staff staff = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.Add("@Id", SqlDbType.Int);
                    command.Parameters["@Id"].Value = id;
                    conn.Open();

                    var reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            staff = new Staff
                            {
                                Id = DBUtils.GetInt(reader, "Id"),
                                FirstName = DBUtils.GetString(reader, "FirstName"),
                                LastName = DBUtils.GetString(reader, "LastName"),
                                DateOfBirth = DBUtils.GetDateTime(reader, "DateOfBirth"),
                                Sex = DBUtils.GetInt(reader, "Sex"),
                                Address = DBUtils.GetString(reader, "Address"),
                                Email = DBUtils.GetString(reader, "Email"),
                                Phone = DBUtils.GetString(reader, "Phone"),
                                ContractDate = DBUtils.GetDateTime(reader, "ContractDate"),
                                DepartmentId = DBUtils.GetInt(reader, "DepartmentId"),
                                PositionId = DBUtils.GetInt(reader, "PositionId"),
                                CreateDate = DBUtils.GetDateTime(reader, "CreateDate"),
                                UpdateDate = DBUtils.GetDateTime(reader, "UpdateDate"),
                            };
                        }
                    }
                }
            }
            return staff;
        }

        /// <summary>
        /// Update Staff
        /// </summary>
        /// <param name="staff"></param>
        /// <returns></returns>
        public bool UpdateStaff(Staff staff)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" UPDATE Staff ");
            sql.Append(" SET ");
            sql.Append(" FirstName = @firstName, ");
            sql.Append(" LastName = @lastName, ");
            sql.Append(" DateOfBirth = @dateOfBirth, ");
            sql.Append(" Sex = @sex, ");
            sql.Append(" Address = @address, ");
            sql.Append(" Email = @email, ");
            sql.Append(" Phone = @phone, ");
            sql.Append(" ContractDate = @contractDate, ");
            sql.Append(" DepartmentId = @departmentId, ");
            sql.Append(" PositionId = @positionId, ");
            sql.Append(" CreateDate = @createDate, ");
            sql.Append(" UpdateDate = @updateDate ");
            sql.Append(" WHERE Id = @id");

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), conn))
                {
                    try
                    {
                        command.Parameters.Add("@firstName", SqlDbType.NVarChar).Value = staff.FirstName;
                        command.Parameters.Add("@lastName", SqlDbType.NVarChar).Value = staff.LastName;
                        command.Parameters.Add("@dateOfBirth", SqlDbType.DateTime).Value = staff.DateOfBirth;
                        command.Parameters.Add("@sex", SqlDbType.Int).Value = staff.Sex;
                        command.Parameters.Add("@address", SqlDbType.NVarChar).Value = staff.Address;
                        command.Parameters.Add("@email", SqlDbType.NVarChar).Value = staff.Email;
                        command.Parameters.Add("@phone", SqlDbType.NVarChar).Value = staff.Phone;
                        command.Parameters.Add("@contractDate", SqlDbType.DateTime).Value = staff.ContractDate;
                        command.Parameters.Add("@departmentId", SqlDbType.Int).Value = staff.DepartmentId;
                        command.Parameters.Add("@positionId", SqlDbType.Int).Value = staff.PositionId;
                        command.Parameters.Add("@createDate", SqlDbType.DateTime).Value = staff.CreateDate;
                        command.Parameters.Add("@updateDate", SqlDbType.Date).Value = DateTime.Now;
                        command.Parameters.Add("@id", SqlDbType.Int).Value = staff.Id;

                        conn.Open();
                        command.ExecuteNonQuery();
                    } catch
                    {
                        return false;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int InsertStaffToDb(List<Staff> list)
        {
            int result = 0;
            SqlTransaction transaction = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    ArrayList listEror = new ArrayList();
                    conn.Open();
                    foreach (Staff staff in list)
                    {
                        StringBuilder sql = new StringBuilder();
                        sql.Append("INSERT INTO STAFF (");
                        sql.Append("      ID");
                        sql.Append("    , FIRSTNAME");
                        sql.Append("    , LASTNAME");
                        sql.Append("    , DATEOFBIRTH");
                        sql.Append("    , SEX");
                        sql.Append("    , ADDRESS");
                        sql.Append("    , EMAIL");
                        sql.Append("    , PHONE");
                        sql.Append("    , CONTRACTDATE");
                        sql.Append("    , DEPARTMENTID");
                        sql.Append("    , POSITIONID");
                        sql.Append("    , CREATEDATE");
                        sql.Append("    , UPDATEDATE)");
                        sql.Append("VALUES (");
                        sql.Append("      @ID");
                        sql.Append("    , @FIRSTNAME");
                        sql.Append("    , @LASTNAME");
                        sql.Append("    , @DATEOFBIRTH");
                        sql.Append("    , @SEX");
                        sql.Append("    , @ADDRESS");
                        sql.Append("    , @EMAIL");
                        sql.Append("    , @PHONE");
                        sql.Append("    , @CONTRACTDATE");
                        sql.Append("    , @DEPARTMENTID");
                        sql.Append("    , @POSITIONID");
                        sql.Append("    , @CREATEDATE");
                        sql.Append("    , @UPDATEDATE)");

                        using (SqlCommand command = new SqlCommand(sql.ToString(), conn))
                        {
                            command.Transaction = transaction;
                            command.Parameters.Add("@ID", SqlDbType.Int).Value = staff.Id;
                            command.Parameters.Add("@FIRSTNAME", SqlDbType.NVarChar).Value = staff.FirstName;
                            command.Parameters.Add("@LASTNAME", SqlDbType.NVarChar).Value = staff.LastName;
                            command.Parameters.Add("@DATEOFBIRTH", SqlDbType.DateTime).Value = staff.DateOfBirth;
                            command.Parameters.Add("@SEX", SqlDbType.Int).Value = staff.Sex;
                            command.Parameters.Add("@ADDRESS", SqlDbType.NVarChar).Value = staff.Address;
                            command.Parameters.Add("@EMAIL", SqlDbType.NVarChar).Value = staff.Email;
                            command.Parameters.Add("@PHONE", SqlDbType.NVarChar).Value = staff.Phone;
                            command.Parameters.Add("@CONTRACTDATE", SqlDbType.DateTime).Value = staff.ContractDate;
                            command.Parameters.Add("@DEPARTMENTID", SqlDbType.Int).Value = staff.DepartmentId;
                            command.Parameters.Add("@POSITIONID", SqlDbType.Int).Value = staff.PositionId;
                            command.Parameters.Add("@CREATEDATE", SqlDbType.DateTime).Value = DateTime.Now;
                            command.Parameters.Add("@UPDATEDATE", SqlDbType.Date).Value = DateTime.Now;
                            result = command.ExecuteNonQuery();
                        }
                    }
                }
                finally
                {
                    conn.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StaffList GetStaffInfoById(int id)
        {
            string sql = "SELECT *,Department.Name as DepartName,Position.Name as PosName FROM Staff";
            sql += " LEFT JOIN Department ON Staff.DepartmentId = Department.Id";
            sql += " LEFT JOIN Position ON Staff.PositionId = Position.Id";
            sql += " WHERE Staff.Id=@Id";
            StaffList staff = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.Add("@Id", SqlDbType.Int);
                    command.Parameters["@Id"].Value = id;
                    conn.Open();

                    var reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            staff = new StaffList
                            {
                                Id = DBUtils.GetInt(reader, "id"),
                                FirstName = DBUtils.GetString(reader, "FirstName"),
                                LastName = DBUtils.GetString(reader, "LastName"),
                                DateOfBirth = DBUtils.GetDateTime(reader, "DateOfBirth"),
                                Sex = DBUtils.GetInt(reader, "Sex"),
                                Address = DBUtils.GetString(reader, "Address"),
                                Email = DBUtils.GetString(reader, "Email"),
                                Phone = DBUtils.GetString(reader, "Phone"),
                                ContractDate = DBUtils.GetDateTime(reader, "ContractDate"),
                                DepartmentId = DBUtils.GetInt(reader, "DepartmentId"),
                                PositionId = DBUtils.GetInt(reader, "PositionId"),
                                CreateDate = DBUtils.GetDateTime(reader, "CreateDate"),
                                UpdateDate = DBUtils.GetDateTime(reader, "UpdateDate"),
                                SexName = DBUtils.GetInt(reader, "Sex") == 1 ? "MALE" : "FEMALE",
                                DepartmentName = DBUtils.GetString(reader, "DepartName"),
                                PositionName = DBUtils.GetString(reader, "PosName"),
                            };
                        }
                    }
                }
            }
            return staff;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkedList"></param>
        /// <returns></returns>
        public int DeleteStaffs(ArrayList checkedList)
        {
            var result = 0;
            string listStaffDel = String.Empty;
            foreach (var item in checkedList)
            {
                listStaffDel += item.ToString() + ",";
            }
            listStaffDel = listStaffDel.Remove(listStaffDel.LastIndexOf(","));

            string sqlAccount = "DELETE FROM Account";
            sqlAccount += " WHERE Account.StaffId IN (";
            sqlAccount += " " + listStaffDel + ")";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlAccount.ToString(), conn))
                {
                    conn.Open();
                    result = command.ExecuteNonQuery();
                }
            }

            string sql = "DELETE FROM Staff";
            sql += " WHERE Staff.Id IN (";
            sql += " " + listStaffDel + ")";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(), conn))
                {
                    conn.Open();
                    result = command.ExecuteNonQuery();
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="staff"></param>
        /// <returns></returns>
        public bool RegisterStaff(Staff staff)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("INSERT INTO STAFF (");
                    sql.Append("      FIRSTNAME");
                    sql.Append("    , LASTNAME");
                    sql.Append("    , DATEOFBIRTH");
                    sql.Append("    , SEX");
                    sql.Append("    , ADDRESS");
                    sql.Append("    , EMAIL");
                    sql.Append("    , PHONE");
                    sql.Append("    , CONTRACTDATE");
                    sql.Append("    , DEPARTMENTID");
                    sql.Append("    , POSITIONID");
                    sql.Append("    , CREATEDATE");
                    sql.Append("    , UPDATEDATE)");
                    sql.Append("VALUES (");
                    sql.Append("      @FIRSTNAME");
                    sql.Append("    , @LASTNAME");
                    sql.Append("    , @DATEOFBIRTH");
                    sql.Append("    , @SEX");
                    sql.Append("    , @ADDRESS");
                    sql.Append("    , @EMAIL");
                    sql.Append("    , @PHONE");
                    sql.Append("    , @CONTRACTDATE");
                    sql.Append("    , @DEPARTMENTID");
                    sql.Append("    , @POSITIONID");
                    sql.Append("    , @CREATEDATE");
                    sql.Append("    , @UPDATEDATE)");

                    using (SqlCommand command = new SqlCommand(sql.ToString(), conn))
                    {
                        DateTime compareDateTime = DateTime.ParseExact(Constant.DATETIME_DEFAULT_VALUE, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        command.Parameters.Add("@firstName", SqlDbType.NVarChar).Value = staff.FirstName;
                        command.Parameters.Add("@lastName", SqlDbType.NVarChar).Value = staff.LastName;
                        command.Parameters.Add("@dateOfBirth", SqlDbType.Date).Value = staff.DateOfBirth;
                        command.Parameters.Add("@sex", SqlDbType.Int).Value = staff.Sex;
                        command.Parameters.Add("@email", SqlDbType.NVarChar).Value = staff.Email;
                        command.Parameters.Add("@createDate", SqlDbType.Date).Value = DateTime.Now;
                        command.Parameters.Add("@updateDate", SqlDbType.Date).Value = DateTime.Now;

                        if (DateTime.Compare(compareDateTime, staff.ContractDate) == 0) { command.Parameters.Add("@contractDate", SqlDbType.VarChar).Value = DBNull.Value; }
                        else { command.Parameters.Add("@contractDate", SqlDbType.Date).Value = staff.ContractDate; }

                        if (staff.DepartmentId == Constant.INT_DEFAULT_VALUE) { command.Parameters.Add("@departmentId", SqlDbType.Int).Value = DBNull.Value; }
                        else command.Parameters.Add("@departmentId", SqlDbType.NVarChar).Value = staff.DepartmentId;

                        if (staff.PositionId == Constant.INT_DEFAULT_VALUE) { command.Parameters.Add("@positionId", SqlDbType.Int).Value = DBNull.Value; }
                        else command.Parameters.Add("@positionId", SqlDbType.NVarChar).Value = staff.PositionId;

                        if (string.IsNullOrEmpty(staff.Phone)) { command.Parameters.Add("@phone", SqlDbType.NVarChar).Value = DBNull.Value; }
                        else command.Parameters.Add("@phone", SqlDbType.NVarChar).Value = staff.Phone;

                        if (string.IsNullOrEmpty(staff.Address)) { command.Parameters.Add("@address", SqlDbType.NVarChar).Value = DBNull.Value; }
                        else command.Parameters.Add("@address", SqlDbType.NVarChar).Value = staff.Address;

                        command.ExecuteReader(CommandBehavior.CloseConnection);
                    }
                }
                catch
                {
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
            
            return true;
        }

        /// <summary>
        /// ConvertToUnSign
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ConvertToUnSign(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        /// <summary>
        /// Get Staff Department
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<StaffDepartmentList> GetStaffDepartment(int Id)
        {
            List<StaffDepartmentList> StaffDepartmentList = new List<StaffDepartmentList>();
            string sql = "SELECT Staff.Id as StaffId, Department.Id, CONCAT(Staff.LastName,' ',Staff.FirstName) as StaffName,Department.Name as DepartmentName FROM Staff";
            sql += " LEFT JOIN Department ON Staff.DepartmentId = Department.Id";
            sql += " WHERE Department.Id=@Id";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.Add("@Id", SqlDbType.Int);
                    command.Parameters["@Id"].Value = Id;
                    conn.Open();

                    var reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            StaffDepartmentList staffDepartmentList = new StaffDepartmentList
                            {
                                Id = DBUtils.GetInt(reader, "Id"),
                                StaffName = DBUtils.GetString(reader, "StaffName"),
                                DepartmentName = DBUtils.GetString(reader, "DepartmentName"),
                                StaffId = DBUtils.GetInt(reader, "StaffId"),
                            };
                            StaffDepartmentList.Add(staffDepartmentList);
                        }
                    }
                }
            }
            return StaffDepartmentList;
        }
    }

    public class StaffDepartmentList
    {
        public int Id { get; set; }
        public string StaffName { get; set; }
        public string DepartmentName { get; set; }
        public int StaffId { get; set; }
    }
}
