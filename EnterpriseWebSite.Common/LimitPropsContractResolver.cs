using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseWebSite.Common
{
    /// <summary>
    /// 序列化属性
    /// </summary>
    public class LimitPropsContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// 类型属性集合
        /// </summary>
        public Dictionary<Type, LimitPropsType> TypePropList { get; set; } = new Dictionary<Type, LimitPropsType>();
        /// <summary>
        /// 添加类型属性集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newProps">取新的对象属性</param>
        public LimitPropsContractResolver Add<T>(Expression<Func<T, object>> newProps, bool? isRetain = null) where T : class, new()
        {
            var express = newProps.Body as NewExpression;
            if (express == null) return this;
            var type = typeof(T);
            if (!this.TypePropList.ContainsKey(type)) this.TypePropList.Add(type, new LimitPropsType());
            if (isRetain != null) this.TypePropList[type].IsRetain = isRetain.Value;
            foreach (var member in express.Members)
            {
                if (!this.TypePropList[type].PropList.Contains(member.Name))
                {
                    this.TypePropList[type].PropList.Add(member.Name);
                }
            }

            return this;
        }

        /// <summary>
        /// 获取序列化属性
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberSerialization"></param>
        /// <returns></returns>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var list = base.CreateProperties(type, memberSerialization);

            var propType = type;
            if (!this.TypePropList.ContainsKey(propType))
            {
                if (propType.BaseType == null || !this.TypePropList.ContainsKey(propType.BaseType)) return list;
                propType = propType.BaseType;
            }

            return list.Where(p => this.TypePropList[propType].IsRetain ? this.TypePropList[propType].PropList.Contains(p.PropertyName) : !this.TypePropList[propType].PropList.Contains(p.PropertyName)).ToList();
        }
    }
}
