using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Web;

namespace BC.InternalSystem.Utils
{ 
    /// <summary>
    /// 通用类，一些常用的实例
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// 锁定生成微秒
        /// </summary>
        private static object mLockMicrosecond = new object();
        private static string mMicrosecond = null;

        /// <summary>
        /// 静态初始化
        /// </summary>
        static Utility()
        {
        }

        /// <summary>
        /// JSON默认配置(序列化/反序列化)
        /// </summary>
        public static JsonSerializerSettings DefaultJsonSettings => new JsonSerializerSettings
        {
            //空值的属性不序列化
            NullValueHandling = NullValueHandling.Ignore,

            //Json中存在的属性，实体中不存在的属性不反序列化
            MissingMemberHandling = MissingMemberHandling.Ignore,

            //序列化日期格式转换
            DateFormatString = "yyyy-MM-dd HH:mm:ss",

            //发现循环引用时不序列化
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        /// <summary>
        /// 获取时间(微秒)
        /// </summary>
        public static string Microsecond
        {
            get
            {
                lock (mLockMicrosecond)
                {
                    string s;

                    do
                    {
                        s = DateTime.Now.ToString("yyyyMMddHHmmssffffff");
                    } while (mMicrosecond == s);

                    return mMicrosecond = s;
                }
            }
        }
        public static string GetEnumDescription(object enumSubitem)
        {
            enumSubitem = (Enum)enumSubitem;
            string strValue = enumSubitem.ToString();

            FieldInfo fieldinfo = enumSubitem.GetType().GetField(strValue);

            if (fieldinfo != null)
            {

                Object[] objs = fieldinfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (objs == null || objs.Length == 0)
                {
                    return strValue;
                }
                else
                {
                    DescriptionAttribute da = (DescriptionAttribute)objs[0];
                    return da.Description;
                }
            }
            else
            {
                return "不限";
            }

        }
    }
}