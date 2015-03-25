using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Utility
{
    public class Tools
    {
        /// <summary>
        /// 获取远程浏览器端 IP 地址
        /// </summary>
        /// <returns>返回 IPv4 地址</returns>
        public static string GetIPAddress()
        {
            string userHostAddress = HttpContext.Current.Request.UserHostAddress;
            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return userHostAddress;
        }

        /// <summary>
        /// 得到用户浏览器类型
        /// </summary>
        /// <returns></returns>
        public static string GetBrowse()
        {
            return System.Web.HttpContext.Current.Request.Browser.Type;
        }

        /// <summary>
        /// 获取浏览器端操作系统名称
        /// </summary>
        /// <returns></returns>
        public static string GetOSName()
        {
            string osVersion = System.Web.HttpContext.Current.Request.Browser.Platform;
            string userAgent = System.Web.HttpContext.Current.Request.UserAgent;

            if (userAgent.Contains("NT 6.3"))
            {
                osVersion = "Windows8.1";
            }
            else if (userAgent.Contains("NT 6.2"))
            {
                osVersion = "Windows8";
            }
            else if (userAgent.Contains("NT 6.1"))
            {
                osVersion = "Windows7";
            }
            else if (userAgent.Contains("NT 6.0"))
            {
                osVersion = "WindowsVista";
            }
            else if (userAgent.Contains("NT 5.2"))
            {
                osVersion = "WindowsServer2003";
            }
            else if (userAgent.Contains("NT 5.1"))
            {
                osVersion = "WindowsXP";
            }
            else if (userAgent.Contains("NT 5"))
            {
                osVersion = "Windows2000";
            }
            else if (userAgent.Contains("NT 4"))
            {
                osVersion = "WindowsNT4.0";
            }
            else if (userAgent.Contains("Me"))
            {
                osVersion = "WindowsMe";
            }
            else if (userAgent.Contains("98"))
            {
                osVersion = "Windows98";
            }
            else if (userAgent.Contains("95"))
            {
                osVersion = "Windows95";
            }
            else if (userAgent.Contains("Mac"))
            {
                osVersion = "Mac";
            }
            else if (userAgent.Contains("Unix"))
            {
                osVersion = "UNIX";
            }
            else if (userAgent.Contains("Linux"))
            {
                osVersion = "Linux";
            }
            else if (userAgent.Contains("SunOS"))
            {
                osVersion = "SunOS";
            }
            return osVersion;
        }
    }
}
