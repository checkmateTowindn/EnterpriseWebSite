using EnterpriseWebSite.Common;
using EnterpriseWebSite.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EnterpriseWebSite.BLL
{
    public class HtmlElementBLL
    {
        ResultInfo.Info info = new ResultInfo.Info();
        HtmlElement htmlElement = new HtmlElement();
        EnterpriseWebSiteContext db = new DAL.EnterpriseWebSiteContext();
        public ResultInfo.Info AddImageList(HtmlElement htmlElement)
        {
            if (htmlElement != null)
            {
                info.ResultType = ResultInfo.BaseResultType.IsNull;
                if (string.IsNullOrEmpty(htmlElement.Title))
                    info.Msg = "标题不能为空！";
                else if (htmlElement.HtmlRegion == null)
                    info.Msg = "请选择所在区域！";
                else if (htmlElement.Remark == null)
                    info.Msg = "备注不能为空！";
                else
                {
                    
                    if (htmlElement.Id != 0)//修改
                    {
                        var data = db.HtmlElement.Where(p => p.Id==htmlElement.Id).FirstOrDefault();
                        data.Admin= db.Attach(htmlElement.Admin, p => p.Id);
                        data.HtmlRegion= db.Attach(htmlElement.HtmlRegion, p => p.Id);
                        data.Title = htmlElement.Title;
                        data.Height = htmlElement.Height;
                        data.Width = htmlElement.Width;
                        data.Remark = htmlElement.Remark;
                    }
                    else
                    {
                        if (htmlElement.Image == null)
                        {
                            info.Msg = "必须选择图片！";
                            return info;
                        }
                        else
                        {
                            htmlElement.Admin = db.Attach(htmlElement.Admin, p => p.Id);
                            htmlElement.HtmlRegion = db.Attach(htmlElement.HtmlRegion, p => p.Id);
                            db.HtmlElement.Add(htmlElement);
                        }
                    }
                    db.SaveChanges();
                    info.Msg = "保存成功";
                    info.ResultType = ResultInfo.BaseResultType.Success;

                }
            }
            else
            {
                info.Msg = "保存失败";
                info.ResultType = ResultInfo.BaseResultType.Error;
            }
            return info;
        }
        /// <summary>
        /// 查询符合条件的图片组
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public ResultInfo.Info QueryImagesList(string startDate, string endDate, string title,int? id)
        {
            var data = db.HtmlElement.AsQueryable().Select(p => new { p.Id, p.Title, p.Admin, p.AddDate, p.HtmlRegion, p.HtmlRegion.HtmlPage,p.Image });
            if (!string.IsNullOrEmpty(startDate))
            {
                var time = Convert.ToDateTime(startDate);
                data = data.Where(p => Convert.ToDateTime(p.AddDate) > time);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                var time = Convert.ToDateTime(endDate);
                data = data.Where(p => Convert.ToDateTime(p.AddDate) < time);
            }
            if (!string.IsNullOrEmpty(title))
                data = data.Where(p => p.Title.Contains(title));
            var list = data.OrderByDescending(p => p.AddDate).ToList();
            LimitPropsContractResolver limitProps = null;
            if (list.Count() > 0)
            {
                limitProps = new LimitPropsContractResolver();
                limitProps.Add<HtmlElement>(p => new { p.Id, p.Title, p.Admin, p.AddDate, p.HtmlRegion, p.Image });
                limitProps.Add<Admin>(p => new { p.Id, p.Name });
                limitProps.Add<HtmlRegion>(p => new { p.Id, p.RegionName, p.HtmlPage });
                limitProps.Add<HtmlPage>(p => new { p.Id, p.PageName });
                limitProps.Add<Image>(p => new { p.Id, p.Name ,p.Sort,p.Url,p.Alt});
            }
            info.DataObj = list.ToJson(limitProps);
            info.ResultType = ResultInfo.BaseResultType.Success;
            return info;
        }
        /// <summary>
        /// 获取该图片组的信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ResultInfo.Info GetImageListShow(int Id)
        {
            var data = db.HtmlElement.Where(p => p.Id == Id).Select(p => new { p.Id, p.Title, p.Admin, p.AddDate, p.HtmlRegion, p.HtmlRegion.HtmlPage, p.Image,p.Width,p.Height,p.Remark });
            var list = data.ToList();
            LimitPropsContractResolver limitProps = null;
            if (list.Count() > 0)
            {
                limitProps = new LimitPropsContractResolver();
                limitProps.Add<HtmlElement>(p => new { p.Id, p.Title, p.Admin, p.AddDate, p.HtmlRegion, p.Image });
                limitProps.Add<Admin>(p => new { p.Id, p.Name });
                limitProps.Add<HtmlRegion>(p => new { p.Id, p.RegionName, p.HtmlPage });
                limitProps.Add<HtmlPage>(p => new { p.Id, p.PageName });
                limitProps.Add<Image>(p => new { p.Id, p.Name, p.Sort, p.Url, p.Alt });
                info.DataObj = list.ToJson(limitProps);
                info.ResultType = ResultInfo.BaseResultType.Success;
            }
            else
            {
                info.ResultType = ResultInfo.BaseResultType.Error;
                info.Msg = "未查询到该图片组！";
            }
            
            return info;
        }

        /// <summary>
        /// 获取该图片组的信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ResultInfo.Info GetImageList(string tile)
        {
            var data = db.HtmlElement.Where(p => p.Title == tile).Select(p => new { p.Id, p.Title, p.Admin, p.AddDate, p.HtmlRegion, p.HtmlRegion.HtmlPage, p.Image, p.Width, p.Height, p.Remark });
            var list = data.ToList();
            LimitPropsContractResolver limitProps = null;
            if (list.Count() > 0)
            {
                limitProps = new LimitPropsContractResolver();
                limitProps.Add<HtmlElement>(p => new { p.Id, p.Title, p.Admin, p.AddDate, p.HtmlRegion, p.Image });
                limitProps.Add<Admin>(p => new { p.Id, p.Name });
                limitProps.Add<HtmlRegion>(p => new { p.Id, p.RegionName, p.HtmlPage });
                limitProps.Add<HtmlPage>(p => new { p.Id, p.PageName });
                limitProps.Add<Image>(p => new { p.Id, p.Name, p.Sort, p.Url, p.Alt });
                info.DataObj = list.ToJson(limitProps);
                info.ResultType = ResultInfo.BaseResultType.Success;
            }
            else
            {
                info.ResultType = ResultInfo.BaseResultType.Error;
                info.Msg = "未查询到该图片组！";
            }

            return info;
        }
        /// <summary>
        /// 追加图片组中的图片
        /// </summary>
        /// <param name="htmlElement"></param>
        /// <returns></returns>
        public ResultInfo.Info AddImage(HtmlElement htmlElement)
        {
            info.ResultType = ResultInfo.BaseResultType.Error;
            if (htmlElement != null)
            {
                var model = db.HtmlElement.Where(p => p.Id == htmlElement.Id);
                if (model.Count() > 0)
                {
                    
                    //先查出来，处理之后，再直接赋值到data中
                    var dataImg= db.HtmlElement.Where(p => p.Id == htmlElement.Id).Select(p=>p.Image).FirstOrDefault() ;
                    for (int i = 0; i < htmlElement.Image.Count(); i++)
                    {
                        dataImg.Add(htmlElement.Image[i]);
                    }
                    var data = model.FirstOrDefault();
                    data.Image= dataImg;
                    db.SaveChanges();
                    info.Msg = "保存成功！";
                    info.ResultType = ResultInfo.BaseResultType.Success;
                }
                else
                {
                    info.Msg = "该图片组不存在！";
                }
            }
            return info;
        }
        /// <summary>
        /// 编辑和删除图片信息
        /// </summary>
        /// <param name="htmlElement"></param>
        /// <returns></returns>
        public ResultInfo.Info UpdateImage(HtmlElement htmlElement)
        {
            info.ResultType = ResultInfo.BaseResultType.Error;
            if (htmlElement != null)
            {
                
                var model = db.HtmlElement.Where(p => p.Id == htmlElement.Id);
                if (model.Count() > 0)
                {
                    var data = model.FirstOrDefault();
                    data.Image = htmlElement.Image;
                    db.SaveChanges();
                }
                else
                {
                    info.Msg = "该图片组不存在！";
                }
            }
            return info;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">图片群id</param>
        /// <param name="id">图片所在id</param>
        /// <returns></returns>
        public ResultInfo.Info DelImage(int[] ids,int id,List<Image> data)
        {
            info.ResultType = ResultInfo.BaseResultType.Error;
            //查询出图片所在的数据
            if (data.Count() > 0) {
                var dataObj = db.Image.Where(p => ids.Contains(p.Id));
                ////图片表中删除这些数据
                foreach (var model in dataObj)
                {
                    db.Image.Remove(model);
                }
                db.SaveChanges();
             
                info.Msg = "删除成功！";
                info.ResultType = ResultInfo.BaseResultType.Success;
            }
            else
            {
                info.Msg = "未查询到您要删除的图片信息！";
            }
            return info;
        }
        /// <summary>
        /// 删除图片组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultInfo.Info DelAdminImage(int[] id)
        {
            info.ResultType = ResultInfo.BaseResultType.Error;
            var data = db.HtmlElement.Where(p => id.Contains(p.Id));
            //查询出图片所在的数据
            if (data.Count() > 0)
            {

                var dataObj = db.HtmlElement.AsQueryable().Where(p => id.Contains(p.Id)).Select(p => new { p.Id, p.Title, p.Admin, p.AddDate, p.HtmlRegion, p.HtmlRegion.HtmlPage, p.Image });
                List<Image> imageList = new List<Image>();
                foreach (var model in dataObj) {
                    if (model.Image!=null)
                    imageList.AddRange(model.Image);
                }
                ////图片表中删除这些数据
                foreach (var model in imageList)
                {
                    db.Image.Remove(model);
                }
                db.SaveChanges();

                foreach (var model in data)
                {
                    db.HtmlElement.Remove(model);
                }
                db.SaveChanges();
                info.Msg = "删除成功！";
                info.ResultType = ResultInfo.BaseResultType.Success;
            }
            else
            {
                info.Msg = "未查询到您要删除的图片组信息！";
            }
            return info;
        }
    }
}
