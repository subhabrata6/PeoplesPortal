
using System.Data;

namespace TestEmployeeApp.HelperClass
{
    public static class Helper
    {

        public static string GetConnectionString(IConfiguration config)
        {
            return config.GetConnectionString("WinAuth");
        }

        public static string GetDBStringValue(object row)
        {
            if (row == null)
                return string.Empty;
            else if(DBNull.Value == row) 
                return string.Empty;
            else
                return Convert.ToString(row);
        }

        public static int GetDBIntValue(object row)
        {
            if (row == null)
                return 0;
            else if (DBNull.Value == row)
                return 0;
            else if(string.IsNullOrEmpty(row.ToString()))
                return 0;
            else
                return Convert.ToInt32(row);
        }
        public static bool GetDBBoolValue(object row)
        {
            if (row == null)
                return false;
            else if (DBNull.Value == row)
                return false;
            else if (string.IsNullOrEmpty(row.ToString()))
                return false;
            else
                return Convert.ToBoolean(row);
        }
        public static decimal GetDBDecimalValue(object row)
        {
            if (row == null)
                return 0;
            else if (DBNull.Value == row)
                return 0;
            else if (string.IsNullOrEmpty(row.ToString()))
                return 0;
            else
                return Convert.ToDecimal(row);
        }

        public static DateTime GetDBDateTimeValue(object row)
        {
            if (row == null)
                return new DateTime();
            else if (DBNull.Value == row)
                return new DateTime();
            else if (string.IsNullOrEmpty(row.ToString()))
                return new DateTime();
            else
                return Convert.ToDateTime(row);
        }
        public static Guid GetDBGuidValue(object row)
        {
            if (row == null)
                return Guid.Empty;
            else if (DBNull.Value == row)
                return Guid.Empty;
            else
                return new Guid(row?.ToString());
        }
    }
}
