using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IBLL;
using IDao;
using Model;

namespace BLL
{
    /// <summary>
    /// 实现业务逻辑基类（基类只实现增删改查逻辑）
    /// <typeparam name="T"></typeparam>
    public class BaseBLL<T> : IBaseBLL<T> where T : class, new()
    {
        private IBaseDAL<T> dao = null;
        public IBaseDAL<T> Dao
        {
            get { return dao; }
            set { this.dao = (IBaseDAL<T>)value; }
        }
        /// <summary>
        /// 绑定Mode实体类
        /// </summary>
        public virtual T BindModel()
        {
            return dao.BindModel();
        }
        /// <summary>
        /// 新增一条记录
        /// </summary>
        public virtual int Add(T entity)
        {
            return dao.Add(entity);
        }
        /// <summary>
        /// 根据主键修改记录
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(T entity)
        {
            dao.Update(entity);
        }
        /// <summary>
        /// 根据where条件修改指定字段
        /// </summary>
        /// <param name="listField"></param>
        /// <param name="listWhere"></param>
        public virtual void Update(List<UpdateField> listField, List<WhereField> listWhere)
        {
            dao.Update(listField, listWhere);
        }
        /// <summary>
        /// 根据where条件修改指定字段
        /// </summary>
        /// <param name="listField">需要跟新的字段集合</param>
        /// <param name="where">where条件字符串</param>
        public virtual void Update(List<UpdateField> listField, string where)
        {
            dao.Update(listField, where);
        }
        /// <summary>
        /// 根据实体类主键删除记录
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(T entity)
        {
            dao.Delete(entity);
        }
        /// <summary>
        /// 根据where条件删除记录
        /// </summary>
        /// <param name="where"></param>
        public virtual void Delete(string where)
        {
            dao.Delete(where);
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
            return dao.SelectT(where);
        }
        public virtual T SelectT(List<WhereField> listWhere)
        {
            return dao.SelectT(listWhere);
        }
        /// <summary>
        /// 查找实体类集合
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public virtual List<T> SelectList(string where = "", string sort = "")
        {
            return dao.SelectList(where, sort);
        }
        /// <summary>
        /// 查找实体类集合
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="sort"></param>
        /// <param name="field"></param>
        /// <param name="listWhere"></param>
        /// <returns></returns>
        public virtual List<T> SelectList(List<WhereField> listWhere = null, string sort = "")
        {
            return dao.SelectList(listWhere, sort);
        }
        /// <summary>
        /// 查找数据
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public virtual DataTable SelectDataTable(string field = "*", string where = "", string sort = "")
        {
            return dao.SelectDataTable(field, where, sort);
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
        public virtual DataTable SelectDataTable(string field = "*", List<WhereField> listWhere = null, string sort = "")
        {
            return dao.SelectDataTable(field, listWhere, sort);
        }
        /// <summary>
        /// 获取数据总条数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual int SelectCount(string where = "")
        {
            return dao.SelectCount(where);
        }
        /// <summary>
        /// 获取数据总条数
        /// </summary>
        /// <param name="listWhere"></param>
        /// <returns></returns>
        public virtual int SelectCount(List<WhereField> listWhere)
        {
            return dao.SelectCount(listWhere);
        }
    }
}
