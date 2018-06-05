using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseWebSite.DAL
{
    [Serializable]
    public class HtmlRegion
    {
        /// <summary>
        /// 区域id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string RegionName { get; set; }
        /// <summary>
        /// 页面id
        /// </summary>
        public HtmlPage HtmlPage { get; set; }
    }
}
