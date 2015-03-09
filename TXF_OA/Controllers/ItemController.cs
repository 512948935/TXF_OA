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
        [Inject]
        public Itb_item_CompanyBLL companyBLL { get; set; }
        [Inject]
        public Itb_item_UserBLL userBLL { get; set; }
        [Inject]
        public Itb_item_RoleBLL roleBLL { get; set; }

        #region 公司信息
        #region List
        public ActionResult CompanyinfoManage(bool isRedirect = false,bool choose = false)
        {
            if (isRedirect)
                return Redirect("/Item/ItemEdit?pageUrl=/Item/CompanyinfoManage?choose=" + choose);
            return View();
        }
        //TODO:显示公司信息
        public string GetCompanyinfoList(bool fromCache = false, string code = "", string disabled = "", string where = "")
        {
            int page = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            if (page == 0) return "[]";
            int pagesize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int total = 0;
            List<WhereField> wheres = JSONStringToList<WhereField>(where);
            DataTable dt = companyBLL.GetPageList(fromCache, page, pagesize, out total, code, disabled, wheres);
            Session["exportData"] = dt;
            string jsonStr = "";
            jsonStr += "{\n";
            jsonStr += "\"total\":" + total + ",\n";
            jsonStr += "\"rows\":" + DataTableToJson(dt) + "";
            jsonStr += "\n}";
            return jsonStr;
        }
        public ActionResult DelCompanyinfo(string id)
        {
            try
            {
                //删除部门信息
                companyBLL.DeleteDepinfo(id);
                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        //导出
        public string ExportCompanyinfo()
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
        public ActionResult CompanyinfoEdit()
        {
            return View();
        }
        public ActionResult GetCompanyinfo(int id)
        {
            try
            {
                DataRow row = companyBLL.GetModel(id);
                return Json(new { status = 1, model = DataRowToJson(row) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveCompanyinfo(tb_item_Company model)
        {
            try
            {
                if (companyBLL.CheckItemNo(model.ID, model.ItemNo) > 0)
                    throw new Exception("当前代码重复,请重新输入.");
                if (model.ID == 0)
                    companyBLL.Add(model);
                else
                    companyBLL.Update(model);
                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        #endregion
        #endregion
        
        #region 部门信息
        #region List
        public ActionResult DepinfoManage(bool isRedirect = false, bool choose = false)
        {
            if (isRedirect)
                return Redirect("/Item/ItemEdit?pageUrl=/Item/DepinfoManage?choose=" + choose);
            return View();
        }
        //TODO:显示部门信息
        public string GetDepinfoList(bool fromCache = false, string code = "", string disabled = "", string where = "")
        {
            int page = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            if (page == 0) return "[]";
            int pagesize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int total = 0;
            List<WhereField> wheres = JSONStringToList<WhereField>(where);
            DataTable dt = depBLL.GetPageList(fromCache, page, pagesize, out total, code, disabled, wheres);
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
                DataRow row = depBLL.GetModel(id);
                return Json(new { status = 1, model = DataRowToJson(row) }, JsonRequestBehavior.AllowGet);
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

        #region 人员信息
        #region List
        public ActionResult UserinfoManage(bool isRedirect = false,bool choose = false)
        {
            if (isRedirect)
                return Redirect("/Item/ItemEdit?pageUrl=/Item/UserinfoManage?choose=" + choose);
            return View();
        }
        //TODO:显示公司信息
        public string GetUserinfoList(bool fromCache = false,string code = "", string disabled = "", string where = "")
        {
            int page = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            if (page == 0) return "[]";
            int pagesize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int total = 0;
            List<WhereField> wheres = JSONStringToList<WhereField>(where);
            DataTable dt = userBLL.GetPageList(fromCache,page, pagesize, out total, code, disabled, wheres);
            Session["exportData"] = dt;
            string jsonStr = "";
            jsonStr += "{\n";
            jsonStr += "\"total\":" + total + ",\n";
            jsonStr += "\"rows\":" + DataTableToJson(dt) + "";
            jsonStr += "\n}";
            return jsonStr;
        }
        public ActionResult DelUserinfo(string id)
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
        public string ExportUserinfo()
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
        #region Edit
        public ActionResult UserinfoEdit()
        {
            return View();
        }
        public ActionResult GetUserinfo(int id)
        {
            try
            {
                DataRow row = userBLL.GetModel(id);
                return Json(new { status = 1, model = DataRowToJson(row) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveUserinfo(tb_item_User model)
        {
            try
            {
                if (userBLL.CheckItemNo(model.ID, model.ItemNo) > 0)
                    throw new Exception("当前代码重复,请重新输入.");
                if (model.ID == 0)
                    userBLL.Add(model);
                else
                    userBLL.Update(model);
                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        //TODO:人事信息设置
        public ActionResult SetPersonalinfo()
        {
            return View();
        }
        #endregion
        #endregion

        #region 角色信息
        #region List
        public ActionResult RoleinfoManage(bool isRedirect = false, bool choose = false)
        {
            if (isRedirect)
                return Redirect("/Item/ItemEdit?pageUrl=/Item/RoleinfoManage?choose=" + choose);
            return View();
        }
        //TODO:显示角色信息
        public string GetRoleinfoList(bool fromCache = false, string code = "", string disabled = "", string where = "")
        {
            int page = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            if (page == 0) return "[]";
            int pagesize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int total = 0;
            List<WhereField> wheres = JSONStringToList<WhereField>(where);
            DataTable dt = roleBLL.GetPageList(fromCache, page, pagesize, out total, code, disabled, wheres);
            Session["exportData"] = dt;
            string jsonStr = "";
            jsonStr += "{\n";
            jsonStr += "\"total\":" + total + ",\n";
            jsonStr += "\"rows\":" + DataTableToJson(dt) + "";
            jsonStr += "\n}";
            return jsonStr;
        }
        public ActionResult DelRoleinfo(string id)
        {
            try
            {
                //删除部门信息
                roleBLL.DeleteDepinfo(id);
                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        //导出
        public string ExportRoleinfo()
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
        #region Edit
        public ActionResult RoleinfoEdit()
        {
            return View();
        }
        public ActionResult GetRoleinfo(int id, string code)
        {
            try
            {
                DataRow dt = roleBLL.GetModel(id);
                return Json(new { status = 1, model = DataRowToJson(dt) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveRoleinfo(tb_item_Role model)
        {
            try
            {
                if (roleBLL.CheckItemNo(model.ID, model.ItemNo) > 0)
                    throw new Exception("当前代码重复,请重新输入.");
                if (model.ID == 0)
                    roleBLL.Add(model);
                else
                    roleBLL.Update(model);
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
        public string GetMenuList(string pageUrl = "")
        {
            string where = "IsItem = 1";
            if (!string.IsNullOrEmpty(pageUrl))
            {
                if (pageUrl.LastIndexOf('?') > 0)
                    pageUrl = pageUrl.Substring(0, pageUrl.LastIndexOf('?'));
                where += " AND PageUrl='" + pageUrl + "'";
            }
            List<tb_sys_Module> list = moduleBLL.SelectList(where: where);
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
            if (pageUrl.LastIndexOf('?') > 0)
                pageUrl = pageUrl.Substring(0, pageUrl.LastIndexOf('?'));
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
        public string LoadComboTree(string pageUrl = "", int id = 0)
        {
            if (dt == null)
                dt = itemBLL.SelectTreeList(pageUrl);
            DataRow[] rows = dt.Select("ParentID =" + id);
            string jsonStr = "[]";
            if (rows.Length > 0)
            {
                jsonStr = "[";
                foreach (DataRow row in rows)
                    jsonStr += ("{\"id\":" + row["ID"] + ",\"text\":\"" + row["NodeCode"] + ".\",\"children\":" + LoadComboTree(pageUrl, Convert.ToInt32(row["ID"])) + "},");
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
