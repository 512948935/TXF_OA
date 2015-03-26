using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using IBLL;
using Ninject;
using Model;

namespace TXF_OA
{
    public class HomeController:Controller
    {
        [Inject]
        public Itb_sys_ModuleBLL moduleBLL { get; set; }
        [Inject]
        public Itb_item_UserBLL userBLL { get; set; }

        public ActionResult Index()
        {
            HttpCookie cookie = Request.Cookies["ID"];
            OnlineUsers onlineUser = null;
            if (cookie != null)
            {
                onlineUser = userBLL.Get(new Guid(cookie["uniqueID"] ?? ""));
                if (onlineUser != null)
                {
                    ViewData["UserName"] = onlineUser.User.ItemName;
                    ViewData["FaceSrc"] = onlineUser.User.FaceSrc;
                    ViewData["RoleID"] = onlineUser.User.RoleID ?? 0;
                }
                else
                    return Redirect("/Account/Login");
            }
            return View();
        }
        #region 加载菜单
        public string GetAccordionData(int RoleID = 0)
        {
            DataTable dt = moduleBLL.GetModuleByRoleID("ParentID=0 AND RoleID=" + RoleID);
            string jsonStr = "[]";
            if (dt.Rows.Count > 0)
            {
                jsonStr = "[";
                foreach (DataRow row in dt.Rows)
                {
                    jsonStr += ("{\"id\":" + row["id"] + ",\"title\":\"" + row["ModuleName"] + "\",\"iconCls\":\"" + row["Icon"] + "\",\"selected\":\"false\"},");
                }
                jsonStr = jsonStr.TrimEnd(',') + "]";
            }
            return jsonStr;
        }
        private DataTable dt;
        public string GetTreeData(int id = 0)
        {
            if (dt == null)
                dt = moduleBLL.GetModuleByRoleID("RoleID=12");
            DataRow[] rows = dt.Select("ParentID=" + id + "");
            string jsonStr = "[]";
            if (rows.Length > 0)
            {
                jsonStr = "[";
                foreach (DataRow row in rows)
                {
                    string pageUrl = Convert.ToString(row["PageUrl"]);
                    if (Convert.ToInt32(row["IsItem"]) > 0)
                        pageUrl += "?isRedirect=true";
                    jsonStr += ("{\"id\":" + row["ID"] + ",\"text\":\"" + row["ModuleName"] + "\",\"state\":\"" + row["NodeState"] + "\",\"iconCls\":\"" + row["Icon"] + "\""
                              + ",\"children\":" + GetTreeData(Convert.ToInt32(row["ID"])) + ""
                              + ",\"attributes\":{\"ID\":\"" + row["ID"] + "\""
                              + ",\"ModuleCode\":\"" + row["ModuleCode"] + "\""
                              + ",\"ModuleName\":\"" + row["ModuleName"] + "\""
                              + ",\"PageUrl\":\"" + pageUrl + "\""
                              + ",\"Icon\":\"" + row["Icon"] + "\""
                              + "}},");
                }
                jsonStr = jsonStr.TrimEnd(',') + "]";
            }
            return jsonStr;
        }
        #endregion

        #region 我的桌面
        public ActionResult MyDesk()
        {
            return View();
        }
        #endregion
    }
}
