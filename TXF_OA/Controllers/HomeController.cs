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
            if (cookie != null)
            {
                OnlineUsers onlineUser = userBLL.Get(new Guid(cookie["uniqueID"] ?? ""));
                if (onlineUser != null)
                {
                    ViewBag.UserName = onlineUser.User.ItemName;
                    ViewBag.FaceSrc = onlineUser.User.FaceSrc ?? "/Content/images/face_1.gif";
                }
                else
                    Response.Redirect("/Account/Login");
            }
            return View();
        }
        #region 加载菜单
        public string GetAccordionData()
        {
            DataTable dt = moduleBLL.SelectDataTable(where: "ParentID=0 AND IsDisabled=0", sort: "ModuleCode");
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
                dt = moduleBLL.SelectDataTable(where: "IsDisabled=0", sort: "ModuleCode");
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
                              + ",\"IsDisabled\":\"" + row["IsDisabled"] + "\""
                              + "}},");
                }
                jsonStr = jsonStr.TrimEnd(',') + "]";
            }
            return jsonStr;
        }
        #endregion
    }
}
