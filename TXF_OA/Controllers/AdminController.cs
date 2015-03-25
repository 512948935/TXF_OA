using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Model;
using IBLL;
using Ninject;
using System.Text;

namespace TXF_OA
{
    public class AdminController : BaseController
    {
        [Inject]
        public Itb_sys_ModuleBLL moduleBLL { get; set; }
        [Inject]
        public Itb_sys_ButtonBLL buttonBLL { get; set; }

        #region 模块信息管理
        #region List
        public ActionResult ModuleManage()
        {
            return View();
        }
        public string GetPageList(string code = "", string where = "")
        {
            List<WhereField> wheres = JSONStringToList<WhereField>(where);
            string jsonStr = "";
            int page = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int pagesize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int total = 0;
            DataTable dt = moduleBLL.GetPageList(page, pagesize, out total, code, wheres);
            jsonStr += "{\n";
            jsonStr += "\"total\":" + total + ",\n";
            jsonStr += "\"rows\":" + DataTableToJson(dt) + "";
            jsonStr += "}";
            return jsonStr;
        }
        //删除模块信息
        public ActionResult DeleteModule(string code)
        {
            try
            {
                moduleBLL.DeleteModule(code);
                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        //删除模块下面的页面信息
        public ActionResult DeleteAction(string id)
        {
            try
            {
                moduleBLL.Delete("ID IN " + id);
                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        private DataTable dt;
        public string LoadTree(int id = 0)
        {
            if (dt == null)
                dt = moduleBLL.SelectDataTable(where: "IsPage=0", sort: "ModuleCode");
            DataRow[] rows = dt.Select("ParentID=" + id);
            string jsonStr = "[]";
            if (rows.Length > 0)
            {
                jsonStr = "[";
                foreach (DataRow row in rows)
                {
                    jsonStr += ("{\"id\":" + row["ID"] + ",\"text\":\"" + row["ModuleCode"] + "." + row["ModuleName"] + "\""
                             + ",\"state\":\"" + row["NodeState"] + "\",\"iconCls\":\"" + row["Icon"] + "\""
                             + ",\"children\":" + LoadTree(Convert.ToInt32(row["ID"])) + ""
                             + ",\"attributes\":{\"ID\":\"" + row["ID"] + "\""
                             + ",\"ModuleCode\":\"" + row["ModuleCode"] + "\""
                             + ",\"ModuleName\":\"" + row["ModuleName"] + "\""
                             + ",\"Icon\":\"" + row["Icon"] + "\""
                             + ",\"IsDisabled\":\"" + row["IsDisabled"] + "\""
                             + "}},");
                }
                jsonStr = jsonStr.TrimEnd(',') + "]";
            }
            return jsonStr;
        }

        //树形节点的展开和折叠
        public void SetNodeState(string state, int id)
        {
            moduleBLL.UpdateNodeState(state, id);
        }
        #endregion
        #region Form
        public ActionResult ModuleEdit()
        {
            return View();
        }
        public ActionResult GetModel(int id)
        {
            try
            {
                tb_sys_Module module = moduleBLL.SelectT("ID=" + id);
                Dictionary<int, tb_sys_Button> buttons = null;
                if (!string.IsNullOrEmpty(module.ButtonID))
                {
                    buttons = new Dictionary<int, tb_sys_Button>();
                    List<tb_sys_Button> buttonList = buttonBLL.SelectList("ID IN (" + module.ButtonID + ") AND Marks=1");
                    string[] strs = module.ButtonID.Split(',');
                    for (int i = 0; i < strs.Length; i++)
                    {
                        buttons.Add(i, buttonList.Where(c => c.ID == Convert.ToInt32(strs[i])).FirstOrDefault());
                    }
                }
                return Json(new { status = 1, model = module, buttons = buttons.Values }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult SaveAction(tb_sys_Module module)
        {
            try
            {
                string PreCode = Request["PreCode"] ?? "";
                moduleBLL.SaveModule(module, PreCode);
                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        #endregion
        #endregion

        #region 创建动态图标
        public ActionResult CreateButtonIcon()
        {
            string strHTML = "<table id='tt' border='0' cellpadding='0' cellspacing='0' align='center' class='tablestyle'>";
            string x, y;
            for (var row = 1; row < 26; row++)
            {
                strHTML += "<tr>";
                x = row < 10 ? "0" + row.ToString() : row.ToString();
                for (var col = 1; col < 21; col++)
                {
                    y = col < 10 ? "0" + col.ToString() : col.ToString();
                    strHTML += "<td><a href='#'><img style='display:block;width:18px;height:18px;border:2px solid #fffffb' title='tu" + y + x + "'"
                            + "class='tu" + y + x + "' /></a></td>";
                }
                strHTML += "</tr>";
            }
            strHTML += "<tr>";
            for (var col = 1; col < 15; col++)
            {
                y = col < 10 ? "0" + col.ToString() : col.ToString();
                strHTML += "<td><a href='#'>";
                strHTML += "<img style='display:block;width:18px;height:18px;border:2px solid #fffffb' title='tu" + y + "26' class='tu" + y + "26' />";
                strHTML += "</a></td>";
            }
            strHTML += "</tr></table>";
            return Content(strHTML);
        }
        #endregion
    }
}
