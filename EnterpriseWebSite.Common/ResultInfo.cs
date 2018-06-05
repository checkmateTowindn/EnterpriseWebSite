using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnterpriseWebSite.Common
{
    public class ResultInfo
    {
        public class Info
        {
            /// <summary>
            /// 返回信息
            /// </summary>
            public string Msg { get; set; }
            /// <summary>
            /// 返回数据
            /// </summary>
            public object DataObj { get; set; }
            /// <summary>
            /// 返回状态类型
            /// </summary>
            public BaseResultType ResultType { get; set; }
        }
        public enum BaseResultType
        {
            /// <summary>
            /// 成功
            /// </summary>
            Success = 0,
            /// <summary>
            /// 错误
            /// </summary>
            Error = 1,
            /// <summary>
            /// 
            /// </summary>
            No = 2,
            /// <summary>
            /// 不存在
            /// </summary>
            NotExists = 3,
            /// <summary>
            /// 已存在
            /// </summary>
            Exists = 4,
            /// <summary>
            /// 超出限制
            /// </summary>
            OverLimit = 5,
            /// <summary>
            /// 为空
            /// </summary>
            IsNull = 6,
            /// <summary>
            /// 没有数据
            /// </summary>
            NoData = 7
        }
    }
}
