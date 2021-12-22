using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Common
{
    public class Constant
    {
        public const int TOKEN_EXPIRED_MINUTES = 5;
        public const int TOKEN_REFRESH_EXPIRED_MINUTES = 30;
        public const int INT_DEFAULT_VALUE = -1;
        public const string DATETIME_DEFAULT_VALUE = "1900-01-01";
        public const string DATETIME_FORMAT = "yyyy-MM-dd";
        public const int ITEM_PER_PAGE = 10;
        public const string ENCRYPT_PASSWORD_KEY = "ENCRYPTPASSWORDKEY";

        public struct ACCOUNT_ACTIVATION
        {
            public const string ACTIVATE_SUCCESS = "2";
            public const string ACTIVATE_NOTFOUND = "0";
            public const string ACTIVATE_ERROR = "-1";
            public const string ACCOUNT_ACTIVATED = "1";
        }
    }
}
