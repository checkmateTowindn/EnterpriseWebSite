using EnterpriseWebSite.Common;
using EnterpriseWebSite.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseWebSite.BLL
{
    public class EnterpriseInfoBLL
    {
        DAL.EnterpriseWebSiteContext db = new DAL.EnterpriseWebSiteContext();
        ResultInfo.Info info = new ResultInfo.Info();
        EnterpriseInfo enterpriseInfo = new EnterpriseInfo();
        /// <summary>
        /// 查询企业信息
        /// </summary>
        /// <returns></returns>
        public ResultInfo.Info QueryInfo(string startDate, string endDate, string title)
        {
            var data = db.EnterpriseInfo.AsQueryable().Select(p => new { p.Id, p.Title, p.Admin, p.AddDate, p.HtmlRegion, p.HtmlRegion.HtmlPage });
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
                limitProps.Add<EnterpriseInfo>(p => new { p.Id, p.Title, p.Admin, p.AddDate, p.HtmlRegion });
                limitProps.Add<Admin>(p => new { p.Id, p.Name });
                limitProps.Add<HtmlRegion>(p => new { p.Id, p.RegionName, p.HtmlPage });
                limitProps.Add<HtmlPage>(p => new { p.Id, p.PageName });
            }
            info.DataObj = list.ToJson(limitProps);
            info.ResultType = ResultInfo.BaseResultType.Success;
            return info;
        }
        /// <summary>
        /// 根据id获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultInfo.Info GetInfoById(int id)
        {
            info.ResultType = ResultInfo.BaseResultType.Error;
            var data = db.EnterpriseInfo.AsQueryable().Where(p => p.Id == id).Select(p => new { p.Id, p.Title,p.Content, p.Admin, p.AddDate, p.HtmlRegion, p.HtmlRegion.HtmlPage });
            if (data != null)
            {
                var list = data.ToList();
                LimitPropsContractResolver limitProps = null;
                if (list.Count() > 0)
                {
                    limitProps = new LimitPropsContractResolver();
                    limitProps.Add<EnterpriseInfo>(p => new { p.Id, p.Title,p.Content, p.Admin, p.AddDate, p.HtmlRegion });
                    limitProps.Add<Admin>(p => new { p.Id, p.Name });
                    limitProps.Add<HtmlRegion>(p => new { p.Id, p.RegionName, p.HtmlPage });
                    limitProps.Add<HtmlPage>(p => new { p.Id, p.PageName });
                }
                info.DataObj = list.ToJson(limitProps);
                info.ResultType = ResultInfo.BaseResultType.Success;
                info.Msg = "获取成功！";
            }
            else
            {
                info.Msg = "获取失败！或许已经删除";
            }
            return info;
        }
        /// <summary>
        /// 根据name获取信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ResultInfo.Info GetInfoByName(string name)
        {
            info.ResultType = ResultInfo.BaseResultType.Error;
            var data = db.EnterpriseInfo.AsQueryable().Where(p => p.Title == name).Select(p => new { p.Id, p.Title, p.Content, p.Admin, p.AddDate, p.HtmlRegion, p.HtmlRegion.HtmlPage });
            if (data != null)
            {
                var list = data.ToList();
                LimitPropsContractResolver limitProps = null;
                if (list.Count() > 0)
                {
                    limitProps = new LimitPropsContractResolver();
                    limitProps.Add<EnterpriseInfo>(p => new { p.Id, p.Title, p.Content, p.Admin, p.AddDate, p.HtmlRegion });
                    limitProps.Add<Admin>(p => new { p.Id, p.Name });
                    limitProps.Add<HtmlRegion>(p => new { p.Id, p.RegionName, p.HtmlPage });
                    limitProps.Add<HtmlPage>(p => new { p.Id, p.PageName });
                }
                info.DataObj = list.ToJson(limitProps);
                info.ResultType = ResultInfo.BaseResultType.Success;
                info.Msg = "获取成功！";
            }
            else
            {
                info.Msg = "获取失败！或许已经删除";
            }
            return info;
        }
        /// <summary>
        /// 添加企业信息
        /// </summary>
        /// <returns></returns>
        public ResultInfo.Info AddInfo(EnterpriseInfo enterpriseInfo)
        {
            if (enterpriseInfo != null)
            {
                info.ResultType = ResultInfo.BaseResultType.IsNull;
                if (string.IsNullOrEmpty(enterpriseInfo.Title))
                    info.Msg = "标题不能为空！";
                else if (enterpriseInfo.HtmlRegion == null)
                    info.Msg = "请选择所在区域！";
                else if (enterpriseInfo.Content == null)
                    info.Msg = "内容不能为空！";
                else
                {
                    var updateData = db.EnterpriseInfo.Where(p => p.Id == enterpriseInfo.Id);
                    if (updateData.Count() > 0)//如果事先存在，那么就执行修改
                    {
                        var data = updateData.FirstOrDefault();
                        data.Content = enterpriseInfo.Content;
                        data.Title = enterpriseInfo.Title;
                        data.Admin = db.Attach(enterpriseInfo.Admin, p => p.Id);
                        data.HtmlRegion = db.Attach(enterpriseInfo.HtmlRegion, p => p.Id);
                        data.AddDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else
                    {
                        enterpriseInfo.Admin = db.Attach(enterpriseInfo.Admin, p => p.Id);
                        enterpriseInfo.HtmlRegion = db.Attach(enterpriseInfo.HtmlRegion, p => p.Id);
                        //基础数据处理
                        enterpriseInfo.AddDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        db.EnterpriseInfo.Add(enterpriseInfo);
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
        /// 修改企业信息
        /// </summary>
        /// <returns></returns>
        public ResultInfo.Info UpdateInfo()
        {
            return info;
        }
        /// <summary>
        /// 删除企业信息
        /// </summary>
        /// <returns></returns>
        public ResultInfo.Info DelInfo(int[] id)
        {
            if (id.Count() > 0)
            {
                var data = db.EnterpriseInfo.Where(p => id.Contains(p.Id));
                if (data != null)
                {
                    foreach (var model in data)
                    {
                        db.EnterpriseInfo.Remove(model);
                    }
                    db.SaveChanges();
                    info.Msg = "删除成功！";
                    info.ResultType = ResultInfo.BaseResultType.Success;
                }
                else
                {
                    info.Msg = "删除失败，或已删除！";
                    info.ResultType = ResultInfo.BaseResultType.NoData;
                }
            }
            else
            {
                info.Msg = "删除失败,没有选择任何数据！";
                info.ResultType = ResultInfo.BaseResultType.NoData;
            }
            return info;
        }
    }
}
