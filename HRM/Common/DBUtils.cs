using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace HRM.Common
{
    public class DBUtils
    {
        // get string value
        public static string GetString(SqlDataReader reader, string fieldName)
        {
            return (reader[fieldName] as string) ?? String.Empty;
        }
        // get int value
        public static int GetInt(SqlDataReader reader, string fieldName)
        {
            return (reader[fieldName] as int?) ?? Constant.INT_DEFAULT_VALUE;
        }
        // get DateTime value
        public static DateTime GetDateTime(SqlDataReader reader, string fieldName)
        {
            return (reader[fieldName] as DateTime?) ?? DateTime.ParseExact(Constant.DATETIME_DEFAULT_VALUE, Constant.DATETIME_FORMAT, CultureInfo.InvariantCulture);
        }
    }
}
