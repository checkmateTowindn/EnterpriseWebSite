using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseWebSite.Common
{
    /// <summary>
    /// 序列化属性设置子项
    /// </summary>
    public class LimitPropsType
    {
        /// <summary>
        /// 是否保留，默认是
        /// </summary>
        public bool IsRetain { get; set; }
        /// <summary>
        /// 类型属性集合
        /// </summary>
        public List<string> PropList { get; set; }

        /// <summary>
        /// 序列化属性设置子项
        /// </summary>
        /// <param name="isRetain"></param>
        /// <param name="props"></param>
        public LimitPropsType(bool isRetain = true, params string[] props)
        {
            this.IsRetain = true;
            this.PropList = new List<string>();
            this.PropList.AddRange(props);
        }
    }
}
