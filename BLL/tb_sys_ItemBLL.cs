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
    public class tb_sys_ItemBLL : BaseBLL<tb_sys_Item>, Itb_sys_ItemBLL
    {
        #region 依赖接口注入
        /// <summary>
        /// 该接口负责用户自定义的功能实现
        /// </summary>
        private Itb_sys_ItemDAL myDao = null;
        /// <summary>
        /// 构造函数(接口转换,Dao只负责基类的增删改查)
        /// </summary>
        public tb_sys_ItemBLL(Itb_sys_ItemDAL dao)
        {
            Dao = myDao = dao;
        }
        #endregion
        public DataTable SelectTreeList(string url)
        {
            return myDao.SelectTreeList(url);
        }
        public string GetTabelName(string code)
        {
            return myDao.GetTabelName(code);
        }
        public int GetMaxLevel()
        {
            return myDao.GetMaxLevel();
        }
        public bool CheckNodeType(int type)
        {
            return myDao.CheckNodeType(type);
        }
        public void UpdateChildNode(string code, string preCode, string tableName)
        {
            myDao.UpdateChildNode(code, preCode, tableName);
        }
        public void UpdateNodeState(int id, string state)
        {
            myDao.UpdateNodeState(id, state);
        }
        public void DeleteByNodeCode(string code)
        {
            myDao.DeleteByNodeCode(code);
        }
    }
}
