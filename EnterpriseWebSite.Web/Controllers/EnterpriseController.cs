using EnterpriseWebSite.BLL;
using EnterpriseWebSite.Common;
using EnterpriseWebSite.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnterpriseWebSite.Web.Controllers
{
    public class EnterpriseController : Controller
    {
        EnterpriseWebSiteContext db = new DAL.EnterpriseWebSiteContext();
        ResultInfo.Info info = new ResultInfo.Info();
        HtmlElementBLL bll = new HtmlElementBLL();
        EnterpriseInfoBLL EnterpriseBll = new EnterpriseInfoBLL();
        MessageBLL messageBll = new MessageBLL();
        // GET: Enterprise
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int? Id)
        {
            ViewBag.Id = Id == null ? 0 : Id;
            return View();
        }
        /// <summary>
        /// 联系我们页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ConcatUs(int? Id)
        {
            ViewBag.Id = Id == null ? 0 : Id;
            return View();
        }
        /// <summary>
        /// 经典案例页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Case(int? Id)
        {
            ViewBag.Id = Id == null ? 0 : Id;
            return View();
        }
        /// <summary>
        /// 关于我们页面
        /// </summary>
        /// <returns></returns>
        public ActionResult About(int? Id)
        {
            ViewBag.Id = Id == null ? 0 : Id;
            return View();
        }
        /// <summary>
        /// 留言
        /// </summary>
        /// <returns></returns>
        public ActionResult Message()
        {
            return View();
        }
        /// <summary>
        /// 服务范围页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Service(int? Id)
        {
            ViewBag.Id = Id == null ? 0 : Id;
            return View();
        }
        /// <summary>
        /// 获取图片信息
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetImageListShow(string title)
        {
            info = bll.GetImageList(title);
            return Json(info);
        }
        /// <summary>
        /// 获取企业信息
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetInfoByName(string Name)
        {
            info = EnterpriseBll.GetInfoByName(Name);
            return Json(info);
        }

        /// <summary>
        /// 添加留言
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddMessage(Message message)
        {
            message.IsAdmin = false;
            message.UpperLeve = 0;
            info.ResultType = ResultInfo.BaseResultType.Error;
            if (Com.isTelephone(message.Mobile))
                info.Msg = "电话号码格式不正确！";
            if (string.IsNullOrEmpty(message.Nick))
                info.Msg = "请填写昵称";
            if (string.IsNullOrEmpty(message.MessageContent))
                info.Msg = "请填写内容";
            if (Session["VerifyCodeUsersLogin"] == null)
            {
                info.Msg = "服务器程序出错，请刷新页面重试！";
            }
            else
            {
                info = messageBll.AddMessage(message);
            }

            return Json(info);
        }

    }
}