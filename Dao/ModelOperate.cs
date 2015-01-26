using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using TXF.Attributes;
using Model;
using IDao;

namespace Dao
{
    /// <summary>
    ///  /// <summary>
    /// 实现SqlDao公共接口基类
    /// </summary>
    public class ModelOperate<T> where T:class,new()
    {
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
        private ModelCheck modelCheck = new ModelCheck();
        /// <summary>
        /// 绑定Mode实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T BindModel()
        {
            T entity = new T();
            //获取当前类型的所有属性
            PropertyInfo[] Properties = typeof(T).GetProperties();
            string error = "";
            //遍历属性集合
            foreach (PropertyInfo Property in Properties)
            {
                //获取属性的值
                string value = HttpContext.Current.Request[Property.Name];
                //获取当前属性所指定的特性
                object[] attributes = Property.GetCustomAttributes(typeof(ModelAttribute), false);
                if (attributes.Length > 0)
                {
                    try
                    {
                        ModelAttribute modelAttribute = attributes[0] as ModelAttribute;
                        this.modelCheck.CheckInput(modelAttribute, value);
                    }
                    catch (Exception ex)
                    {
                        error += ex.Message;
                    }
                }
                //给实体赋值
                if (error.Equals("") && !string.IsNullOrEmpty(value))
                {
                    Property.SetValue(entity, Convert.ChangeType(value, (System.Nullable.GetUnderlyingType(Property.PropertyType) ?? Property.PropertyType)), null);
                }
            }
            if (!error.Equals("")) throw new Exception(error);
            return entity;
        }
        #endregion

        #region  新增记录
        /// <summary>
        /// 新增一条记录
        /// </summary>
        public int Add(T entity)
        {
            Type type = entity.GetType();
            string tableName = type.Name;
            PropertyInfo[] Propertys = type.GetProperties();
            List<SqlParameter> listParameter = new List<SqlParameter>();
            string fieldNames = string.Empty;
            string fieldValues = string.Empty;
            foreach (PropertyInfo Property in Propertys)
            {
                string name = Property.Name;
                object value = Property.GetValue(entity, null);
                if (value != null)
                {
                    object[] attributes = Property.GetCustomAttributes(typeof(ModelAttribute), false);
                    if (attributes.Length == 0)
                        throw new Exception("对象没有定义表结构！");
                    else
                    {
                        ModelAttribute modelAttribute = attributes[0] as ModelAttribute;
                        if (!modelAttribute.AutoIncrement)
                        {
                            fieldNames += "[" + name + "],";
                            fieldValues += "@" + name + ",";
                            listParameter.Add(new SqlParameter("@" + name, value));
                        }
                    }
                }
            }
            string sql = string.Format("SET NOCOUNT ON;INSERT INTO [{0}]({1}) VALUES({2});SELECT SCOPE_IDENTITY()", tableName, fieldNames.TrimEnd(','), fieldValues.TrimEnd(','));
            return DBHelper.Add(sql, listParameter.ToArray());
        }
        #endregion

