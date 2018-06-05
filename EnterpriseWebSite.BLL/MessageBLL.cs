using EnterpriseWebSite.Common;
using EnterpriseWebSite.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseWebSite.BLL
{
    public class MessageBLL
    {
        EnterpriseWebSiteContext db = new EnterpriseWebSiteContext();
        Admin adminModel = new Admin();
        Message messageModel = new Message();
        ResultInfo.Info info = new ResultInfo.Info();
        /// <summary>
        /// 查询留言
        /// </summary>
        /// <returns></returns>
        public ResultInfo.Info QueryMessage(string startDate, string endDate, string keyWord,string htmlPageId,bool? isAdmin)
        {
            var data = db.Message.AsQueryable();
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
            if (!string.IsNullOrEmpty(keyWord))
                data = data.Where(p => p.Mobile.Contains(keyWord)||p.MessageContent.Contains(keyWord)|| p.Nick.Contains(keyWord));
            if (!string.IsNullOrEmpty(htmlPageId))
                data = data.Where(p => p.HtmlPage.Id ==Convert.ToInt32(htmlPageId));
            if(isAdmin!=null)
                data = data.Where(p => p.IsAdmin ==isAdmin);
            var list = data.OrderByDescending(p => p.Id).ToList();
            LimitPropsContractResolver limitProps = null;
            if (list.Count() > 0)
            {
                limitProps = new LimitPropsContractResolver();
                limitProps.Add<Message>(p => new { p.Id, p.MessageContent, p.Admin, p.AddDate, p.HtmlPage, p.Mobile, p.Nick,p.UpperLeve });
                limitProps.Add<Admin>(p => new { p.Id, p.Name });
                limitProps.Add<HtmlPage>(p => new { p.Id, p.PageName });
            }
            info.DataObj = list.ToJson(limitProps);
            info.ResultType = ResultInfo.BaseResultType.Success;
            return info;
        }
        /// <summary>
        /// 根据所属页面和回复时间获取留言
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ResultInfo.Info GeMessageListByOwnedPage(Message message)
        {
            if (message != null)
            {
                var lst = db.Message.Where(p => p.HtmlPage.Id == message.HtmlPage.Id).ToList();
                info.DataObj = lst;
                info.Msg = "获取成功";
                info.ResultType = ResultInfo.BaseResultType.Success;
            }
            else {
                info.Msg = "获取失败";
                info.ResultType = ResultInfo.BaseResultType.Error;
            }
            return info;
        }
        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ResultInfo.Info AddMessage(Message message)
        {
            if (message != null)
            {
                if (message.Id != 0)//说明是修改
                {
                    var data = db.Message.Where(p => p.Id == message.Id).FirstOrDefault();
                    if (data != null)
                    {
                        data = message;
                        data.Admin = db.Attach(message.Admin, p => p.Id);
                        data.HtmlPage = db.Attach(message.HtmlPage, p => p.Id);
                    }
                    else {
                        info.Msg = "未找到您要修改的留言！";
                        info.ResultType = ResultInfo.BaseResultType.Error;
                        return info;
                    }
                }
                else
                {
                    message.Admin = db.Attach(message.Admin, p => p.Id);
                    message.HtmlPage = db.Attach(message.HtmlPage, p => p.Id);
                    db.Message.Add(message);
                }
                db.SaveChanges();
                info.Msg = "保存成功";
                info.ResultType = ResultInfo.BaseResultType.Success;
            }
            else
            {
                info.Msg = "保存失败";
                info.ResultType = ResultInfo.BaseResultType.Error;
            }
            return info;
        }
        /// <summary>
        /// 修改评论
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ResultInfo.Info UpdateMessage(Message message)
        {
            if (message != null)
            {

                using (var tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        var obj = db.Message.Where(p => p.Id == message.Id).FirstOrDefault();
                        //删除原有的
                        db.Message.Remove(obj);
                        //添加新的
                        db.Message.Add(message);
                        db.SaveChanges();
                        //提交事务
                        tran.Commit();
                        info.ResultType = ResultInfo.BaseResultType.Success;
                        info.Msg = "修改成功";
                    }
                    catch (Exception ex)
                    {
                        info.ResultType = ResultInfo.BaseResultType.Error;
                        info.Msg = "修改失败，事务回滚," + ex;
                        tran.Rollback();
                    }
                }
            }
            else
            {
                info.ResultType = ResultInfo.BaseResultType.IsNull;
                info.Msg = "非法操作，传入数据为空。";
            }
            return info;
        }
        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ResultInfo.Info DelMessage(int[] id)
        {
            if (id.Count()>0)
            {
                var data = db.Message.Where(p => id.Contains(p.Id));
                if (data != null)
                {
                    foreach (var model in data)
                    {
                        db.Message.Remove(model);
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
                info.ResultType = ResultInfo.BaseResultType.IsNull;
                info.Msg = "非法操作，传入数据为空。";
            }
            return info;
        }
        /// <summary>
        /// 获取指定id的留言
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultInfo.Info GetMessageById(int id)
        {
            var data = db.Message.Where(p => p.Id == id).Select(p=>new { p.Id, p.MessageContent, p.Admin, p.AddDate, p.HtmlPage, p.Mobile, p.Nick, p.UpperLeve }).ToList();
            if (data != null)
            {
                LimitPropsContractResolver limitProps = null;
                if (data.Count() > 0)
                {
                    limitProps = new LimitPropsContractResolver();
                    limitProps.Add<Message>(p => new { p.Id, p.MessageContent, p.Admin, p.AddDate, p.HtmlPage, p.Mobile, p.Nick, p.UpperLeve });
                    limitProps.Add<Admin>(p => new { p.Id, p.Name });
                    limitProps.Add<HtmlPage>(p => new { p.Id, p.PageName });
                }
                info.DataObj = data.ToJson(limitProps);
                info.ResultType = ResultInfo.BaseResultType.Success;
            }
            else
            {
                info.Msg = "该留言不存在，或已被删除！";
                info.ResultType = ResultInfo.BaseResultType.Error;
            }
            return info;
        }
        /// <summary>
        /// 获取与指定id相关的所有留言
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultInfo.Info GetMessageByIdList(int id)
        {
            var data=  GetMessageByIdListFnc(id);
            if (data.Count()> 0)
            {
                var fatherData= db.Message.Where(p => p.UpperLeve == id).ToList();
                if (fatherData.Count > 0)
                {
                    //使用递归，查出所有
                    
                }
                else
                {

                }
                info.DataObj = data;
                info.ResultType = ResultInfo.BaseResultType.Success;
            }
            else
            {
                info.Msg = "该留言不存在，或已被删除！";
                info.ResultType = ResultInfo.BaseResultType.Error;
            }
            return info;
        }
        public List<Message> GetMessageByIdListFnc(int id)
        {
            // var data = db.Message.Where(p => p.Id == id).Select(p => new { p.Id, p.MessageContent, p.Admin, p.AddDate, p.HtmlPage, p.Mobile, p.Nick, p.UpperLeve }).ToList();
            var data = db.Message.Where(p => p.Id == id).ToList();
            return data;
        }

    }
}
