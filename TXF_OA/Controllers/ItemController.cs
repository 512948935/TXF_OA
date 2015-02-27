using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Ninject;
using IBLL;
using System.Data;
using System.Transactions;

namespace TXF_OA
{
    public class ItemController : BaseController
    {
        [Inject]
        public Itb_sys_ModuleBLL moduleBLL { get; set; }
        [Inject]
        public Itb_sys_ItemBLL itemBLL { get; set; }
        [Inject]
        public Itb_item_DepartmentBLL depBLL { get; set; }

        #region 部门信息
        #region List
        public ActionResult DepinfoManage()
        {
            return View();
        }
        //TODO:显示部门信息
        public string GetDepinfoList(string code = "", string disabled = "", string where = "")
        {
            int page = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int pagesize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int total = 0;
            List<WhereField> wheres = JSONStringToList<WhereField>(where);
            DataTable dt = depBLL.GetPageList(page, pagesize, out total, code, disabled, wheres);
            Session["exportData"] = dt;
            string jsonStr = "";
            jsonStr += "{\n";
            jsonStr += "\"total\":" + total + ",\n";
            jsonStr += "\"rows\":" + DataTableToJson(dt) + "";
            jsonStr += "\n}";
            return jsonStr;
        }
        public ActionResult DelDepinfo(string id)
        {
            try
            {
                //删除部门信息
                depBLL.DeleteDepinfo(id);
                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        //导出
        public string ExportDepinfo()
        {
            try
            {
                DataTable dt = (DataTable)Session["exportData"];
                ExportToExcel(dt);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion
        #region Form
        public ActionResult DepinfoEdit()
        {
            return View();
        }
        public ActionResult GetDepinfo(int id)
        {
            try
            {
                tb_item_Department dep = depBLL.SelectT("ID=" + id);
                return Json(new { status = 1, model = dep }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveDepinfo(tb_item_Department model)
        {
            try
            {
                if (depBLL.CheckItemNo(model.ID, model.ItemNo) > 0)
                    throw new Exception("当前代码重复,请重新输入.");
                if (model.ID == 0)
                    depBLL.Add(model);
                else
                    depBLL.Update(model);
                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        #endregion
        #endregion

        #region 数据类别管理
        /// <summary>
        /// 页面信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ItemEdit()
        {
            return View();
        }
        /// <summary>
        /// 保存tb_sys_Item提交
        /// </summary>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveItem(tb_sys_Item entity)
        {
            try
            {
                itemBLL.IsValidNode(entity);
                string preCode = Request["PreCode"] ?? "";
                itemBLL.SaveItemInfo(entity, preCode);
                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult DeleteItem(string NodeCode)
        {
            try
            {
                itemBLL.DeleteByNodeCode(NodeCode);
                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 获取数据管理的类别
        /// </summary>
        /// <param name="Fun_IDs"></param>
        /// <returns></returns>
        public string GetMenuList(int id = 0)
        {
            List<tb_sys_Module> list = moduleBLL.SelectList(where: "IsItem = 1");
            string json = "";
            foreach (tb_sys_Module m in list)
            {
                json += "{\"id\":" + m.ID + ",\"text\":\"" + m.ModuleName + "\"},";
            }
            if (json != "")
            {
                json = "[" + json.TrimEnd(',') + "]";
            }
            else json = "[]";
            return json;
        }
        private DataTable dt;
        public string LoadTree(string pageUrl = "", int id = 0)
        {
            if (dt == null)
                dt = itemBLL.SelectTreeList(pageUrl);
            DataRow[] rows = dt.Select("ParentID =" + id);
            string jsonStr = "[]";
            if (rows.Length > 0)
            {
                jsonStr = "[";
                foreach (DataRow row in rows)
                {
                    jsonStr += ("{\"id\":" + row["ID"] + ",\"text\":\"" + row["NodeCode"] + "." + row["NodeName"] + "\",\"state\":\"" + row["NodeState"] + "\",\"children\":" + LoadTree(pageUrl, Convert.ToInt32(row["ID"])) + ","
                              + "\"attributes\":{\"ID\":\"" + row["ID"] + "\""
                                              + ",\"NodeCode\":\"" + row["NodeCode"] + "\""
                                              + ",\"NodeName\":\"" + row["NodeName"] + "\""
                                              + ",\"NodeType\":\"" + row["NodeType"] + "\""
                                              + ",\"PageUrl\":\"" + row["PageUrl"] + "\""
                                              + "}},");
                }
                jsonStr = jsonStr.TrimEnd(',') + "]";
            }
            return jsonStr;
        }
        //树形节点的展开和折叠
        public void SetNodeState(int id, string state)
        {
            itemBLL.UpdateNodeState(id, state);
        }
        #endregion
    }
}
