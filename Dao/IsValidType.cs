using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Dao
{
    public static class IsValidType
    {
        /// <summary>
        /// 检测是否整数16位
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInt16(string value)
        {
            try
            {
                Int16.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 检测是否整数32位
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInt32(string value)
        {
            try
            {
                Int32.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 检测是否整数64位
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInt64(string value)
        {
            try
            {
                Int64.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 检测是否Double
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDouble(string value)
        {
            try
            {
                double.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 检测是否Decimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDecimal(string value)
        {
            try
            {
                decimal.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 检测是否日期，含时间
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDateTime(string value)
        {
            try
            {
                DateTime.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 检测是否布尔型数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsBool(string value)
        {
            try
            {
                bool.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 正则表达式验证
        /// </summary>
        /// <param name="str"></param>
        /// <param name="parten"></param>
        /// <returns></returns>
        public static bool RegexMatch(string str, string pattern)
        {
            //return Regex.IsMatch(str, pattern);
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            return regex.IsMatch(str);
        }
    }
}
