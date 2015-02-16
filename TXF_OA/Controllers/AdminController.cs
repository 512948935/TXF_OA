using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Transactions;
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
        public Itb_item_DepartmentBLL depBLL { get; set; }

        #region 部门信息管理
        #region List
        public ActionResult DepartmentManage()
        {
            return View();
        }
        public string GetDepinfoList(string code = "", string isDelete = "", string where = "")
        {
            int page = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int pagesize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int total = 0;
            if (string.IsNullOrEmpty(where)) where = "1=1";
            List<WhereField> wheres = new List<WhereField>();
            if (where != "1=1")
            {
                wheres = JSONStringToList<WhereField>(where);
                where = "1=1";
            }
            foreach (WhereField field in wheres)
            {
                if (field.Value.ToString() != "")
                    where += string.Format(" AND {0} {1} '%{2}%'", field.Key, field.Symbol, field.Value);
            }
            DataTable dt = null; //depBLL.GetList(page, pagesize, out total, code, isDelete, where);
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
                depBLL.Delete(id);
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
        public ActionResult DepartmentEdit()
        {
            return View();
        }
        #endregion

        #region 模块信息管理
        #region list
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
        public ActionResult DeleteAction(string id)
        {
            try
            {
                moduleBLL.Delete(" ID IN " + id);
                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
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
                             + ",\"IsDelete\":\"" + row["IsDelete"] + "\""
                             + "}},");
                }
                jsonStr = jsonStr.TrimEnd(',') + "]";
            }
            return jsonStr;
        }
        /// <summary>
        /// 检测当前节点
        /// </summary>
        /// <param name="item"></param>
        private void IsValidNode(tb_sys_Module item)
        {
            item.ModuleCode = item.ModuleCode.Trim('.');
            if (item.ModuleCode.Equals("")) throw new Exception("输入节点无效.");
            string[] itemNos = item.ModuleCode.Split('.');
            //获取最大的级次
            int maxLevelId = moduleBLL.GetMaxLevel();
            if (itemNos.Length - maxLevelId > 1) throw new Exception("输入的级次超出范围.");
            if (maxLevelId == 0)//未添加任何节点，直接返回ParentID
            {
                item.ParentID = 0;
                item.NodeLevel = 1;
                return;
            }
            string tempNo = "";
            string where = "1=1";
            for (int i = 0; i < itemNos.Length; i++)
            {
                item.NodeLevel = i + 1;
                tempNo += itemNos[i] + ".";
                //查找记录
                where = string.Format("ModuleCode='{0}' AND NodeLevel={1}", tempNo.TrimEnd('.'), item.NodeLevel);

                if (item.ID > 0)
                    where += " AND ID<>" + item.ID;
                tb_sys_Module record = moduleBLL.SelectT(where);
                if (record == null)
                {
                    if (itemNos.Length == 1)
                        item.ParentID = 0;
                    else
                    {
                        if (i < itemNos.Length - 1) { throw new Exception("输入的级次超出范围."); }//当前节次没有重复，还有下级节次的情况
                        //查找上级节点
                        string preCode = tempNo.TrimEnd('.').Substring(0, tempNo.TrimEnd('.').LastIndexOf('.'));
                        where = string.Format("ModuleCode='{0}' AND NodeLevel={1}", preCode, item.NodeLevel - 1);
                        tb_sys_Module parentItem = moduleBLL.SelectT(where);
                        if (parentItem.IsPage)
                            throw new Exception("不能在页面节点下面添加模块.");
                        item.ParentID = parentItem.ID;
                    }
                    return;
                }
            }
            throw new Exception("当前代码重复.");
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
                tb_sys_Module model = moduleBLL.SelectT("ID=" + id);
                return Json(new { status = 1, model = model }, JsonRequestBehavior.AllowGet);
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
                IsValidNode(module);
                if (module.ID == 0)
                    moduleBLL.Add(module);
                else
                {
                    string preCode = Request["PreCode"];
                    using (TransactionScope scope = new TransactionScope())
                    {
                        moduleBLL.Update(module);
                        if (!module.IsPage)
                            moduleBLL.UpdateChildNode(module.ModuleCode, preCode);
                        scope.Complete();
                    }
                }
                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        #endregion
        #endregion
       
    }
}
