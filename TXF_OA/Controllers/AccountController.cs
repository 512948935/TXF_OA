using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using IBLL;
using Ninject;
using System.Text;
using TXF_OA.Models;


namespace TXF_OA.Controllers
{
    public class AccountController : Controller
    {
        [Inject]
        public Itb_item_UserBLL userBLL { get; set; }

        public ActionResult Login()
        {
            return View();
        }
        public FileContentResult CreateCode()
        {
            ValidateCode code = new ValidateCode();
            string codeString = code.CreateValidateCode(5);
            Session["ValidateCode"] = codeString;
            //创建验证码的图片
            byte[] bytes = code.CreateValidateGraphic(codeString);
            //最后将验证码返回;
            return File(bytes, "image/gif");
        }
        [HttpPost]
        public ActionResult LogIn(string name,string pwd,string code)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    throw new Exception("请输入帐号.");
                if (string.IsNullOrEmpty(pwd))
                    throw new Exception("请输入密码.");
                if (string.IsNullOrEmpty(code))
                    throw new Exception("请输入验证码.");
                List<WhereField> wheres = new List<WhereField>(){
                                          new WhereField ("ItemName",name)
                                         ,new WhereField("UserPwd",pwd )
                                         ,new WhereField("Marks",1)
                };
                CheckUserInfo(wheres, code);
                return Content("success");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        private void CheckUserInfo(List<WhereField> wheres, string Code)
        {
            //首先我们拿到系统的验证码
            string sessionCode = Session["ValidateCode"] == null ? new Guid().ToString() : Session["ValidateCode"].ToString();
            //然后我们就将验证码去掉，避免了暴力破解
            Session["ValidateCode"] = new Guid();
            //判断用户输入的验证码是否正确
            if (sessionCode != Code)
                throw new Exception("验证码输入不正确.");
            //如果用户信息存在的话讲用户信息保存到session中
            try
            {
                tb_item_User user = userBLL.SelectT(wheres);
                if (user == null)
                    throw new Exception("用户名或密码不正确.");
                if(user.IsDisabled)
                    throw new Exception("您的帐号已被冻结.");
                Guid uniqueID = Guid.NewGuid();
                HttpCookie cookie = new HttpCookie("ID");//初使化并设置Cookie的名称
                DateTime dt = DateTime.Now;
                TimeSpan ts = new TimeSpan(1, 0, 0, 0, 0);//过期时间为1分钟
                cookie.Expires = dt.Add(ts);//设置过期时间
                cookie.Values.Add("uniqueID", uniqueID.ToString());
                //cookie.Values.Add("userID", user.ID.ToString());
                Response.AppendCookie(cookie);
                
                userBLL.AddToCache(user, uniqueID);
            }
            catch (Exception ex)
            {
                throw new Exception("登录失败," + ex.Message);
            }
        }
        #region 注销
        public ActionResult LogOut()
        {
            HttpCookie cookie = Request.Cookies["ID"];
            if (cookie != null)
            {
                userBLL.Remove(new Guid(cookie["uniqueID"] ?? ""));
                cookie.Expires = DateTime.Now.Add(new TimeSpan(-1, 0, 0, 0));
                Response.AppendCookie(cookie);
            }
            return Redirect("/Account/Login");
        }
        #endregion
    }
}
