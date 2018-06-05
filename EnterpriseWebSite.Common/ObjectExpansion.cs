using System;
using Newtonsoft.Json;
using BC.InternalSystem.Utils;
using System.IO;
using System.ComponentModel;
using EnterpriseWebSite.Common;

/// <summary>
/// 基类扩展(Object)
/// </summary>
public static class ObjectExpansion
{
    /// <summary>
    /// 序列化Json字符串（默认Json配置：空值的属性不序列化、Json中存在的属性，实体中不存在的属性不反序列化，避免出错）
    /// </summary>
    /// <param name="source"></param>
    /// <param name="jsonSettings">指定要序列化的Json配置</param>
    /// <returns></returns>
    public static string ToJson(this object source, JsonSerializerSettings jsonSettings = null) => source == null || source is DBNull ? null : JsonConvert.SerializeObject(source, jsonSettings ?? Utility.DefaultJsonSettings);

    /// <summary>
    /// 序列化Json字符串（默认Json配置：空值的属性不序列化、Json中存在的属性，实体中不存在的属性不反序列化，避免出错）
    /// </summary>
    /// <param name="source"></param>
    /// <param name="setAction">设置默认的序列化的Json配置函数</param>
    /// <returns></returns>
    public static string ToJson(this object source, Action<JsonSerializerSettings> setAction)
    {
        if (source == null || source is DBNull) return null;
        var jsonSettings = Utility.DefaultJsonSettings;
        setAction?.Invoke(jsonSettings);
        return JsonConvert.SerializeObject(source, jsonSettings);
    }

    /// <summary>
    /// 序列化Json字符串（默认Json配置：空值的属性不序列化、Json中存在的属性，实体中不存在的属性不反序列化，避免出错）
    /// </summary>
    /// <param name="source"></param>
    /// <param name="limitPropsContractResolver">序列化属性设置</param>
    /// <returns></returns>
    public static string ToJson(this object source, LimitPropsContractResolver limitPropsContractResolver)
    {
        if (source == null || source is DBNull) return null;
        var jsonSettings = Utility.DefaultJsonSettings;
       jsonSettings.ContractResolver = limitPropsContractResolver;
        return JsonConvert.SerializeObject(source, jsonSettings);
    }

    /// <summary>
    /// 序列化Json字符串(默认配置)
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string ToJsondef(this object source) => source == null || source is DBNull ? null : JsonConvert.SerializeObject(source);
}