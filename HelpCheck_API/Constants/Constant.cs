using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace HelpCheck_API.Constants
{
    public class Constant
    {
        public static readonly string SE_Asia_Standard_Time = "SE Asia Standard Time";
        public static readonly CultureInfo DATETIME_FORMAT = new CultureInfo("es-ES");
        public static readonly int DIFF_YEAR_BHUDDA = 543;

        public static readonly Dictionary<int, string> MASTER_MONTH_NAME = new Dictionary<int, string>() {
            { 1,"มกราคม"},
            { 2,"กุมภาพันธ์" },
            { 3,"มีนาคม" },
            { 4,"เมษายน" },
            { 5,"พฤษภาคม" },
            { 6,"มิถุนายน" },
            { 7,"กรกฎาคม" },
            { 8,"สิงหาคม" },
            { 9,"กันยายน" },
            { 10,"ตุลาคม" },
            { 11,"พฤศจิกายน" },
            { 12,"ธันวาคม" },
        };
        public static readonly string TOKEN_TYPE_BEARER = "bearer";
        public static readonly string STATUS_SUCCESS = "success";
        public static readonly string STATUS_USER_ALREADY_EXISTS = "User already exists";
        public static readonly string STATUS_ERROR = "Internal server error";
        public static readonly string STATUS_DATA_NOT_FOUND = "Data not found";
        public static readonly string STATUS_DUPLICATE_DATA = "Duplicate data";
        public static readonly string STATUS_PASSWORD_NOT_MATCH = "This username or password not match";
        public static readonly string STATUS_INVALID_REQUEST_DATA = "Invalid request data";
        public static readonly string STATUS_INVALID_REQUEST_HEADER = "Invalid request header.Request Header of POST and PUT method should be either application/json or multipart/form-data.";
        public static readonly string STATUS_TOKEN_EXPIRED = "Token is already expired";
        public static readonly string DATA_STATUS_ACTIVE = "active";
        public static readonly string DATA_STATUS_CANCEL = "cancel";
        public static readonly string DATA_STATUS_DELETE = "delete";
        public static readonly string DATA_ID_CARD_NOT_CORRECT = "เลขบัตรประชาชนไม่ถูกต้อง";
        public static readonly int USER_TOKEN_EXPIRE_MINUTE = 30;
        public static readonly string NOTIFICATION_STATUS_SUCCESS = "success";
        public static readonly string NOTIFICATION_STATUS_WARNING = "warning";
        public static readonly string NOTIFICATION_STATUS_ERROR = "error";
        public static readonly string NOTIFICATION_STATUS_INFO = "info";
        public static readonly string DATA_STATUS_FORBIDDEN = "Your request is denied";
        public static readonly string DATA_OTP_EXPIRE = "OTP หมดอายุ โปรดขอรหัสอีกครั้ง";
        public static readonly string DATA_CODE_EXPIRE = "รหัสยืนยันนี้ไม่ถูกต้อง หรือรหัสยืนยันหมดอายุแล้ว โปรดขอรหัสอีกครั้ง";
    }
}
