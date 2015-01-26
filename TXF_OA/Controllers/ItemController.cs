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
        public string GetDepinfoList(string code = "", string isDelete = "", string where = "")
        {
            int page = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int pagesize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int total = 0;
            List<WhereField> wheres = new List<WhereField>();
            if (where != "")
                wheres = JSONStringToList<WhereField>(where);
            else
                where = "1=1";
            foreach (WhereField field in wheres)
            {
                if (field.Value.ToString() != "")
                    where += string.Format(" AND {0} {1} '%{2}%'", field.Key, field.Symbol, field.Value);
            }
            DataTable dt = depBLL.GetPageList(page, pagesize, out total, code, isDelete, where);
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
        #region Form
        public ActionResult DepinfoEdit()
        {
            return View();
        }
        public ActionResult GetDepinfo(int id)
        {
            try
            {
                tb_item_Department dep = depBLL.SelectEntity("ID=" + id);
                return Json(new { status = 1, model = dep }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveDepinfo(tb_item_Department model, int typeID = 0)
        {
            try
            {
                tb_sys_Item item = itemBLL.SelectEntity("ID=" + model.ItemID);
                if (item == null)
                {
                    item = new tb_sys_Item();
                    item.ID = model.ItemID;
                    item.NodeType = typeID;
                    item.TableName = "tb_item_Department";
                }
                item.NodeCode = model.ItemNo;
                item.NodeName = model.ItemName;
                IsValidNode(item);
                string preCode = Request["PreCode"];
                using (TransactionScope scope = new TransactionScope())
                {
                    if (model.ID == 0)
                    {
                        //先添加节点
                        model.ItemID = itemBLL.Add(item);
                        depBLL.Add(model);
                    }
                    else
                    {
                        itemBLL.Update(item);
                        depBLL.Update(model);
                    }
                    scope.Complete();
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

        #region 数据字典管理
        /// <summary>
        /// 页面信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ItemEdit()
        {
            return View();
        }
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="Fun_IDs"></param>
        /// <returns></returns>
        public string GetMenuList(int id = 0)
        {
            string where = "IsItem = 1";
            if (id > 0)
                where = "ID =" + id;
            List<tb_sys_Module> list = moduleBLL.SelectList(where: where, sort: "ID");
            string json = "";
            foreach (tb_sys_Module m in list)
            {
                json += "{\"id\":" + m.ID + ",\"text\":\"" + m.ModuleName + "\"},";
            }
            if (json != "") { json = "[" + json.TrimEnd(',') + "]"; }
            else json = "[]";
            return json;
        }
        /// <summary>
        /// 保存tb_sys_Item提交
        /// </summary>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveItemInfo(tb_sys_Item entity)
        {
            try
            {
                IsValidNode(entity);
                if (entity.ID == 0)
                    itemBLL.Add(entity);
                else
                {
                    string preCode = Request["PreCode"];
                    string tableName = itemBLL.GetTabelName(preCode);
                    tb_sys_Item item = itemBLL.SelectEntity("ID=" + entity.ID);
                    item.NodeCode = entity.NodeCode;
                    item.NodeName = entity.NodeName;
                    itemBLL.Update(item);
                    itemBLL.UpdateChildNode(entity.NodeCode, preCode, tableName);
                }
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
                string tableName = itemBLL.GetTabelName(NodeCode);
                if (!string.IsNullOrEmpty(tableName))
                    throw new Exception("当前节点或子节点已存在明细.");
                itemBLL.DeleteByNodeCode(NodeCode);
                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 检测当前节点
        /// </summary>
        /// <param name="item"></param>
        private void IsValidNode(tb_sys_Item item)
        {
            item.NodeCode = item.NodeCode.Trim('.');
            if (item.NodeCode.Equals("")) throw new Exception("输入节点无效.");
            string[] itemNos = item.NodeCode.Split('.');
            //获取最大的级次
            int maxLevelId = itemBLL.GetMaxLevel();
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
                where = string.Format("NodeCode='{0}' AND NodeLevel={1}", tempNo.TrimEnd('.'), item.NodeLevel);
                if (item.ID > 0)
                    where += " AND ID<>" + item.ID;
                tb_sys_Item record = itemBLL.SelectEntity(where);
                if (record == null)
                {
                    if (itemNos.Length == 1)
                    {
                        item.ParentID = 0;
                        //检测节点类型是否重复
                        if (item.ID == 0 && !itemBLL.CheckNodeType(item.NodeType))
                            throw new Exception("当前节点类型已添加.");
                    }
                    else
                    {
                        if (i < itemNos.Length - 1) { throw new Exception("输入的级次超出范围."); }//当前节次没有重复，还有下级节次的情况
                        //查找上级节点
                        string preCode = tempNo.TrimEnd('.').Substring(0, tempNo.TrimEnd('.').LastIndexOf('.'));
                        where = string.Format("NodeCode='{0}' AND NodeLevel={1}", preCode, item.NodeLevel - 1);
                        tb_sys_Item parentItem = itemBLL.SelectEntity(where);
                        if (!string.IsNullOrEmpty(parentItem.TableName))
                            throw new Exception("不能在明细节点下面添加类别节点.");
                        if (parentItem.NodeType != item.NodeType)
                            throw new Exception("类别不一致.");
                        item.ParentID = parentItem.ID;
                    }
                    return;
                }
            }
            throw new Exception("当前代码重复.");
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
                                              + ",\"TableName\":\"" + row["TableName"] + "\""
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
