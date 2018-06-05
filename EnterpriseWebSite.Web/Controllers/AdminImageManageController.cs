using EnterpriseWebSite.BLL;
using EnterpriseWebSite.Common;
using EnterpriseWebSite.DAL;
using LoginAndAuthority.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnterpriseWebSite.Web.Controllers
{
    public class AdminImageManageController : Controller
    {
        DAL.EnterpriseWebSiteContext db = new DAL.EnterpriseWebSiteContext();
        ResultInfo.Info info = new ResultInfo.Info();
        HtmlElementBLL bll = new HtmlElementBLL();
        // GET: AdminImageManage
        [Login]
        public ActionResult Index()
        {
            return View();
        }
        [Login]
        public ActionResult AddOrEdit(int? Id)
        {
            ViewBag.Id = Id == null ? 0 : Id;
            return View();
        }
        [Login]
        public ActionResult ImageListShow(int Id)
        {
            ViewBag.Id = Id;
            return View();
        }
        /// <summary>
        /// 获取该组所有图片
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost, Login]
        public JsonResult GetImageListShow(int Id)
        {
            info = bll.GetImageListShow(Id);
            return Json(info);
        }
        /// <summary>
        /// 图片组后面追加图片
        /// </summary>
        /// <param name="htmlElement"></param>
        /// <returns></returns>
        [Login]
        public ActionResult AddImage(int id,string url,string count)
        {
            ViewBag.Id = id;
            ViewBag.Url = url;
            ViewBag.Count = count;
            return View();
        }
        /// <summary>
        /// 图片组后面追加图片
        /// </summary>
        /// <param name="htmlElement"></param>
        /// <returns></returns>
        [Login, HttpPost]
        public JsonResult AddImage(HtmlElement htmlElement)
        {
            info = bll.AddImage(htmlElement);
            return Json(info);
        }
        
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        [HttpPost, Login]
        public JsonResult GetImageList()
        {
            return Json(info);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        [HttpPost, Login]
        public JsonResult QueryImage()
        {
            return Json(info);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        [HttpPost, Login]
        public JsonResult AddImageList(HtmlElement htmlElement)
        {
            htmlElement.Admin = Session["AdminInfo"] as Admin;
            htmlElement.AddDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            info = bll.AddImageList(htmlElement);
            return Json(info);
        }
        [HttpPost, Login]
        public JsonResult QueryImagesList(string startDate, string endDate, string title,int? id)
        {
            info =bll.QueryImagesList(startDate, endDate, title, id);
            return Json(info);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        [HttpPost, Login]
        public JsonResult UpdateImage(Admin admin)
        {
            return Json(info);
        }
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="ids">图片id</param>
        /// <param name="id">图片所在组的id</param>
        /// <returns></returns>
        [HttpPost, Login]
        public JsonResult DelImage(int[] ids,int id)
        {
            info.ResultType = ResultInfo.BaseResultType.Error;
            //查询出图片所在的数据
            var data = db.Image.Where(p => ids.Contains(p.Id)).ToList();
            if (data.Count() > 0)
            {
                //取出图片的路径，删除这些图片
                foreach (Image imageModel in data)
                {
                    string file = Path.Combine(Server.MapPath(imageModel.Url));
                  
                    if (System.IO.File.Exists(file.Replace("\\", @"\")))
                    {
                        System.IO.File.Delete(file.Replace("\\", @"\"));
                    }
                    System.IO.File.Delete(file);
                }
                info = bll.DelImage(ids, id,data);
            }
               
            return Json(info);
        }
        /// 删除图片组
        /// </summary>
        /// <param name="ids">图片所在组的id</param>
        /// <returns></returns>
        [HttpPost, Login]
        public JsonResult DelAdminImage(int[] ids)
        {
            info.ResultType = ResultInfo.BaseResultType.Error;
            info = bll.DelAdminImage(ids);
            return Json(info);
        }



        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        [HttpPost,Login]
        public JsonResult UpLoadProcess(string id, string name, string type, string lastModifiedDate, int? size, HttpPostedFileBase file,string filePaths)
        {
            string filePathName = string.Empty;

            string localPath = Path.Combine(HttpRuntime.AppDomainAppPath+"/Upload/Image/" + filePaths + "/");
            if (!Directory.Exists(localPath))//如果目录不存在就新建一个
            {
                Directory.CreateDirectory(localPath);
            }
            if (Request.Files.Count == 0)
            {
                return Json(new { jsonrpc = 2.0, error = new { code = 102, message = "保存失败" }, id = "id" });
            }

            string ex = Path.GetExtension(file.FileName);
            filePathName = Guid.NewGuid().ToString("N") + ex;
            if (!System.IO.Directory.Exists(localPath))
            {
                System.IO.Directory.CreateDirectory(localPath);
            }
            string filePath= Path.Combine(localPath, filePathName);
            file.SaveAs(filePath);
            return Json(new
            {
                jsonrpc = "2.0",
                id = id,
                filePath ="/Upload/Image/" + filePaths + "/"+ filePathName
            });
        }
    }
}