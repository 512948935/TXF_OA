using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDao;
using IBLL;
using Model;
using System.Data;

namespace BLL
{
    public class tb_sys_Role_PermissionBLL : BaseBLL<tb_sys_Role_Permission>, Itb_sys_Role_PermissionBLL
    {
        #region 接口注入反转
        /// <summary>
        /// 该接口负责用户自定义的功能实现
        /// </summary>
        private Itb_sys_Role_PermissionDAL myDao;
        /// <summary>
        /// 构造函数(接口转换,Dao只负责基类的增删改查)
        /// </summary>
        public tb_sys_Role_PermissionBLL(Itb_sys_Role_PermissionDAL dao)
        {
            Dao = myDao = dao;
        }
        #endregion
        public void SavePermissions(List<tb_sys_Role_Permission> Permissions)
        {
            myDao.SavePermissions(Permissions);
        }
        public DataTable GetPermissions(bool fromCache, int roleID)
        {
            DataTable permissions = null;
            string cacheKey = "tb_sys_Role_Permission_" + roleID;
            if (!fromCache)
            {
                permissions = myDao.GetPermissions(roleID);
                MyCache.IO.Opation.Remove(cacheKey);
            }
            else
            {
                object obj = MyCache.IO.Opation.Get(cacheKey);
                if (obj == null)
                {
                    permissions = myDao.GetPermissions(roleID);
                    MyCache.IO.Opation.Set(cacheKey, permissions, DateTime.Now.AddSeconds(30));
                }
                else
                {
                    permissions = obj as DataTable;
                }
            }
            return permissions;
        }
    }
}
