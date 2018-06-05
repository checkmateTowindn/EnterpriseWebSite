using EnterpriseWebSite.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoginAndAuthority.Filters
{
    /// <summary>
    /// 登陆验证
    /// </summary>
    public class LoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["AdminInfo"] == null)
            {
                filterContext.Result = new ContentResult();
                filterContext.HttpContext.Response.Clear();//这里是关键，清除在返回前已经设置好的标头信息，这样后面的跳转才不会报错
                filterContext.HttpContext.Response.BufferOutput = true;//设置输出缓冲
                filterContext.HttpContext.Response.Redirect("~/AdminLogin/Index");
                return;
            }
            else
            {
                Admin admin = filterContext.HttpContext.Session["AdminInfo"] as Admin;
                if (admin.State == State.Disable)
                {
                    filterContext.Result = new ContentResult();
                    filterContext.HttpContext.Response.Clear();//这里是关键，清除在返回前已经设置好的标头信息，这样后面的跳转才不会报错
                    filterContext.HttpContext.Response.BufferOutput = true;//设置输出缓冲
                    filterContext.HttpContext.Response.Redirect("~/AdminLogin/Index");
                    return;
                }
            }
        }
    }
    /// <summary>
    /// 权限验证
    /// </summary>
    public class AuthorityAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["AdminInfo"] != null)
            {
                Admin admin = filterContext.HttpContext.Session["AdminInfo"] as Admin;
                if (admin.Authority != Authority.Admin && admin.Authority != Authority.Leader)
                {
                    filterContext.Result = new ContentResult();
                    filterContext.HttpContext.Response.Clear();//这里是关键，清除在返回前已经设置好的标头信息，这样后面的跳转才不会报错
                    filterContext.HttpContext.Response.BufferOutput = true;//设置输出缓冲
                    filterContext.HttpContext.Response.Redirect("~/AdminLogin/Index");
                    return;
                }
            }
            else {
                filterContext.Result = new ContentResult();
                filterContext.HttpContext.Response.Clear();//这里是关键，清除在返回前已经设置好的标头信息，这样后面的跳转才不会报错
                filterContext.HttpContext.Response.BufferOutput = true;//设置输出缓冲
                filterContext.HttpContext.Response.Redirect("~/AdminLogin/Index");
                return;
            }
        }
    }
}