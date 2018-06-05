using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseWebSite.DAL
{
    [Serializable]
    public class HtmlPage
    {
        /// <summary>
        /// 页面id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 页面名称
        /// </summary>
        public string PageName { get; set; }
    }
}
