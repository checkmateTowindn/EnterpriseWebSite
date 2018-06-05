using EnterpriseWebSite.BLL;
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
    public class AdminAdminManageController : Controller
    {
        DAL.EnterpriseWebSiteContext db = new DAL.EnterpriseWebSiteContext();
        ResultInfo.Info info = new ResultInfo.Info();
        AdminBLL adminBLL = new AdminBLL();
        // GET: AdminAdminManage
        [Login, Authority]
        public ActionResult Index()
        {
            return View();
        }
        [Login, Authority]
        public ActionResult AddOrEdit(int? id)
        {
            ViewBag.Id = id == null ? 0 : id;
            return View();
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        [Login, Authority]
        [HttpPost]
        public JsonResult GetAdminList(string startDate, string endDate, string name)
        {
            info = adminBLL.QueryAdmin(startDate,endDate,name);
            return Json(info);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        [Login, Authority]
        [HttpPost]
        public JsonResult AddAdmin(Admin admin)
        {
            Admin admi = Session["AdminInfo"] as Admin;
            admin.CreateAdminId = admi.Id;
            if (admin.Id == 0)
                info = adminBLL.AddAdmin(admin);
            else
                info = adminBLL.UpdateAdmin(admin);
            return Json(info);
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        [Login, Authority]
        [HttpPost]
        public JsonResult UpdateAdminState(int id,int state)
        {

            info = adminBLL.UpdateAdminState(id,state);
            return Json(info);
        }
        /// <summary>
        /// 删除(实际上是修改状态为删除状态)
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        [Login, Authority]
        [HttpPost]
        public JsonResult DelAdmin(int[] ids)
        {
            info = adminBLL.UpdateAdminStateByIds(ids);
            return Json(info);
        }
        [HttpPost]
        public JsonResult QueryAdminById(int id)
        {
            info = adminBLL.QueryAdminById(id);
            return Json(info);
        }
    }
}