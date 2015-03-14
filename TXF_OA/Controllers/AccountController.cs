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
               Session["User"] = user;
            }
            catch (Exception ex)
            {
                throw new Exception("登录失败," + ex.Message);
            }
        }
    }
}
