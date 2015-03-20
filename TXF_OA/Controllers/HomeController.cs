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
    public class HomeController : BaseController
    {
        [Inject]
        public Itb_sys_ModuleBLL moduleBLL { get; set; }

        public ActionResult Index()
        {
            if (CurrentUser != null)
            {
                ViewBag.UserName = CurrentUser.ItemName;
                ViewBag.FaceSrc = CurrentUser.FaceSrc ?? "/Content/images/face_1.gif";
            }
            return View();
        }
        #region 加载菜单
        public string GetAccordionData()
        {
            DataTable dt = moduleBLL.SelectDataTable(where: "ParentID=0", sort: "ModuleCode");
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
                dt = moduleBLL.SelectDataTable(where: "1=1", sort: "ModuleCode");
            DataRow[] rows = dt.Select("ParentID=" + id + "");
            string jsonStr = "[]";
            if (rows.Length > 0)
            {
                jsonStr = "[";
                foreach (DataRow row in rows)
                {
                    string pageUrl = row["PageUrl"].ToString();
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

        #region 注销
        public ActionResult LogOut()
        {
            HttpCookie cookie = new HttpCookie("Account");
            cookie.Expires = DateTime.Now.Add(new TimeSpan(-1, 0, 0, 0));
            Response.AppendCookie(cookie);
            Session["User"] = null;
            return Redirect("/Account/Login");
        }
        #endregion
    }
}
