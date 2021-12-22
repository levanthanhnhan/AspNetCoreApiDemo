using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

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

        /// <summary>
        /// Encrypt Password
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <returns></returns>
        public static string EncryptPassword(string password)
        {
            bool useHashing = true;
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(password);

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(Constant.ENCRYPT_PASSWORD_KEY));
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(Constant.ENCRYPT_PASSWORD_KEY);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// Decrypt Password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string DecryptPassword(string password)
        {
            bool useHashing = true;
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(password);

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(Constant.ENCRYPT_PASSWORD_KEY));
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(Constant.ENCRYPT_PASSWORD_KEY);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}
