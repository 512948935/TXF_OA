using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Data;
using Newtonsoft.Json;
using System.IO;
using Model;
using Ninject;
using IBLL;

namespace TXF_OA
{
    public class BaseController : Controller
    {
        //定义一个基类的UserInfo对象
        public tb_item_User CurrentUser { get; set; }
        [Inject]
        public Itb_item_UserBLL userBLL { get; set; }
        /// <summary>
        /// 重写基类在Action之前执行的方法
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            CurrentUser = Session["User"] as tb_item_User;
            if (CurrentUser == null)
            {
                if (Request.Cookies["Account"] != null && userBLL != null)
                {
                    string name = Request.Cookies["Account"]["UserName"];
                    string pwd = Request.Cookies["Account"]["UserPwd"];
                    List<WhereField> wheres = new List<WhereField>(){
                                          new WhereField ("ItemName",name)
                                         ,new WhereField("UserPwd",pwd )
                    };
                    Session["User"] = CurrentUser = userBLL.SelectT(wheres);
                }
                else
                    Response.Redirect("/Account/Login");
            }
            else
            {
                if (Request.Cookies["Account"] == null)
                {
                    HttpCookie cookie = new HttpCookie("Account");//初使化并设置Cookie的名称
                    DateTime dt = DateTime.Now;
                    TimeSpan ts = new TimeSpan(1, 0, 0, 0, 0);//过期时间为1分钟
                    cookie.Expires = dt.Add(ts);//设置过期时间
                    cookie.Values.Add("UserName", CurrentUser.ItemName);
                    cookie.Values.Add("UserPwd", CurrentUser.UserPwd);
                    Response.AppendCookie(cookie);
                }
            }
            //检验用户是否已经登录，如果登录则不执行，否则则执行下面的跳转代码
        }
        #region JsonHelper
        /// <summary>
        /// DataTable to JSON
        /// </summary>
        /// <param name="jsonName">返回json的名称</param>
        /// <param name="dt">转换成json的表</param>
        /// <returns></returns>
        public static string DataTableToJson(DataTable dt)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":\"" + StringFilter(dt.Rows[i][j].ToString()) + "\"");
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]");
            return Json.ToString();
        }
        public static string DataRowToJson(DataRow row)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("[");
            if (row != null)
            {
                Json.Append("{");
                for (int j = 0; j < row.Table.Columns.Count; j++)
                {
                    Json.Append("\"" + row.Table.Columns[j].ColumnName.ToString() + "\":\"" + StringFilter(row[j].ToString()) + "\"");
                    if (j < row.Table.Columns.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
                Json.Append("}");
            }
            Json.Append("]");
            return Json.ToString();
        }
        /// <summary>
        /// DataTable to TreeJson
        /// </summary>
        /// <param name="jsonName">返回json的名称</param>
        /// <param name="dt">转换成json的表</param>
        /// <returns></returns>
        public static string DataTableToTreeJson(DataTable dt)
        {
            DataRow[] rows = dt.Select("ParentID=0");
            string Json = "[";
            foreach (DataRow row in rows)
            {
                Json += "{";
                for (int j = 0; j < row.Table.Columns.Count; j++)
                {
                    Json += "\"" + row.Table.Columns[j].ColumnName.ToString() + "\":\"" + StringFilter(row[j].ToString()) + "\",";
                }
                Json += "\"children\":" + GetChildrenTreeJson(dt, row["ID"]) + "},";
            }
            return Json = Json.TrimEnd(',') + "]";
        }
        private static string GetChildrenTreeJson(DataTable dt, object parentID)
        {
            DataRow[] rows = dt.Select("ParentID=" + parentID + "");
            string Json = "[";
            foreach (DataRow row in rows)
            {
                Json += "{";
                for (int j = 0; j < row.Table.Columns.Count; j++)
                {
                    Json += "\"" + row.Table.Columns[j].ColumnName.ToString() + "\":\"" + StringFilter(row[j].ToString()) + "\",";
                }
                Json += "\"children\":" + GetChildrenTreeJson(dt, row["ID"]) + "},";
            }
            return Json = Json.TrimEnd(',') + "]";
        }
        private static String StringFilter(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {

                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '/':
                        sb.Append("\\/");
                        break;
                    case '\b':      //退格
                        sb.Append("\\b");
                        break;
                    case '\f':      //走纸换页
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n"); //换行    
                        break;
                    case '\r':      //回车
                        sb.Append("\\r");
                        break;
                    case '\t':      //横向跳格
                        sb.Append("\\t");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// JSONstringToList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static List<T> JSONStringToList<T>(string jsonStr)
        {
            if (string.IsNullOrEmpty(jsonStr)) return null;
            T obj = Activator.CreateInstance<T>();
            return JsonConvert.DeserializeObject<List<T>>(jsonStr);
        }
        private static T Deserialize<T>(string jsonStr)
        {
            if (string.IsNullOrEmpty(jsonStr)) return default(T); ;
            T obj = Activator.CreateInstance<T>();
            return JsonConvert.DeserializeObject<T>(jsonStr);
        }
        #endregion

        #region 创建WhereHTML
        public ActionResult CreateWhereHTML(string col)
        {
            List<WhereColumn> columns = JSONStringToList<WhereColumn>(col);
            try
            {
                string strHTML = "<table id='ttSearch' class='tablestyle' border='0' cellpadding='0' cellspacing='0' align='center' width='100%'>";
                foreach (WhereColumn column in columns)
                {
                    strHTML += "<tr>";
                    strHTML += "<td class='td1'>" + column.ColumnTitle + "：</td>";
                    strHTML += "<td class='td2'><input type='text' class='txt1' id='t_" + column.ColumnField + "' name='" + column.ColumnField + "'/></td>";
                    strHTML += "</tr>";
                }
                strHTML += "</table>";
                return Content(strHTML);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public class WhereColumn
        {
            public string ColumnField { get; set; }
            public string ColumnTitle { get; set; }
        }
        #endregion

        #region Excel导出
        public void ExportToExcel(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            string json = Request["columns"];
            string title = Request["title"];
            List<ExportColumn> columns = JSONStringToList<ExportColumn>(json);
            sb.Append("<table width='100%' border='1' cellpadding='0' cellspacing='0'>");
            sb.Append("<tr>");
            string fields = "";
            foreach (ExportColumn column in columns)
            {
                fields += column.ColumnField + ",";
                sb.Append("<th height='25'><div align='center'>" + column.ColumnTitle + "</div></th>");
            }
            sb.Append("</tr>");
            foreach (DataRow row in dt.Rows)
            {
                sb.Append("<tr>");
                foreach (ExportColumn col in columns)
                {
                    sb.Append("<td height='25'><div align='center'>" + row[col.ColumnField] + "</div></td>");

                } sb.Append("</tr>");
            }
            sb.Append("</table>");
            Response.ClearHeaders();
            Response.Clear();
            Response.Expires = 0;
            Response.Buffer = true;
            Response.Charset = "UTF-8";
            Response.HeaderEncoding = System.Text.Encoding.UTF8;
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            if (dt.Rows.Count < 6)
            {
                Response.HeaderEncoding = System.Text.Encoding.Default;
                Response.ContentEncoding = System.Text.Encoding.Default;
            }
            Response.AddHeader("content-disposition", "attachment; filename=" + HttpUtility.UrlEncode(Path.GetFileName("" + title + ".xls")));
            Response.ContentType = "application/ms-excel";
            Response.Write(sb.ToString());
        }
        public class ExportColumn
        {
            public string ColumnField { get; set; }
            public string ColumnTitle { get; set; }
        }
        #endregion
    }
}
