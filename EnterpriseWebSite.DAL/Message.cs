using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseWebSite.DAL
{
    [Serializable]
    public class Message
    {
        /// <summary>
        /// 留言
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// 所属页面
        /// </summary>
        public HtmlPage HtmlPage { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [StringLength(32)]
        public string Mobile { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nick { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        [StringLength(6)]
        public string IdentifyingCode { get; set; }
        /// <summary>
        /// 留言内容
        /// </summary>
        [StringLength(1000)]
        public string MessageContent { get; set; }
        /// <summary>
        /// 上一级，如果为0那就是没有上一级
        /// </summary>
        public int UpperLeve { get; set; }
        /// <summary>
        /// 是否管理员
        /// </summary>
        public bool IsAdmin { get; set; }
        /// <summary>
        /// 回复的AdminId
        /// </summary>
        public Admin Admin { get; set; }
        /// <summary>
        /// 留言时间
        /// </summary>
        [StringLength(32)]
        public string AddDate { get; set; } = DateTime.Now.ToString();
    }
}
