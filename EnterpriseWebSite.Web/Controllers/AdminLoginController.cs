using EnterpriseWebSite.Common;
using EnterpriseWebSite.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnterpriseWebSite.BLL;
using LoginAndAuthority.Filters;

namespace EnterpriseWebSite.Web.Controllers
{
    public class AdminLoginController : Controller
    {
        ResultInfo.Info info = new ResultInfo.Info();
        BLL.AdminBLL bll = new AdminBLL();
        // GET: AdminLogin
        
        public ActionResult Index()
        {
            if (Session["AdminInfo"] != null)
                return RedirectToAction("../AdminIndex/Index");
            return View();
        }

        #region 用户登录
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userAccount"></param>
        /// <param name="passWord"></param>
        /// <param name="reCaptcha"></param>
        /// <returns>返回登陆相关消息</returns>
        [HttpPost]
        [AllowAnonymous]
        public JsonResult Login(Admin admin)
        {
            info.ResultType = ResultInfo.BaseResultType.Error;
            if (Session["AdminInfo"] != null)
            {
                //跳转
                info.ResultType = ResultInfo.BaseResultType.Exists;
                info.DataObj = "";
                info.Msg = "已登录,请刷新页面！";
            }
            else
            {
                if (string.IsNullOrWhiteSpace(admin.Account))
                {
                    info.Msg = "请输入用户名！";
                }
                else if (string.IsNullOrWhiteSpace(admin.PassWord))
                {
                    info.Msg = "请输入密码！";
                }
                else if (string.IsNullOrWhiteSpace(admin.VerifyCode))
                {
                    info.Msg = "请输入验证码！";
                }
                if (Session["VerifyCodeUsersLogin"] == null)
                {
                    info.Msg = "服务器程序出错，请刷新页面重新登录！";
                }
                else
                {
                    if (Session["VerifyCodeUsersLogin"].ToString().ToLower() != admin.VerifyCode.ToLower())
                    {
                        info.Msg = "验证码输入错误！";
                    }
                    else
                    {
                        info = bll.Login(admin);
                        if (info.ResultType == ResultInfo.BaseResultType.Success)
                        {
                            Session["AdminInfo"] = info.DataObj as Admin;
                        }
                        
                    }
                }

            }
            return Json(info);
        }
        #endregion

        /// <summary>
        /// 注销登录
        /// </summary>
        /// <param name="type">1:切换账号 2:安全退出</param>
        /// <returns></returns>
        public ActionResult QuitAdmin(int type)
        {
            Session["AdminInfo"] = null;
            if (type == 1) return RedirectToAction("../AdminLogin/Index");
            else return RedirectToAction("../AdminLogin/Index");
        }
    }
}