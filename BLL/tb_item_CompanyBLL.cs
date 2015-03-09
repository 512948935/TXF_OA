using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using IBLL;
using IDao;
using System.Data;

namespace BLL
{
    public class tb_item_CompanyBLL : BaseBLL<tb_item_Company>, Itb_item_CompanyBLL
    {
        #region 接口注入反转
        /// <summary>
        /// 该接口负责用户自定义的功能实现
        /// </summary>
        private Itb_item_CompanyDAL myDao;
        /// <summary>
        /// 构造函数(接口转换,Dao只负责基类的增删改查)
        /// </summary>
        public tb_item_CompanyBLL(Itb_item_CompanyDAL dao)
        {
            Dao = myDao = dao;
        }
        #endregion

        //静态缓存key
        private static string cacheKey;
        private static Dictionary<string, int> CacheKeys = new Dictionary<string, int>();
        public DataTable GetPageList(bool fromCache, int page, int pagesize, out int total, string code, string disabled, List<WhereField> listWhere)
        {
            DataTable dt = null;
            if (!fromCache)
            {
                dt = myDao.GetPageList(page, pagesize, out total, code, disabled, listWhere);
                foreach (string cacheKey in CacheKeys.Keys)
                    MyCache.IO.Opation.Remove(cacheKey);
                CacheKeys.Clear();
            }
            else
            {
                cacheKey = string.Format("page:{0},pagesiez:{1},code:'{2}',disabled:'{3}',name:'{4}'", page, pagesize, code, disabled, "tb_item_Company");
                object obj = MyCache.IO.Opation.Get(cacheKey);
                if (obj == null)
                {
                    dt = myDao.GetPageList(page, pagesize, out total, code, disabled, listWhere);
                    MyCache.IO.Opation.Set(cacheKey, dt, DateTime.Now.AddSeconds(30));
                    if (!CacheKeys.Keys.Contains(cacheKey))
                        CacheKeys.Add(cacheKey, total);
                }
                else
                {
                    dt = obj as DataTable;
                    total = CacheKeys[cacheKey];
                }
            }
            return dt;
        }
        public DataRow GetModel(int id)
        {
            DataTable dt = MyCache.IO.Opation.Get(cacheKey ?? "") as DataTable ?? myDao.GetModel(id);
            return dt.Select("ID=" + id)[0];
        }
        public int CheckItemNo(int id, string itemNo)
        {
            return myDao.CheckItemNo(id, itemNo);
        }
        public void DeleteDepinfo(string id)
        {
            myDao.DeleteDepinfo(id);
        }
    }
}
