using EnterpriseWebSite.Common;
using EnterpriseWebSite.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseWebSite.BLL
{
    public class AdminBLL
    {
        EnterpriseWebSiteContext db = new EnterpriseWebSiteContext();
        Admin adminModel = new Admin();
        ResultInfo.Info info = new ResultInfo.Info();
        public ResultInfo.Info QueryAdmin(string startDate, string endDate,string name)
        {
            var data = db.Admin.AsQueryable().Select(p => new { p.Id,p.Account,p.Authority,p.AddDate,p.Mobile,p.Sex,p.Name,p.State });
            if (!string.IsNullOrEmpty(startDate))
            {
              var time=  Convert.ToDateTime(startDate);
                data = data.Where(p => Convert.ToDateTime(p.AddDate)>time);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                var time = Convert.ToDateTime(endDate);
                data = data.Where(p => Convert.ToDateTime(p.AddDate) < time);
            }
            if (!string.IsNullOrEmpty(name))
                data = data.Where(p => p.Account.Contains(name) || p.Mobile.Contains(name) || p.Name.Contains(name));
            info.DataObj = data.OrderByDescending(p=>p.AddDate).ToList();
            info.ResultType = ResultInfo.BaseResultType.Success;
            return info;
        }
        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public ResultInfo.Info Login(Admin admin)
        {

            string passWord = Com.SHA256Encrypt(admin.PassWord);
            if (Com.isTelephone(admin.Account))
            {
                admin = db.Admin.Where(u => u.Mobile == admin.Account && u.PassWord == (passWord) && u.State != 0).FirstOrDefault();
            }
            else
            {
                admin = db.Admin.Where(u => u.Account == admin.Account && u.PassWord == (passWord) && u.State != 0).FirstOrDefault();
            }
            if (admin==null)
            {
                info.ResultType = ResultInfo.BaseResultType.Error;
                info.Msg = "账户或者密码错误";
            }
            else
            {
                info.Msg = "登录成功";
                info.ResultType = ResultInfo.BaseResultType.Success;
                info.DataObj = admin;
            }
            return info;
        }
        /// <summary>
        /// 添加管理员账号
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public ResultInfo.Info AddAdmin(Admin admin)
        {
            if (admin != null)
            {
                info.ResultType = ResultInfo.BaseResultType.IsNull;
                if (string.IsNullOrEmpty(admin.Account))
                    info.Msg = "账号不能为空";
                else if (string.IsNullOrEmpty(admin.PassWord) || admin.PassWord != admin.PassWord2)
                    info.Msg = "密码不能为空且两次密码必需相同";
                else if (db.Admin.Where(p => p.Account == admin.Account).Count() > 0)
                    info.Msg = "账号已存在";
                else
                {
                    //基础数据处理
                    admin.PassWord = Com.SHA256Encrypt(admin.PassWord);
                    admin.PassWord2 = admin.PassWord;
                    admin.Sex = admin.Sex == "on" ? "1" : "0";
                    admin.State = State.Enable;
                    admin.AddDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    db.Admin.Add(admin);
                    db.SaveChanges();
                    info.Msg = "添加成功";
                    info.ResultType = ResultInfo.BaseResultType.Success;
                }
            }
            else
            {
                info.Msg = "添加失败";
                info.ResultType = ResultInfo.BaseResultType.Error;
            }
            return info;
        }
        /// <summary>
        /// 修改管理员账号
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public ResultInfo.Info UpdateAdmin(Admin admin)
        {
            if (admin != null)
            {

                using (var tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        var obj = db.Admin.Where(p => p.Account == admin.Account).FirstOrDefault();
                        if (obj != null) {
                            //基础数据处理
                            obj.Account = admin.Account;
                            obj.Name = admin.Name;
                            obj.Remark = admin.Remark;
                            obj.Mobile = admin.Mobile;
                            obj.PassWord = Com.SHA256Encrypt(admin.PassWord);
                            obj.PassWord2 = admin.PassWord;
                            obj.Sex = admin.Sex == "on" ? "1" : "0";
                            obj.State = State.Enable;
                            obj.AddDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            db.SaveChanges();
                        }
                        //提交事务
                        tran.Commit();
                        info.ResultType = ResultInfo.BaseResultType.Success;
                        info.Msg = "修改成功";
                    }
                    catch (Exception ex)
                    {
                        info.ResultType = ResultInfo.BaseResultType.Error;
                        info.Msg = "事务回滚,"+ ex;
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
        /// 启用/停用账号
        /// </summary>
        /// <returns></returns>
        public ResultInfo.Info UpdateAdminState(int id, int state)
        {
            if (id != 0)
            {
                info.ResultType = ResultInfo.BaseResultType.Error;
                var data = db.Admin.Where(p => p.Id == id).FirstOrDefault();
                if (data != null)
                {
                    State s = (State)state;
                    data.State = s;
                    db.SaveChanges();
                    info.ResultType = ResultInfo.BaseResultType.Success;
                    info.Msg = "修改成功";
                }
                else
                {
                    info.Msg = "未查到您所操作的账号！";
                }
            }
            return info;
        }
        public ResultInfo.Info UpdateAdminStateByIds(int[] ids)
        {
            if (ids.Count() > 0)
            {
                var data = db.Admin.Where(p => ids.Contains(p.Id));
                if (data != null)
                {
                    foreach (var model in data)
                    {
                        model.State = State.Delete;
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
        public ResultInfo.Info QueryAdminById(int id)
        {
            var data = db.Admin.Where(p => p.Id == id).FirstOrDefault();
            if (data != null)
            {
                data.PassWord = "";
                data.PassWord2 = "";
                info.DataObj = data;
                info.ResultType = ResultInfo.BaseResultType.Success;
            }
            else
            {
                info.Msg = "没有查到任何数据！";
                info.ResultType = ResultInfo.BaseResultType.IsNull;
            }
            return info;
        }

    }
}
