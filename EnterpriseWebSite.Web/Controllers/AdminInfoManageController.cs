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
    public class AdminInfoManageController : Controller
    {
        DAL.EnterpriseWebSiteContext db = new DAL.EnterpriseWebSiteContext();
        ResultInfo.Info info = new ResultInfo.Info();
        BLL.EnterpriseInfoBLL bll = new BLL.EnterpriseInfoBLL();
        /// <summary>
        /// 企业信息管理
        /// </summary>
        /// <returns></returns>
        [Login]
        public ActionResult Index()
        {
            return View();
        }
        [Login]
        public ActionResult AddOrEdit(int? id)
        {
            ViewBag.Id = id == null ? 0 : id;
            return View();
        }
        [HttpPost,Login]
        public JsonResult GetPageList()
        {
            info.DataObj = db.HtmlPage.ToList();
            return Json(info);
        }
        [HttpPost, Login]
        public JsonResult GetPageRegion(int id)
        {
            info.DataObj = db.HtmlRegion.Where(p=>p.HtmlPage.Id==id).ToList();
            return Json(info);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary> 
        /// <returns></returns>
        [HttpPost,Login]
        public JsonResult GetInfoList(string startDate, string endDate, string title)
        {
            info = bll.QueryInfo(startDate,endDate,title);
            return Json(info);
        }
        [HttpGet]
        public ActionResult InfoPreview(int Id)
        {
            ViewBag.Id = Id;
            return View(info);
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetInfoById(int id)
        {
            info = bll.GetInfoById(id);
            return Json(info, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加或者修改
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        [HttpPost, Login]
        [ValidateInput(false)]
        public JsonResult AddInfo(EnterpriseInfo enterpriseInfo)
        {
            Admin adminId = Session["AdminInfo"] as Admin;
            enterpriseInfo.Admin = adminId;
            info = bll.AddInfo(enterpriseInfo);
            return Json(info);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        [HttpPost, Login]
        public JsonResult DelInfo(int[] ids)
        {
            info = bll.DelInfo(ids);
            return Json(info);
        }

    }
}