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
    public class tb_item_DepartmentBLL : BaseBLL<tb_item_Department>, Itb_item_DepartmentBLL
    {
        #region 接口注入反转
        /// <summary>
        /// 该接口负责用户自定义的功能实现
        /// </summary>
        private Itb_item_DepartmentDAL myDao;
        /// <summary>
        /// 构造函数(接口转换,Dao只负责基类的增删改查)
        /// </summary>
        public tb_item_DepartmentBLL(Itb_item_DepartmentDAL dao)
        {
            Dao = myDao = dao;
        }
        #endregion

        public DataTable GetPageList(int page, int pagesize, out int total, string code, string isDelete, string where)
        {
            return myDao.GetPageList(page, pagesize, out total, code, isDelete, where);
        }
    }
}
