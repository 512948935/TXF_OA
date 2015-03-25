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
using AutoMapper;
using TXF_OA.Models;
using System.IO;

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
        //[Inject]
        //public Itb_item_UserBLL userBLL { get; set; }
        [Inject]
        public Itb_item_RoleBLL roleBLL { get; set; }
        [Inject]
        public Itb_sys_ButtonBLL buttonBLL { get; set; }
        [Inject]
        public Itb_sys_Role_PermissionBLL role_PermissionBLL { get; set; }

        #region 公司信息
        #region List
        public ActionResult CompanyinfoManage(bool isRedirect = false, int moduleID = 0, bool choose = false)
        {
            if (isRedirect)
                return Redirect("/Item/ItemEdit?pageUrl=/Item/CompanyinfoManage&choose=" + choose);
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
                model.User = onlineUser.User;
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
                model.User = onlineUser.User;
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
                {
                    List<UpdateField> fields = new List<UpdateField>
                    {
                         new UpdateField("ItemID",model.ItemID)
                        ,new UpdateField("ItemNo",model.ItemNo)
                        ,new UpdateField("ItemName",model.ItemName)
                        ,new UpdateField("RealName",model.RealName)
                        ,new UpdateField("UserPwd",model.UserPwd)
                        ,new UpdateField("DepID",model.DepID)
                        ,new UpdateField("RoleID",model.RoleID)
                        ,new UpdateField("IsDisabled",model.IsDisabled)
                        ,new UpdateField("Remark",model.Remark)
                        ,new UpdateField("ModifiedUserID",onlineUser.User.ID)
                        ,new UpdateField("ModifiedBy",onlineUser.User.ItemName)
                        ,new UpdateField("ModifiedOn",DateTime.Now)
                    };
                    userBLL.Update(fields, "ID=" + model.ID);
                }
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

        #region 个人信息
        public ActionResult UserPersonalInfo()
        {
            return View();
        }
        public ActionResult GetPersonalinfo(int id)
        {
            try
            {
                tb_item_User model = userBLL.SelectT("ID=" + id);
                return Json(new { status = 1, model = model }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult SavePersonalInfo(tb_item_User user)
        {
            try
            {
                List<UpdateField> fields = new List<UpdateField>
                {
                     new UpdateField("RealName",user.RealName)
                    ,new UpdateField("Sex",user.Sex)
                    ,new UpdateField("UserDuty",user.UserDuty)
                    ,new UpdateField("Email",user.Email)
                    ,new UpdateField("BirthDay",user.BirthDay)
                    ,new UpdateField("Ethnic",user.Ethnic)
                    ,new UpdateField("IDCardNo",user.IDCardNo)
                    ,new UpdateField("BankNo",user.BankNo)
                    ,new UpdateField("IsWedding",user.IsWedding)
                    ,new UpdateField("Faction",user.Faction)
                    ,new UpdateField("Origin",user.Origin)
                    ,new UpdateField("Household",user.Household)
                    ,new UpdateField("Education",user.Education)
                    ,new UpdateField("Technical",user.Technical)
                    ,new UpdateField("University",user.University)
                    ,new UpdateField("Major",user.Major)
                    ,new UpdateField("JoinTime",user.JoinTime)
                    ,new UpdateField("Phone",user.Phone)
                    ,new UpdateField("FamilyAddress",user.FamilyAddress)
                    ,new UpdateField("IsWorking",user.IsWorking)
                };
                userBLL.Update(fields, "ID=" + user.ID);
                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        #endregion

        #region 上传头像
        public ActionResult UserHeader()
        {
            return View();
        }
        public string UploadImage()
        {
            string qqfile = Request["qqfile"];
            var inputStream = Request.InputStream;
            if (string.IsNullOrEmpty(qqfile))
            {
                //ie浏览器
                HttpPostedFileBase file = new HttpPostedFileWrapper(System.Web.HttpContext.Current.Request.Files[0]);
                qqfile = file.FileName;
                inputStream = file.InputStream;
            }
            string uploadFolder = Url.Content("~/Content/images/UploadFile/TempImg/" + DateTime.Now.ToString("yyyyMM") + "/");
            string imgName = DateTime.Now.ToString("ddHHmmssff");
            string imgType = qqfile.Substring(qqfile.LastIndexOf("."));
            string uploadPath = Server.MapPath(uploadFolder);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            uploadPath = uploadPath + imgName + imgType;
            using (var flieStream = new FileStream(uploadPath, FileMode.Create))
            {
                inputStream.CopyTo(flieStream);
            }
            inputStream.Dispose();
            System.Drawing.Image img = System.Drawing.Image.FromFile(uploadPath);
            if (img.Width > 400 || img.Height > 400)
            {
                System.Drawing.Image newImg = CropImage.GetThumbNailImage(img, 400, 400, true);
                img.Dispose();
                newImg.Save(uploadPath);
            }
            string json = "{\"success\":true,\"message\":\"" + uploadFolder + imgName + imgType + "\"}";
            return json;
        }
        public string SaveImage(int id)
        {
            int x = Convert.ToInt32(Request["x"]);
            int y = Convert.ToInt32(Request["y"]);
            int w = Convert.ToInt32(Request["w"]);
            int h = Convert.ToInt32(Request["h"]);
            string imgsrc = Request["imgsrc"].Substring(0, Request["imgsrc"].LastIndexOf("?"));
            var faceSrc = CropImage.CutImage(imgsrc, x, y, w, h, 48, 48, "_1");
            var avatarSrc = CropImage.CutImage(imgsrc, x, y, w, h, 180, 180, "_2");
            string path = "({\"FaceSrc\":\"" + faceSrc + "\",\"AvatarSrc\":\"" + avatarSrc + "\"})";
            List<UpdateField> fields = new List<UpdateField>{
                     new UpdateField("FaceSrc",faceSrc)
                    ,new UpdateField("AvatarSrc",avatarSrc)
            };
            userBLL.Update(fields, "ID=" + id);
            return path;
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
                model.User = onlineUser.User;
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

        #region 设置权限
        public ActionResult RoleSetting()
        {
            return View();
        }
        public ActionResult GettPermissionButtons(string pageUrl = "", string choose = "")
        {
            try
            {
                DataTable dtPermission = role_PermissionBLL.GetPermissions(true, onlineUser.User.RoleID.Value);
                DataRow row = dtPermission.Select("PageUrl='" + pageUrl + "'").FirstOrDefault();
                Dictionary<int, tb_sys_Button> buttons = null;
                if (row != null && Convert.ToString(row["ButtonID"]) != "")
                {
                    buttons = new Dictionary<int, tb_sys_Button>();
                    List<tb_sys_Button> buttonList = buttonBLL.SelectList("ID IN (" + row["ButtonID"] + ") AND Marks=1");
                    string[] strs = row["ButtonID"].ToString().Split(',');
                    for (int i = 0; i < strs.Length; i++)
                    {
                        tb_sys_Button btn = buttonList.Where(c => c.ID == Convert.ToInt32(strs[i])).FirstOrDefault();
                        if (choose == "True" && btn.ItemName != "search")
                            continue;
                        buttons.Add(i, buttonList.Where(c => c.ID == Convert.ToInt32(strs[i])).FirstOrDefault());
                    }
                }
                string buttonJson = "[]";
                if (buttons != null)
                {
                    buttonJson = "[";
                    foreach (int i in buttons.Keys)
                    {
                        buttonJson += "{\"text\":\"" + buttons[i].ButtonText + "\",\"iconCls\":\"" + buttons[i].ButtonIcon + "\",\"handler\":\"toolbar_" + buttons[i].ItemName + "\"}";
                        if (i < buttons.Keys.Count() - 1)
                            buttonJson += ",\"-\",";
                    }
                    buttonJson += "]";
                }
                return Json(new { status = 1, buttons = buttonJson }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult GetPermissions(int roleID)
        {
            List<tb_sys_Module> modules = moduleBLL.SelectList();
            DataTable permissions = role_PermissionBLL.GetPermissions(false, roleID);
            DataTable newTable = CreateTable();
            foreach (tb_sys_Module module in modules)
            {
                DataRow rowNew = newTable.NewRow();
                rowNew["ID"] = module.ID;
                rowNew["ParentID"] = module.ParentID;
                rowNew["NodeName"] = module.ModuleCode + "." + module.ModuleName;
                rowNew["ModuleCode"] = module.ModuleCode;
                rowNew["ModuleName"] = module.ModuleName;
                rowNew["PermissionID"] = 0;
                rowNew["IsChecked"] = false;
                DataRow row = permissions.Select("ModuleID=" + module.ID).FirstOrDefault();
                string[] isCheckedButtons = { };
                if (row != null)
                {
                    rowNew["PermissionID"] = row["ID"];
                    rowNew["IsChecked"] = row["IsChecked"];
                    isCheckedButtons = row["ButtonID"].ToString().Split(',');
                }
                if (!string.IsNullOrEmpty(module.ButtonID))
                {
                    Dictionary<int, tb_sys_Button> buttons = new Dictionary<int, tb_sys_Button>();
                    List<tb_sys_Button> buttonList = buttonBLL.SelectList("ID IN (" + module.ButtonID + ") AND Marks=1");
                    string[] strs = module.ButtonID.Split(',');
                    for (int i = 0; i < strs.Length; i++)
                    {
                        tb_sys_Button btn = buttonList.Where(c => c.ID == Convert.ToInt32(strs[i])).FirstOrDefault();
                        if (btn == null) continue;
                        if (isCheckedButtons.Contains(strs[i]))
                            btn.IsChecked = true;
                        buttons.Add(i, btn);
                    }
                    rowNew["buttons"] = CreateButtons(module.ID, buttons);
                }
                newTable.Rows.Add(rowNew);
            }
            string jsonStr = "[]";
            if (newTable.Rows.Count > 0)
                jsonStr = DataTableToTreeJson(newTable);
            return Content(jsonStr);
        }
        //TODO:权限保存
        public ActionResult SavePermissions()
        {
            string data = Request["data"];
            try
            {
                List<tb_sys_Role_Permission> permissions = JSONStringToList<tb_sys_Role_Permission>(data);
                permissions.ForEach(c => c.User = onlineUser.User);
                role_PermissionBLL.SavePermissions(permissions);
                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public string CreateButtons(int id, Dictionary<int, tb_sys_Button> buttons)
        {
            string buttonHTML = "<ul>";
            foreach (tb_sys_Button btn in buttons.Values)
            {
                if (btn.IsChecked)
                    buttonHTML += "<li style='float:left'>&nbsp;<input type='checkbox' id='button_" + btn.ID + "'"
                               + " name='module_" + id + "' value=" + btn.ID + " checked=" + btn.IsChecked + " />" + btn.ButtonText + "</li>";
                else
                    buttonHTML += "<li style='float:left'>&nbsp;<input type='checkbox' id='button_" + btn.ID + "'"
                               + " name='module_" + id + "' value=" + btn.ID + " />" + btn.ButtonText + "</li>";

            }
            buttonHTML += "</ul>";
            return buttonHTML;
        }
        private DataTable CreateTable()
        {
            DataTable dt = new DataTable("newTable");
            DataColumn columnField = new DataColumn();
            columnField.DataType = System.Type.GetType("System.Int32");
            columnField.ColumnName = "ID";
            dt.Columns.Add(columnField);

            columnField = new DataColumn();
            columnField.DataType = System.Type.GetType("System.Int32");
            columnField.ColumnName = "ParentID";
            dt.Columns.Add(columnField);

            columnField = new DataColumn();
            columnField.DataType = System.Type.GetType("System.String");
            columnField.ColumnName = "NodeName";
            dt.Columns.Add(columnField);

            columnField = new DataColumn();
            columnField.DataType = System.Type.GetType("System.String");
            columnField.ColumnName = "ModuleCode";
            dt.Columns.Add(columnField);
            columnField = new DataColumn();

            columnField = new DataColumn();
            columnField.DataType = System.Type.GetType("System.String");
            columnField.ColumnName = "ModuleName";
            dt.Columns.Add(columnField);

            columnField = new DataColumn();
            columnField.DataType = System.Type.GetType("System.Int32");
            columnField.ColumnName = "PermissionID";
            dt.Columns.Add(columnField);

            columnField = new DataColumn();
            columnField.DataType = System.Type.GetType("System.Boolean");
            columnField.ColumnName = "IsChecked";
            dt.Columns.Add(columnField);

            columnField = new DataColumn();
            columnField.DataType = System.Type.GetType("System.String");
            columnField.ColumnName = "Buttons";
            dt.Columns.Add(columnField);
            return dt;
        }
        #endregion
        #endregion

        #region 系统按钮
        #region List
        public ActionResult ButtonInfoManage(bool isRedirect = false, bool choose = false)
        {
            if (isRedirect)
                return Redirect("/Item/ItemEdit?pageUrl=/Item/ButtoninfoManage?choose=" + choose);
            return View();
        }
        //TODO:显示角色信息
        public string GetButtoninfoList(bool fromCache = false, string code = "", string disabled = "", string where = "")
        {
            int page = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            if (page == 0) return "[]";
            int pagesize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int total = 0;
            List<WhereField> wheres = JSONStringToList<WhereField>(where);
            DataTable dt = buttonBLL.GetPageList(fromCache, page, pagesize, out total, code, disabled, wheres);
            Session["exportData"] = dt;
            string jsonStr = "";
            jsonStr += "{\n";
            jsonStr += "\"total\":" + total + ",\n";
            jsonStr += "\"rows\":" + DataTableToJson(dt) + "";
            jsonStr += "\n}";
            return jsonStr;
        }
        public ActionResult DelButtoninfo(string id)
        {
            try
            {
                buttonBLL.DeleteDepinfo(id);
                return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        //导出
        public string ExportButtoninfo()
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
        public ActionResult ButtoninfoEdit()
        {
            return View();
        }
        public ActionResult GetButtoninfo(int id, string code)
        {
            try
            {
                DataRow dt =buttonBLL.GetModel(id);
                return Json(new { status = 1, model = DataRowToJson(dt) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveButtoninfo(tb_sys_Button model)
        {
            try
            {
                model.User = onlineUser.User;
                if (roleBLL.CheckItemNo(model.ID, model.ItemNo) > 0)
                    throw new Exception("当前代码重复,请重新输入.");
                if (model.ID == 0)
                    buttonBLL.Add(model);
                else
                    buttonBLL.Update(model);
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
