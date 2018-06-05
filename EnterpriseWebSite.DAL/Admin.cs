using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseWebSite.DAL
{
    [Serializable]
    public class Admin
    {
        /// <summary>
        /// 账号ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }
        /// <summary>
        /// 密码2
        /// </summary>
        public string PassWord2 { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string VerifyCode { get; set; }
        /// <summary>
        /// 权限
        /// </summary>
        public Authority Authority { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        public int CreateAdminId { get; set; }

        public State State { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public string AddDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(128)]
        public string Remark { get; set; }
    }
    public enum Authority
    {
        /// <summary>
        /// 超级管理员
        /// </summary>
        Admin=0,
        /// <summary>
        /// 领导
        /// </summary>
        Leader=1,
        /// <summary>
        /// 管理员
        /// </summary>
        Manager=2
    }
public enum State {
        Disable=0,
        Enable=1,
        Delete=2,
    }
}
