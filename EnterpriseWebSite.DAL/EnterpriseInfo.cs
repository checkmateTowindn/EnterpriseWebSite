using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseWebSite.DAL
{
    public class EnterpriseInfo
    {
        public int Id { get; set; }
        /// <summary>
        /// 所处位置的名称
        /// </summary>
        public HtmlRegion HtmlRegion { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Admin Admin { get; set; }
        public string AddDate { get; set; }
    }
}
