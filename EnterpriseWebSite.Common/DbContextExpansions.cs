using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Collections.Generic;
using System.Reflection;

namespace EnterpriseWebSite.Common
{
    /// <summary>
    /// 数据库上下文扩展
    /// </summary>
    public static class DbContextExpansions
    {
        /// <summary>
        /// 附加数据库已存在的实体集合，返回只赋值主键值新对象
        /// </summary>
        /// <typeparam name="TEntity">数据库实体对象类型</typeparam>
        /// <typeparam name="TProp">数据库实体对象属性类型</typeparam>
        /// <param name="db">数据库操作</param>
        /// <param name="entitys">数据库实体对象集合</param>
        /// <param name="keyProp">数据库实体对象主键</param>
        /// <returns></returns>
        public static List<TEntity> Attach<TEntity, TProp>(this DbContext db, List<TEntity> entitys, Expression<Func<TEntity, TProp>> keyProp) where TEntity : class, new() => (List<TEntity>)db.Attach(((IEnumerable<TEntity>)entitys), keyProp);

        /// <summary>
        /// 附加数据库已存在的实体集合，返回只赋值主键值新对象
        /// </summary>
        /// <typeparam name="TEntity">数据库实体对象类型</typeparam>
        /// <typeparam name="TProp">数据库实体对象属性类型</typeparam>
        /// <param name="db">数据库操作</param>
        /// <param name="entitys">数据库实体对象集合</param>
        /// <param name="keyProp">数据库实体对象主键</param>
        /// <returns></returns>
        public static ICollection<TEntity> Attach<TEntity, TProp>(this DbContext db, ICollection<TEntity> entitys, Expression<Func<TEntity, TProp>> keyProp) where TEntity : class, new() => (ICollection<TEntity>)db.Attach(((IEnumerable<TEntity>)entitys), keyProp);

        /// <summary>
        /// 附加数据库已存在的实体集合，返回只赋值主键值新对象
        /// </summary>
        /// <typeparam name="TEntity">数据库实体对象类型</typeparam>
        /// <typeparam name="TProp">数据库实体对象属性类型</typeparam>
        /// <param name="db">数据库操作</param>
        /// <param name="entitys">数据库实体对象集合</param>
        /// <param name="keyProp">数据库实体对象主键</param>
        /// <returns></returns>
        public static IEnumerable<TEntity> Attach<TEntity, TProp>(this DbContext db, IEnumerable<TEntity> entitys, Expression<Func<TEntity, TProp>> keyProp) where TEntity : class, new()
        {
            ICollection<TEntity> newEntitys = null;
            if (db != null && entitys != null && entitys.Count() > 0)
            {
                newEntitys = (ICollection<TEntity>)Activator.CreateInstance(entitys.GetType());
                foreach (var item in entitys)
                {
                    var attach = db.Attach(item, keyProp);
                    if (attach == null) continue;
                    newEntitys.Add(attach);
                }
            }

            return newEntitys;
        }

        /// <summary>
        /// 附加数据库已存在的实体，返回只赋值主键值新对象
        /// </summary>
        /// <typeparam name="TEntity">数据库实体对象类型</typeparam>
        /// <typeparam name="TProp">数据库实体对象属性类型</typeparam>
        /// <param name="db">数据库操作</param>
        /// <param name="entity">数据库实体对象</param>
        /// <param name="keyProp">数据库实体对象主键</param>
        /// <returns></returns>
        public static TEntity Attach<TEntity, TProp>(this DbContext db, TEntity entity, Expression<Func<TEntity, TProp>> keyProp) where TEntity : class, new()
        {
            TEntity local = null;

            if (db != null && entity != null && keyProp != null)
            {
                var func = keyProp.Compile();
                var keyValue = func(entity);
                if (keyValue != null && !(keyValue is DBNull) && !string.IsNullOrEmpty(keyValue.ToString()))
                {
                    var dbSet = db.Set(entity.GetType());
                    foreach (TEntity item in dbSet.Local)
                    {
                        if (func(item).Equals(keyValue))
                        {
                            local = item;
                            break;
                        }
                    }

                    if (local == null)
                    {
                        local = new TEntity();
                        var dbEntity = db.Entry(local);
                        dbEntity.Property(keyProp).CurrentValue = keyValue;
                        dbEntity.State = EntityState.Unchanged;
                    }
                }
            }

            return local;
        }

        /// <summary>
        /// 设置引用属性并附加数据库已存在的实体集合，返回只赋值主键值新对象
        /// </summary>
        /// <typeparam name="SetEntity">设置数据库实体对象类型</typeparam>
        /// <typeparam name="AttachEntity">附加数据库实体对象类型</typeparam>
        /// <typeparam name="AttachProp">附加数据库实体对象属性和数据库实体对象类型<</typeparam>
        /// <param name="db">数据库操作</param>
        /// <param name="setEntity">要设置数据库实体对象</param>
        /// <param name="setProp">要设置数据库实体对象属性</param>
        /// <param name="attachEntitys">数据库实体对象集合</param>
        /// <param name="keyProp">数据库实体对象主键</param>
        public static void AttachSet<SetEntity, AttachEntity, AttachProp>(this DbContext db, SetEntity setEntity, Expression<Func<SetEntity, ICollection<AttachEntity>>> setProp, ICollection<AttachEntity> attachEntitys, Expression<Func<AttachEntity, AttachProp>> keyProp) where SetEntity : class, new() where AttachEntity : class, new()
        {
            if (db == null || setProp == null) return;

            //db.Entry(setEntity).Collection(set).CurrentValue = db.Attach(attachEntitys, key);
            var list = db.Entry(setEntity).Collection(setProp).CurrentValue;
            if (list == null)
            {
                db.Entry(setEntity).Collection(setProp).CurrentValue = db.Attach(attachEntitys, keyProp);
                return;
            }

            list.Clear();
            var attach = db.Attach(attachEntitys, keyProp);
            if (attach != null && attach.Count > 0)
            {
                foreach (var item in attach) list.Add(item);
            }
        }

        /// <summary>
        /// 设置引用属性并附加数据库已存在的实体，返回只赋值主键值新对象
        /// </summary>
        /// <typeparam name="SetEntity">设置数据库实体对象类型</typeparam>
        /// <typeparam name="AttachEntity">附加数据库实体对象类型</typeparam>
        /// <typeparam name="AttachProp">附加数据库实体对象属性和数据库实体对象类型<</typeparam>
        /// <param name="db">数据库操作</param>
        /// <param name="setEntity">要设置数据库实体对象</param>
        /// <param name="setProp">要设置数据库实体对象属性</param>
        /// <param name="attachEntity">数据库实体对象</param>
        /// <param name="keyProp">数据库实体对象主键</param>
        /// <returns></returns>
        public static void AttachSet<SetEntity, AttachEntity, AttachProp>(this DbContext db, SetEntity setEntity, Expression<Func<SetEntity, AttachEntity>> setProp, AttachEntity attachEntity, Expression<Func<AttachEntity, AttachProp>> keyProp) where SetEntity : class, new() where AttachEntity : class, new()
        {
            if (db == null || setProp == null) return;

            db.Entry(setEntity).Reference(setProp).CurrentValue = db.Attach(attachEntity, keyProp);
        }
    }
}