        #region 修改单条记录
        /// <summary>
        /// 根据主键修改记录
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            Type type = entity.GetType();
            string tableName = type.Name;
            PropertyInfo[] Propertys = type.GetProperties();
            List<SqlParameter> listParameter = new List<SqlParameter>();
            string sets = string.Empty;
            string where = " 1=1 ";
            foreach (PropertyInfo Property in Propertys)
            {
                string name = Property.Name;
                object value = Property.GetValue(entity, null) ?? "";
                //获取当前属性的所有特性，如果有，查找是否有主键
                object[] attributes = Property.GetCustomAttributes(typeof(ModelAttribute), false);
                if (attributes.Length > 0)
                {
                    ModelAttribute modelAttribute = attributes[0] as ModelAttribute;
                    //如果找到主键，则根据主键进行修改，因为主键是唯一的
                    if (modelAttribute.PrimaryKey)
                        where += " and [" + name + "]=@" + name + " ";
                    else if
                        (modelAttribute.IsNotUpdate) continue;
                    else
                        sets += "[" + name + "]=@" + name + ",";
                    listParameter.Add(new SqlParameter("@" + name, value));
                }
            }
            try
            {
                string sql = string.Format("UPDATE [{0}] SET {1} WHERE {2}", tableName, sets.TrimEnd(','), where);
                DBHelper.ExecuteNonQuery(sql, listParameter.ToArray());
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
            Type type = typeof(T);
            string tableName = type.Name;
            if (listField.Count() == 0 || listWhere.Count == 0) return;
            string sets = string.Empty;
            List<SqlParameter> listParameter = new List<SqlParameter>();
            string where = " 1=1 ";
            foreach (UpdateField field in listField)
            {
                sets += "[" + field.Key + "]=@" + field.Key + ",";
                listParameter.Add(new SqlParameter("@" + field.Key, field.Value));
            }
            foreach (WhereField item in listWhere)
            {
                where += " and [" + item.Key + "]=@" + item.Key + " ";
                listParameter.Add(new SqlParameter("@" + item.Key, item.Value));
            }
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat("update [{0}] set {1} where {2}", tableName, sets.TrimEnd(','), where);
                DBHelper.ExecuteNonQuery(sql.ToString(), listParameter.ToArray());
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
            Type type = typeof(T);
            string tableName = type.Name;
            string sets = string.Empty;
            List<SqlParameter> listParameter = new List<SqlParameter>();
            foreach (UpdateField field in listField)
            {
                sets += "[" + field.Key + "]=@" + field.Key + ",";
                listParameter.Add(new SqlParameter("@" + field.Key, field.Value));
            }
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat("update [{0}] set {1} where {2}", tableName, sets.TrimEnd(','), where);
                DBHelper.ExecuteNonQuery(sql.ToString(), listParameter.ToArray());
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
            Type type = entity.GetType();
            string tableName = type.Name;
            PropertyInfo[] Propertys = type.GetProperties();
            List<SqlParameter> listParameter = new List<SqlParameter>();
            string where = " 1=1 ";
            bool existPrimaryKey = false;
            foreach (PropertyInfo Property in Propertys)
            {
                string name = Property.Name;
                object value = Property.GetValue(entity, null);
                if (value != null)
                {
                    object[] attributes = Property.GetCustomAttributes(typeof(ModelAttribute), false);
                    if (attributes.Length > 0)
                    {
                        ModelAttribute modelAttribute = attributes[0] as ModelAttribute;
                        if (modelAttribute.PrimaryKey)
                        {
                            where += " and [" + name + "]=@" + name + " ";
                            listParameter.Add(new SqlParameter("@" + name, value));
                            existPrimaryKey = true;
                            break;
                        }
                    }
                }
            }
            if (existPrimaryKey)
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendFormat("DELETE FROM {0} WHERE {1}", tableName, where);
                    DBHelper.ExecuteNonQuery(sql.ToString(), listParameter.ToArray());
                }
                catch (Exception)
                {

                    throw;
                }
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
                    DBHelper.ExecuteNonQuery(sql.ToString(), null);
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
            List<SqlParameter> listParameter = new List<SqlParameter>();
            string where = " 1=1 ";
            foreach (WhereField item in listWhere)
            {
                where += " and [" + item.Key + "]=@" + item.Key + " ";
                listParameter.Add(new SqlParameter("@" + item.Key, item.Value));
            }
            try
            {
                Type type = typeof(T);
                string tableName = type.Name;
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat("delete from [{0}] where {1}", tableName, where);
                DBHelper.ExecuteNonQuery(sql.ToString(), listParameter.ToArray());
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
        public T SelectEntity(string where)
        {
            try
            {
                Type type = typeof(T);
                string tableName = type.Name;
                string sql = string.Format("SELECT * FROM [{0}] WHERE {1}", tableName, where);
                DataTable dt = DBHelper.Query(sql);
                if (dt.Rows.Count > 0)
                {
                    PropertyInfo[] Properties = typeof(T).GetProperties();
                    T entity = (T)Activator.CreateInstance(type);
                    foreach (PropertyInfo Property in Properties)
                    {
                        object[] attributes = Property.GetCustomAttributes(typeof(ModelAttribute), false);
                        if (attributes.Length == 0)
                            throw new Exception("对象没有定义表结构！");
                        else
                        {
                            ModelAttribute modelAttribute = attributes[0] as ModelAttribute;
                            if (!modelAttribute.Readonly && dt.Rows[0][Property.Name] != DBNull.Value)
                                Property.SetValue(entity, dt.Rows[0][Property.Name], null);
                        }
                    }
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
        public List<T> SelectList(string field = "*", string where = "1=1", string sort = "")
        {
            try
            {
                Type type = typeof(T);
                string tableName = type.Name;
                List<T> entities = new List<T>();
                DataTable dt = SelectDataTable(field, where);
                var plist = new List<PropertyInfo>(typeof(T).GetProperties());
                foreach (DataRow item in dt.Rows)
                {
                    T entity = System.Activator.CreateInstance<T>();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                        if (info != null)
                        {
                            if (!Convert.IsDBNull(item[i]))
                            {
                                info.SetValue(entity, item[i], null);
                            }
                        }
                    }
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
        /// <typeparam name="T"></typeparam>
        /// <param name="where">where条件集合</param>
        /// <returns>List<T></returns>
        public List<T> SelectList(string field = "*", List<WhereField> listWhere = null, string sort = "")
        {
            try
            {
                Type type = typeof(T);
                string tableName = type.Name;
                List<T> entities = new List<T>();
                DataTable dt = SelectDataTable(field, listWhere);
                if (dt.Rows.Count > 0)
                {
                    PropertyInfo[] Properties = typeof(T).GetProperties();
                    foreach (PropertyInfo Property in Properties)
                    {
                        T entity = (T)Activator.CreateInstance(type);
                        object[] attributes = Property.GetCustomAttributes(typeof(ModelAttribute), false);
                        if (attributes.Length == 0)
                            throw new Exception("对象没有定义表结构！");
                        else
                        {
                            ModelAttribute modelAttribute = attributes[0] as ModelAttribute;
                            if (!modelAttribute.Readonly && dt.Rows[0][Property.Name] != DBNull.Value)
                                Property.SetValue(entity, dt.Rows[0][Property.Name], null);
                        }
                        entities.Add(entity);
                    }
                    return entities;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                Type type = typeof(T);
                string tableName = type.Name;
                string sql = string.Format("SELECT {0} FROM [{1}] WHERE {2}",field,tableName, where);
                if (!string.IsNullOrEmpty(sort)) sql += " ORDER BY " + sort;
                DataTable dt = DBHelper.Query(sql);
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
                Type type = typeof(T);
                string tableName = type.Name;
                List<SqlParameter> listParameter = new List<SqlParameter>();
                string where = " 1=1 ";
                if (listWhere != null)
                {
                    foreach (WhereField item in listWhere)
                    {
                        if (!string.IsNullOrEmpty(item.Value.ToString()))
                        {
                            if (item.Symbol.Equals("like"))
                            {
                                where += " AND [" + item.Key + "] " + item.Symbol + " @" + item.Key + " ";
                                listParameter.Add(new SqlParameter("@" + item.Key, "%" + item.Value + "%"));
                            }
                            else
                            {
                                where += " AND [" + item.Key + "] " + item.Symbol + " @" + item.Key + " ";
                                listParameter.Add(new SqlParameter("@" + item.Key, item.Value));
                            }
                        }
                    }
                }
                string sql = string.Format("SELECT * FROM [{0}] WHERE {1}", tableName, where);
                if (!string.IsNullOrEmpty(sort)) sql += " ORDER BY " + sort;
                DataTable dt = DBHelper.Query(sql, listParameter.ToArray());
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
                return (int)DBHelper.SingleQuery(sql.ToString());
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
            List<SqlParameter> listParameter = new List<SqlParameter>();
            Type type = typeof(T);
            string tableName = type.Name;
            string where = " 1=1 ";
            if (listWhere != null)
            {
                foreach (WhereField item in listWhere)
                {
                    if (!string.IsNullOrEmpty(item.Value.ToString()))
                    {
                        if (item.Symbol.Equals("like"))
                        {
                            where += " AND [" + item.Key + "] " + item.Symbol + " @" + item.Key + " ";
                            listParameter.Add(new SqlParameter("@" + item.Key, "%" + item.Value + "%"));
                        }
                        else
                        {
                            where += " AND [" + item.Key + "] " + item.Symbol + " @" + item.Key + " ";
                            listParameter.Add(new SqlParameter("@" + item.Key, item.Value));
                        }
                    }
                }
            }
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat("SELECT COUNT(1) FROM [{0}] WHERE {1}", tableName, where);
                return (int)DBHelper.SingleQuery(sql.ToString(), listParameter.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
