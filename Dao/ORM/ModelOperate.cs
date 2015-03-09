using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using TXF.Attributes;
using Model;
using IDao;
using System.Data.Common;

namespace Dao.ORM
{
    /// <summary>
    ///  /// <summary>
    /// 实现SqlDao公共接口基类
    /// </summary>
    public class ModelOperate<T> where T:class,new()
    {
        private ModelBase _ModelBase = new ModelBase();
        private static ModelOperate<T> _modelHelper;
        /// <summary>
        /// 获取当前类的实例
        /// </summary>
        /// <returns></returns>
        public static ModelOperate<T> Instance()
        {
            if (_modelHelper == null)
            {
                _modelHelper = new ModelOperate<T>();
            }
            return _modelHelper;
        }
        #region 绑定实体类
        /// <summary>
        /// 绑定Mode实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T BindModel()
        {
            T entity = new T();
            string[] fields = _ModelBase.GetInsertFields<T>();
            Dictionary<string, object> pNameAndValue = new Dictionary<string, object>();
            foreach (string field in fields)
            {
                //获取属性的值
                string value = HttpContext.Current.Request[field];
                pNameAndValue.Add(field, value);
            }
            ReflectionHelper.SetPropertyValue<T>(entity, pNameAndValue);
            return entity;
        }
        #endregion

        #region  新增记录
        /// <summary>
        /// 新增一条记录
        /// </summary>
        public int Add(T entity)
        {
            List<SqlParameter> Parameters = new List<SqlParameter>();
            string tableName = typeof(T).Name;
            //获取需要插入的字段和值
            Dictionary<string, object> models = _ModelBase.GetModelValue<T>(entity, ActionState.Add);
            string columnName = string.Empty;
            string paramName = string.Empty;
            foreach (string field in models.Keys)
            {
                columnName += "[" + field + "],";
                paramName += "@" + field + ",";
                Parameters.Add(new SqlParameter("@" + field, models[field]));
            }
            string sql = string.Format("SET NOCOUNT ON;INSERT INTO [{0}]({1}) VALUES({2});SELECT SCOPE_IDENTITY()", tableName, columnName.TrimEnd(','), paramName.TrimEnd(','));
            object result = DataProvider.DBHelper.ExecuteScalar(CommandType.Text, sql, Parameters);
            return Convert.ToInt32(result);
        }
        #endregion

