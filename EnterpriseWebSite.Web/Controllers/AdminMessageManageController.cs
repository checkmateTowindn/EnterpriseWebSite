using EnterpriseWebSite.Common;
using EnterpriseWebSite.DAL;
using LoginAndAuthority.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnterpriseWebSite.Web.Controllers
{
    public class AdminMessageManageController : Controller
    {
        DAL.EnterpriseWebSiteContext db = new DAL.EnterpriseWebSiteContext();
        ResultInfo.Info info = new ResultInfo.Info();
        BLL.MessageBLL bll = new BLL.MessageBLL();
        // GET: AdminMessageManage
        [Login]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 管理员回复页面
        /// </summary>
        /// <returns></returns>
        [Login]
        public ActionResult AdminReply(int? Id)
        {
            ViewBag.Id = Id == null ? 0 : Id;
            return View();
        }
        /// <summary>
        /// 获取与该id相关的留言
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetMessageByIdList(int Id)
        {
            if (Id != 0)
                info = bll.GetMessageByIdList(Id);
            else {
                info.Msg = "该留言已被删除！";
                info.ResultType = ResultInfo.BaseResultType.Error;
            }
            return Json(info);
        }
        /// <summary>
        /// 管理员回复
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [Login,HttpPost]
        public ActionResult AdminReply(Message message)
        {
            message.IsAdmin = false;
            message.Admin = Session["AdminInfo"] as Admin;
            message.Mobile = message.Admin.Mobile;
            message.Nick = message.Admin.Name;
            return View();
        }
        /// <summary>
        /// 添加留言页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AddOrEdit(int? Id)
        {
            ViewBag.Id = Id == null ? 0 : Id;
            return View();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult QueryMessage(string startDate,string endDate, string keyWord,string htmlPageId,bool? isAdmin)
        {
            info = bll.QueryMessage(startDate, endDate, keyWord, htmlPageId,isAdmin);
            return Json(info);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddMessage(Message message)
        {
            message.IsAdmin = false;
            message.UpperLeve = 0;
            info.ResultType = ResultInfo.BaseResultType.Error;
            if(Com.isTelephone(message.Mobile))
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
                if (Session["VerifyCodeUsersLogin"].ToString().ToLower() != message.IdentifyingCode.ToLower())
                {
                    info.Msg = "验证码输入错误！";
                }
                else
                {
                    info = bll.AddMessage(message);
                }
            }
           
            return Json(info);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateInfo(Message message)
        {
            return Json(info);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DelInfo(Message message)
        {
            return Json(info);
        }
        [HttpPost]
        public JsonResult GetMessageById(int id)
        {
            info = bll.GetMessageById(id);
            return Json(info);
        }
    }
}