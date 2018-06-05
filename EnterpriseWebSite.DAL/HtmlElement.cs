using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseWebSite.DAL
{
    public class HtmlElement
    {
        /// <summary>
        /// 这个图片组的信息的id
        /// </summary>
        public int Id { get; set; }
        public string Title { get; set; }
        /// <summary>
        /// 所属Html区域
        /// </summary>
        public HtmlRegion HtmlRegion { get; set; }
        /// <summary>
        /// 图片的URL
        /// </summary>
        public List<Image> Image { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public Admin Admin { get; set; }
        public string Remark { get; set; }
        public string AddDate { get; set; }
    }
}
