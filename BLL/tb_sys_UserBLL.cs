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
    public class tb_item_UserBLL : BaseBLL<tb_item_User>, Itb_item_UserBLL
    {
        #region 接口注入反转
        /// <summary>
        /// 该接口负责用户自定义的功能实现
        /// </summary>
        private Itb_item_UserDAL myDao;
        /// <summary>
        /// 构造函数(接口转换,Dao只负责基类的增删改查)
        /// </summary>
        public tb_item_UserBLL(Itb_item_UserDAL dao)
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
                cacheKey = string.Format("page:{0},pagesiez:{1},code:'{2}',disabled:'{3}',name:'{4}'", page, pagesize, code, disabled, "tb_item_User");
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

        #region 在线用户管理
        //静态缓存key
        private static string key = "OnlineUsers";
        /// <summary>
        /// 得到所有在线用户表
        /// </summary>
        /// <returns></returns>
        public List<OnlineUsers> GetAll()
        {
            object obj = MyCache.IO.Opation.Get(key);
            return obj as List<OnlineUsers> ?? new List<OnlineUsers>();
        }
        private void set(List<OnlineUsers> list)
        {
            if (list == null) return;
            MyCache.IO.Opation.Set(key, list);
        }
        /// <summary>
        /// 添加一个用户到在线用户表
        /// </summary>
        public void AddToCache(tb_item_User user, Guid uniqueID)
        {
            try
            {
                if (user == null) return;
                var onList = GetAll();
                bool isadd = false;
                var onUser = onList.Find(p => p.User.ID == user.ID);
                if (onUser == null)
                {
                    onUser = new OnlineUsers();
                    isadd = true;
                }
                onUser.User = user;
                onUser.ID = user.ID;
                onUser.ClientInfo = string.Concat("操作系统：", Utility.Tools.GetOSName(), "浏览器：", Utility.Tools.GetBrowse());
                onUser.IP = Utility.Tools.GetIPAddress();
                onUser.LastPage = "";
                onUser.LoginTime = DateTime.Now;
                onUser.UniqueID = uniqueID;
                if (isadd)
                    onList.Add(onUser);
                set(onList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 删除一个在线用户
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool Remove(Guid id)
        {
            var list = GetAll();
            var user = list.Find(p => p.UniqueID == id);
            if (user != null)
            {
                list.Remove(user);
            }
            set(list);
            return true;
        }

        /// <summary>
        /// 清除所有在线用户
        /// </summary>
        /// <returns></returns>
        public bool RemoveAll()
        {
            var list = new List<OnlineUsers>();
            MyCache.IO.Opation.Set(key, list);
            return true;
        }

        /// <summary>
        /// 查询一个在线用户实体
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public OnlineUsers Get(Guid id)
        {
            var list = GetAll();
            return list.Find(p => p.UniqueID == id);
        }
        #endregion
    }
}