        #region 修改单条记录
        /// <summary>
        /// 根据主键修改记录
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            string tableName = typeof(T).Name;
            //获取需要更新字段的值
            Dictionary<string, object> models = _ModelBase.GetModelValue<T>(entity, ActionState.Update);
            //获取主键
            string keyField = _ModelBase.GetPrimaryKeyName<T>();
            string where = "[" + keyField + "]=@" + keyField + "";
            List<SqlParameter> Parameters = new List<SqlParameter>();
            string sets = string.Empty;
            foreach (string field in models.Keys)
            {
                if (field == keyField)
                    Parameters.Add(new SqlParameter("@" + keyField, models[keyField]));
                else
                {
                    sets += "[" + field + "]=@" + field + ",";
                    Parameters.Add(new SqlParameter("@" + field, models[field]));
                }
            }
            try
            {
                string sql = string.Format("UPDATE [{0}] SET {1} WHERE {2}", tableName, sets.TrimEnd(','), where);
                DataProvider.DBHelper.ExecuteNonQuery(CommandType.Text, sql, Parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 根据where条件修改指定字段
        /// </summary>
        /// <param name="listField">需要更新的字段集合</param>
        /// <param name="listWhere">where条件集合</param>
        public void Update(List<UpdateField> listField, List<WhereField> listWhere)
        {
            string tableName = typeof(T).Name;
            if (listField.Count() == 0 || listWhere.Count == 0) return;
            string sets = string.Empty;
            List<SqlParameter> Parameters = new List<SqlParameter>();
            string where = " 1=1 ";
            foreach (UpdateField field in listField)
            {
                sets += "[" + field.Key + "]=@" + field.Key + ",";
                Parameters.Add(new SqlParameter("@" + field.Key, field.Value));
            }
            foreach (WhereField item in listWhere)
            {
                where += " and [" + item.Key + "]=@" + item.Key + " ";
                Parameters.Add(new SqlParameter("@" + item.Key, item.Value));
            }
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat("UPDATE [{0}] SET {1} WHERE {2}", tableName, sets.TrimEnd(','), where);
                DataProvider.DBHelper.ExecuteNonQuery(CommandType.Text,sql.ToString(), Parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 根据where条件修改指定字段
        /// </summary>
        /// <param name="listField">需要跟新的字段集合</param>
        /// <param name="where">where条件字符串</param>
        public void Update(List<UpdateField> listField, string where)
        {
            if (listField.Count() == 0) return;
            string tableName = typeof(T).Name;
            string sets = string.Empty;
            List<SqlParameter> Parameters = new List<SqlParameter>();
            foreach (UpdateField field in listField)
            {
                sets += "[" + field.Key + "]=@" + field.Key + ",";
                Parameters.Add(new SqlParameter("@" + field.Key, field.Value));
            }
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat("UPDATE [{0}] SET {1} WHERE {2}", tableName, sets.TrimEnd(','), where);
                DataProvider.DBHelper.ExecuteNonQuery(CommandType.Text,sql.ToString(), Parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 删除单条记录
        /// <summary>
        /// 根据实体类主键删除记录
        /// </summary>
        public void Delete(T entity)
        {
            string tableName = typeof(T).Name;
            //获取主键
            string keyField = _ModelBase.GetPrimaryKeyName<T>();
            string where = " WHERE [" + keyField + "]=@" + keyField + "";
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Dictionary<string, object> models = _ModelBase.GetModelValue<T>(entity, ActionState.Add);
            Parameters.Add(new SqlParameter("@" + keyField, models[keyField]));
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat("DELETE FROM {0} WHERE {1}", tableName, where);
                DataProvider.DBHelper.ExecuteNonQuery(CommandType.Text, sql.ToString(), Parameters);
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 根据where条件删除记录
        /// </summary>
        /// <param name="listWhere">where条件字符串</param>
        public void Delete(string where)
        {
            if (!string.IsNullOrEmpty(where))
            {
                try
                {
                    Type type = typeof(T);
                    string tableName = type.Name;
                    StringBuilder sql = new StringBuilder();
                    sql.AppendFormat("DELETE FROM [{0}] WHERE {1}", tableName, where);
                    DataProvider.DBHelper.ExecuteNonQuery(CommandType.Text,sql.ToString(), null);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        /// <summary>
        /// 根据List<where>条件删除记录
        /// </summary>
        public void Delete(List<WhereField> listWhere)
        {
            if (listWhere.Count() == 0) return;
            List<SqlParameter> Parameters = new List<SqlParameter>();
            string where = " 1=1 ";
            foreach (WhereField item in listWhere)
            {
                where += " and [" + item.Key + "]=@" + item.Key + " ";
                Parameters.Add(new SqlParameter("@" + item.Key, item.Value));
            }
            try
            {
                Type type = typeof(T);
                string tableName = type.Name;
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat("DELETE FROM [{0}] WHERE {1}", tableName, where);
                DataProvider.DBHelper.ExecuteNonQuery(CommandType.Text,sql.ToString(), Parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 查询记录
        /// <summary>
        /// 查找单个实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where">where条件集合</param>
        public T SelectT(string where)
        {
            try
            {
                Type type = typeof(T);
                string tableName = type.Name;
                //获取所有字段
                string[] fields = _ModelBase.GetAllFields<T>();
                string selectField = _ModelBase.GetSelectFieldStr(fields);
                string sql = string.Format("SELECT {0} FROM [{1}] WHERE {2}",selectField,tableName, where);
                DbDataReader reader = DataProvider.DBHelper.ExecuteReader(CommandType.Text, sql);
                Dictionary<string, object> pNameAndValue = new Dictionary<string, object>();
                while (reader.Read())
                {
                    T entity = (T)Activator.CreateInstance(type);
                    foreach (string field in fields)
                    {
                        //取得当前数据库字段的顺序
                        int Ordinal = reader.GetOrdinal(field);
                        object obj = reader.GetValue(Ordinal);
                        if (obj != DBNull.Value)
                            pNameAndValue.Add(field, obj);
                    }
                    ReflectionHelper.SetPropertyValue<T>(entity, pNameAndValue);
                    return entity;
                }
                return default(T);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public T SelectT(List<WhereField> listWhere)
        {
            try
            {
                string where = " 1=1 ";
                List<SqlParameter> Parameters = null;
                if (listWhere != null)
                {
                    Parameters = new List<SqlParameter>();
                    foreach (WhereField item in listWhere)
                    {
                        if (!string.IsNullOrEmpty(item.Value.ToString()))
                        {
                            if (item.Symbol.Equals("like"))
                            {
                                where += " AND [" + item.Key + "] " + item.Symbol + " @" + item.Key + " ";
                                Parameters.Add(new SqlParameter("@" + item.Key, "%" + item.Value + "%"));
                            }
                            else
                            {
                                where += " AND [" + item.Key + "] " + item.Symbol + " @" + item.Key + " ";
                                Parameters.Add(new SqlParameter("@" + item.Key, item.Value));
                            }
                        }
                    }
                }
                Type type = typeof(T);
                string tableName = type.Name;
                //获取所有字段
                string[] fields = _ModelBase.GetAllFields<T>();
                string selectField = _ModelBase.GetSelectFieldStr(fields);
                string sql = string.Format("SELECT {0} FROM [{1}] WHERE {2}", selectField, tableName, where);
                DbDataReader reader = DataProvider.DBHelper.ExecuteReader(CommandType.Text, sql, Parameters);
                Dictionary<string, object> pNameAndValue = new Dictionary<string, object>();
                while (reader.Read())
                {
                    T entity = (T)Activator.CreateInstance(type);
                    foreach (string field in fields)
                    {
                        //取得当前数据库字段的顺序
                        int Ordinal = reader.GetOrdinal(field);
                        object obj = reader.GetValue(Ordinal);
                        if (obj != DBNull.Value)
                            pNameAndValue.Add(field, obj);
                    }
                    ReflectionHelper.SetPropertyValue<T>(entity, pNameAndValue);
                    return entity;
                }
                return default(T);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 查找实体类集合
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public List<T> SelectList(string where = "1=1", string sort = "")
        {
            try
            {
                Type type = typeof(T);
                string tableName = type.Name;
                //获取所有字段
                string[] fields = _ModelBase.GetAllFields<T>();
                string selectField = _ModelBase.GetSelectFieldStr(fields);
                if (sort.Equals(""))
                    sort = _ModelBase.GetPrimaryKeyName<T>();
                string sql = string.Format("SELECT {0} FROM [{1}] WHERE {2} ORDER BY [{3}]", selectField, tableName, where, sort);
                DbDataReader reader = DataProvider.DBHelper.ExecuteReader(CommandType.Text, sql);
                List<T> entities = new List<T>();
                while (reader.Read())
                {
                    Dictionary<string, object> pNameAndValue = new Dictionary<string, object>();
                    T entity = (T)Activator.CreateInstance(type);
                    foreach (string field in fields)
                    {
                        //取得当前数据库字段的顺序
                        int Ordinal = reader.GetOrdinal(field);
                        object obj = reader.GetValue(Ordinal);
                        if (obj != DBNull.Value)
                            pNameAndValue.Add(field, obj);
                    }
                    ReflectionHelper.SetPropertyValue<T>(entity, pNameAndValue);
                    entities.Add(entity);
                }
                return entities;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
          /// <summary>
        /// 查找实体类集合
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public List<T> SelectList(List<WhereField> listWhere = null, string sort = "")
        {
            string where = " 1=1 ";
            List<SqlParameter> Parameters = null;
            if (listWhere != null)
            {
                Parameters = new List<SqlParameter>();
                foreach (WhereField item in listWhere)
                {
                    if (!string.IsNullOrEmpty(item.Value.ToString()))
                    {
                        if (item.Symbol.Equals("like"))
                        {
                            where += " AND [" + item.Key + "] " + item.Symbol + " @" + item.Key + " ";
                            Parameters.Add(new SqlParameter("@" + item.Key, "%" + item.Value + "%"));
                        }
                        else
                        {
                            where += " AND [" + item.Key + "] " + item.Symbol + " @" + item.Key + " ";
                            Parameters.Add(new SqlParameter("@" + item.Key, item.Value));
                        }
                    }
                }
            }
            Type type = typeof(T);
            string tableName = type.Name;
            //获取所有字段
            string[] fields = _ModelBase.GetAllFields<T>();
            string selectField = _ModelBase.GetSelectFieldStr(fields);
            if (sort.Equals("")) sort = _ModelBase.GetPrimaryKeyName<T>();
            string sql = string.Format("SELECT {0} FROM [{1}] WHERE {2} ORDER BY [{3}]", selectField, tableName, where, sort);
            DbDataReader reader = DataProvider.DBHelper.ExecuteReader(CommandType.Text, sql);
            Dictionary<string, object> pNameAndValue = new Dictionary<string, object>();
            List<T> entities = new List<T>();
            while (reader.Read())
            {
                T entity = (T)Activator.CreateInstance(type);
                foreach (string field in fields)
                {
                    //取得当前数据库字段的顺序
                    int Ordinal = reader.GetOrdinal(field);
                    object obj = reader.GetValue(Ordinal);
                    if (obj != DBNull.Value)
                        pNameAndValue.Add(field, obj);
                }
                ReflectionHelper.SetPropertyValue<T>(entity, pNameAndValue);
                entities.Add(entity);
            }
            return entities;
        }
        /// <summary>
        /// 查找数据
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public DataTable SelectDataTable(string field = "*", string where = "1=1", string sort = "")
        {
            try
            {
                string tableName = typeof(T).Name;
                if (field.Equals("*"))
                {
                    //获取所有字段
                    string[] fields = _ModelBase.GetAllFields<T>();
                    field = _ModelBase.GetSelectFieldStr(fields);
                }
                if (sort.Equals("")) sort = _ModelBase.GetPrimaryKeyName<T>();
                string sql = string.Format("SELECT {0} FROM [{1}] WHERE {2} ORDER BY [{3}]", field, tableName, where, sort);
                DataTable dt = DataProvider.DBHelper.ExecuteDataTable(CommandType.Text, sql);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 查找数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="field">查询的字段</param>
        /// <param name="listWhere">where条件集合</param>
        /// <returns>DataTable</returns>
        public DataTable SelectDataTable(string field = "*", List<WhereField> listWhere = null, string sort = "")
        {
            try
            {
                string tableName = typeof(T).Name;
                List<SqlParameter> Parameters = null;
                string where = " 1=1 ";
                if (listWhere != null)
                {
                    Parameters = new List<SqlParameter>();
                    foreach (WhereField item in listWhere)
                    {
                        if (!string.IsNullOrEmpty(item.Value.ToString()))
                        {
                            if (item.Symbol.Equals("like"))
                            {
                                where += " AND [" + item.Key + "] " + item.Symbol + " @" + item.Key + " ";
                                Parameters.Add(new SqlParameter("@" + item.Key, "%" + item.Value + "%"));
                            }
                            else
                            {
                                where += " AND [" + item.Key + "] " + item.Symbol + " @" + item.Key + " ";
                                Parameters.Add(new SqlParameter("@" + item.Key, item.Value));
                            }
                        }
                    }
                }
                if (field.Equals("*"))
                {
                    //获取所有字段
                    string[] fields = _ModelBase.GetAllFields<T>();
                    field = _ModelBase.GetSelectFieldStr(_ModelBase.GetAllFields<T>());
                }
                if (sort.Equals("")) sort = _ModelBase.GetPrimaryKeyName<T>();
                string sql = string.Format("SELECT {0} FROM [{1}] WHERE {2} ORDER BY [{3}]", field, tableName, where, sort);
                DataTable dt = DataProvider.DBHelper.ExecuteDataTable(CommandType.Text, sql, Parameters);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取数据总条数
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int SelectCount(string where = "")
        {
            try
            {
                Type type = typeof(T);
                string tableName = type.Name;
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat("SELECT COUNT(1)CNT FROM [{0}] WHERE {1}", tableName, where);
                return (int)DataProvider.DBHelper.ExecuteScalar(CommandType.Text, sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取数据总条数
        /// </summary>
        /// <param name="Where"></param>
        /// <returns></returns>
        public int SelectCount(List<WhereField> listWhere)
        {
            Type type = typeof(T);
            string tableName = type.Name;
            string where = " 1=1 ";
            List<SqlParameter> Parameters = null;
            if (listWhere != null)
            {
                Parameters = new List<SqlParameter>();
                foreach (WhereField item in listWhere)
                {
                    if (!string.IsNullOrEmpty(item.Value.ToString()))
                    {
                        if (item.Symbol.Equals("like"))
                        {
                            where += " AND [" + item.Key + "] " + item.Symbol + " @" + item.Key + " ";
                            Parameters.Add(new SqlParameter("@" + item.Key, "%" + item.Value + "%"));
                        }
                        else
                        {
                            where += " AND [" + item.Key + "] " + item.Symbol + " @" + item.Key + " ";
                            Parameters.Add(new SqlParameter("@" + item.Key, item.Value));
                        }
                    }
                }
            }
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat("SELECT COUNT(1) FROM [{0}] WHERE {1}", tableName, where);
                return (int)DataProvider.DBHelper.ExecuteScalar(CommandType.Text, sql.ToString(), Parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
