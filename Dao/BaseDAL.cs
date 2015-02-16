using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model;
using IDao;

namespace Dao
{
    public class BaseDAL<T> : IBaseDAL<T> where T : class,new()
    {
        /// <summary>
        /// 绑定Mode实体类
        /// </summary>
        public virtual T BindModel()
        {
            return ORM.ModelOperate<T>.Instance().BindModel();
        }
        /// <summary>
        /// 新增一条记录
        /// </summary>
        public virtual int Add(T entity)
        {
            return ORM.ModelOperate<T>.Instance().Add(entity);
        }
        /// <summary>
        /// 根据主键修改记录
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(T entity)
        {
            ORM.ModelOperate<T>.Instance().Update(entity);
        }
        /// <summary>
        /// 根据where条件修改指定字段
        /// </summary>
        /// <param name="listField"></param>
        /// <param name="listWhere"></param>
        public virtual void Update(List<UpdateField> listField, List<WhereField> listWhere)
        {
            ORM.ModelOperate<T>.Instance().Update(listField, listWhere);
        }
        /// <summary>
        /// 根据where条件修改指定字段
        /// </summary>
        /// <param name="listField">需要跟新的字段集合</param>
        /// <param name="where">where条件字符串</param>
        public virtual void Update(List<UpdateField> listField, string where)
        {
            ORM.ModelOperate<T>.Instance().Update(listField, where);
        }
        /// <summary>
        /// 根据实体类主键删除记录
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(T entity)
        {
            ORM.ModelOperate<T>.Instance().Delete(entity);
        }
        /// <summary>
        /// 根据where条件删除记录
        /// </summary>
        /// <param name="where"></param>
        public virtual void Delete(string where)
        {
            ORM.ModelOperate<T>.Instance().Delete(where);
        }
        /// <summary>
        /// 根据List<where>条件删除记录
        /// </summary>
        /// <param name="listWhere"></param>
        public virtual void Delete(List<WhereField> listWhere) { }
        /// <summary>
        /// 查找单个实体类
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual T SelectT(string where)
        {
            return ORM.ModelOperate<T>.Instance().SelectT(where);
        }
        /// <summary>
        /// 查找实体类集合
        /// </summary>
        /// <param name="where"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public virtual List<T> SelectList(string where = "", string sort = "")
        {
            return ORM.ModelOperate<T>.Instance().SelectList(where, sort);
        }
        /// <summary>
        /// 查找实体类集合
        /// </summary>
        /// <param name="where"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public virtual List<T> SelectList(List<WhereField> where = null, string sort = "")
        {
            return ORM.ModelOperate<T>.Instance().SelectList(where, sort);
        }
        /// <summary>
        /// 查找数据
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public virtual DataTable SelectDataTable(string field = "", string where = "", string sort = "")
        {
            return ORM.ModelOperate<T>.Instance().SelectDataTable(field, where, sort);
        }
        /// <summary>
        /// 查找数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="sort"></param>
        /// <param name="field"></param>
        /// <param name="listWhere"></param>
        /// <returns></returns>
        public virtual DataTable SelectDataTable(string field = "", List<WhereField> listWhere = null, string sort = "")
        {
            return ORM.ModelOperate<T>.Instance().SelectDataTable(field, listWhere, sort);
        }
        /// <summary>
        /// 获取数据总条数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual int SelectCount(string where = "")
        {
            return ORM.ModelOperate<T>.Instance().SelectCount(where);
        }
        /// <summary>
        /// 获取数据总条数
        /// </summary>
        /// <param name="listWhere"></param>
        /// <returns></returns>
        public virtual int SelectCount(List<WhereField> listWhere)
        {
            return ORM.ModelOperate<T>.Instance().SelectCount(listWhere);
        }
    }
}
