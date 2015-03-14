using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TXF.Attributes;
using System.Reflection;

namespace Dao.ORM
{
    /// <summary>
    /// Model实体的解析接口
    /// </summary>
    public enum ActionState
    {
        Add, Update
    }
    public class ModelBase
    {
        private  object locker1 = new object();
        private  object locker2 = new object();
        ModelCheck modelCheck = new ModelCheck();
        /// <summary>
        /// 根据Model类型获取表名
        /// </summary>
        public string GetTableName<T>()
        {
            string str = typeof(T).ToString();
            int a = str.LastIndexOf('.');
            string rStr = str.Substring(a + 1);
            return rStr;
        }
        /// <summary>
        /// 获取Model的属性对象,获取第一次后会放入一个缓存列表中(反射一次)
        /// </summary>
        public Dictionary<string, ModelAttribute> GetModelAttribute<T>() where T : new()
        {
            lock (locker1)
            {
                string key = typeof(T).Name + "_ModelAttribute";
                object obj = MyCache.IO.Opation.Get(key);
                if (obj == null)
                {
                    var attrs = GetModelParam<T>();
                    MyCache.IO.Opation.Set(key, attrs);
                    return attrs;
                }
                return (Dictionary<string, ModelAttribute>)obj;
            }
        }
        /// <summary>
        /// 通过解析获得Model的对象的参数,Key:为类的属性名
        /// </summary>
        /// <param name="model">model对象</param>
        /// <returns>返回model参数</returns>
        private Dictionary<string, ModelAttribute> GetModelParam<T>() where T : new()
        {
            var list = new Dictionary<string, ModelAttribute>();
            PropertyInfo[] pros = ReflectionHelper.GetPropertyInfo<T>();
            foreach (PropertyInfo item in pros)
            {
                var attr = ReflectionHelper.GetCustomAttribute<ModelAttribute>(item);
                if (attr != null)
                {
                    if (string.IsNullOrEmpty(attr.Name))
                        attr.Name = item.Name;
                    list.Add(item.Name, attr);
                }
            }
            return list;
        }
        /// <summary>
        /// 获取所有字段
        /// </summary>
        public string[] GetAllFields<T>() where T : new()
        {
            lock (locker2)
            {
                string key = typeof(T).Name + "_AllFields";
                List<string> fieldList = MyCache.IO.Opation.Get(key) as List<string>;
                if (fieldList == null)
                {
                    Dictionary<string, ModelAttribute> modelAttr = GetModelAttribute<T>();
                    fieldList = new List<string>();
                    foreach (string item in modelAttr.Keys)
                    {
                        fieldList.Add(item);
                    }
                    MyCache.IO.Opation.Set(key, fieldList);
                    return fieldList.ToArray();
                }
                return fieldList.ToArray();
            }
        }
        /// <summary>
        /// 获取Insert语句的字段,会去除自增长列名
        /// </summary>
        public string[] GetInsertFields<T>() where T : new()
        {
            Dictionary<string, ModelAttribute> modelAttr = GetModelAttribute<T>();
            List<string> fl = new List<string>();
            foreach (string item in modelAttr.Keys)
            {
                if (modelAttr[item].AutoIncrement || modelAttr[item].NotAdd == true) continue;
                fl.Add(item);
            }
            if (fl == null || fl.Count == 0)
            {
                string errMsg = string.Format("解析{0}类的字段失败", typeof(T));
                throw new Exception(errMsg);
            }
            return fl.ToArray();
        }
        /// <summary>
        /// 获得主键字段名
        /// </summary>
        public string GetPrimaryKeyName<T>() where T : new()
        {
            Dictionary<string, ModelAttribute> modelAttr = GetModelAttribute<T>();
            foreach (string item in modelAttr.Keys)
            {
                if (modelAttr[item].AutoIncrement)
                    return item;
            }
            throw new Exception("获取主键失败");
        }
        /// <summary>
        /// 反射获取字段名与对应的字段值
        /// </summary>
        /// <param name="model">实体对象</param>        
        public Dictionary<string, object> GetModelValue<T>(T model, ActionState state) where T : new()
        {
            Dictionary<string, ModelAttribute> modelAttr = GetModelAttribute<T>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            string error = "";
            foreach (string field in modelAttr.Keys)
            {
                PropertyInfo proInfo = ReflectionHelper.GetPropertyInfo<T>(field);
                string fieldName = field;
                object attrValue = proInfo.GetValue(model, null);
                try
                {
                    if (state == ActionState.Add && (modelAttr[field].AutoIncrement || modelAttr[field].NotAdd)) continue;
                    if (state == ActionState.Update && modelAttr[field].NotUpdate) continue;
                    //if (attrValue != null)
                    this.modelCheck.CheckInput(modelAttr[field], attrValue);
                    r.Add(fieldName, attrValue);
                }
                catch (Exception ex)
                {
                    error += ex.Message;
                    throw;
                }
            }
            if (!error.Equals("")) throw new Exception(error);
            return r;
        }
        /// <summary>
        /// 获得数据库用的字段格式,[Id],[Name],[Age]
        /// </summary>        
        public string GetSelectFieldStr(string[] fieldArr)
        {
            if (fieldArr == null || fieldArr.Length == 0) return null;
            StringBuilder fdStr = new StringBuilder();
            for (int i = 0; i < fieldArr.Length; i++)
            {
                fdStr.AppendFormat("[{0}],", fieldArr[i]);
            }
            return fdStr.ToString().TrimEnd(',');
        }
    }
}
