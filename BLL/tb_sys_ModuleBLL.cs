using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model;
using IBLL;
using IDao;

namespace BLL
{
    public class tb_sys_ModuleBLL : BaseBLL<tb_sys_Module>, Itb_sys_ModuleBLL
    {
        #region 依赖接口注入
        /// <summary>
        /// 该接口负责用户自定义的功能实现
        /// </summary>
        private Itb_sys_ModuleDAL myDao = null;
        /// <summary>
        /// 构造函数(接口转换,Dao只负责基类的增删改查)
        /// </summary>
        public tb_sys_ModuleBLL(Itb_sys_ModuleDAL dao)
        {
            Dao = myDao = dao;
        }
        #endregion

        public void SaveModule(tb_sys_Module module, string code)
        {
            myDao.SaveModule(module, code);
        }
        public DataTable GetModuleByRoleID(string where)
        {
            return myDao.GetModuleByRoleID(where);
        }
        public void UpdateNodeState(string state, int id)
        {
            myDao.UpdateNodeState(state, id);
        }
        public void DeleteModule(string code)
        {
            myDao.DeleteModule(code);
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        public DataTable GetPageList(int page, int pagesize, out int total, string code, List<WhereField> listWhere)
        {
            return myDao.GetPageList(page, pagesize, out total, code, listWhere);
        }
    }
}
