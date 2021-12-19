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
    public class DepartmentChartService
    {
        private readonly string _connectionString;

        public DepartmentChartService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("ConnectionString");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<DepartmentChart> GetDepartments()
        {
            List<DepartmentChart> listDeparmentChart = new List<DepartmentChart>();

            string sql = "SELECT Department.Name, COUNT(Department.Id) AS Members FROM Staff " +
                         "INNER JOIN Department ON Staff.DepartmentId = Department.Id " +
                         "WHERE Department.Id != 1 " +
                         "GROUP BY Department.Name, Department.LeaderStaffId";

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
                            DepartmentChart _departmentChart = new DepartmentChart();
                            _departmentChart.DepartmentName = DBUtils.GetString(reader, "Name");
                            _departmentChart.Members = DBUtils.GetInt(reader, "Members");

                            listDeparmentChart.Add(_departmentChart);
                        }
                    }
                }
            }

            return listDeparmentChart;
        }
    }
}
