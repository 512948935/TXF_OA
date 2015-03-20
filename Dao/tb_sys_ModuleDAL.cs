using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model;
using IDao;

namespace Dao
{
    public class tb_sys_ModuleDAL : BaseDAL<tb_sys_Module>, Itb_sys_ModuleDAL
    {
        /// <summary>
        /// 更新树形节点状态
        /// </summary>
        public void UpdateNodeState(string state, int id)
        {
            try
            {
                string sql = string.Format("UPDATE dbo.tb_sys_Module SET NodeState = '{0}' WHERE ID={1}", state, id);
                DataProvider.DBHelper.ExecuteNonQuery(CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void DeleteModule(string code)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT COUNT(1) FROM dbo.tb_sys_Module WHERE IsPage=1 AND LEFT(ModuleCode," + code.Length + ")='" + code + "'");
                int cnt = Convert.ToInt32(DataProvider.DBHelper.ExecuteScalar(CommandType.Text, sql.ToString()));
                if (cnt > 0)
                    throw new Exception("当前模块或子模块下已存在页面.");
                sql.Clear();
                sql.Append("DELETE FROM dbo.tb_sys_Module WHERE LEFT(ModuleCode," + code.Length + ")='" + code + "'");
                DataProvider.DBHelper.ExecuteNonQuery(CommandType.Text, sql.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获得分页数据
        /// </summary>
        public DataTable GetPageList(int page, int pagesize, out int total, string code, List<WhereField> listWhere = null)
        {
            string where = "IsPage=1";
            if (!string.IsNullOrEmpty(code))
                where += " AND LEFT(ModuleCode," + code.Length + ")='" + code + "'";
            if (listWhere != null)
            {
                foreach (WhereField item in listWhere)
                {
                    where += " and [" + item.Key + "] " + item.Symbol + " '%" + item.Value + "%'";
                }
            }
            string sql = string.Format("SELECT COUNT(1)CNT FROM dbo.tb_sys_Module WHERE {0}", where);
            total = Convert.ToInt32(DataProvider.DBHelper.ExecuteScalar(CommandType.Text, sql.ToString()));
            int rowNo = (page - 1) * pagesize;
            sql = string.Format(@"SELECT TOP({0})* FROM(SELECT ROW_NUMBER()OVER(ORDER BY ModuleCode)RowNo,ID,ModuleCode,ModuleName
                                 ,PageUrl,Icon,IsDisabled FROM dbo.tb_sys_Module WHERE {1}) as tt WHERE RowNo>{2}", pagesize, where, rowNo);
            return DataProvider.DBHelper.ExecuteDataTable(CommandType.Text, sql.ToString());
        }
        public void SaveModule(tb_sys_Module module, string code)
        {
            try
            {
                IsValidNode(module);
                if (module.ID == 0)
                    Add(module);
                else
                {
                    DataProvider.DBHelper.BeginTransaction();
                    Update(module);
                    if (!module.IsPage)
                        UpdateChildNode(module.ModuleCode, code);
                    DataProvider.DBHelper.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                DataProvider.DBHelper.RollbackTransaction();
                throw ex;
            }
        }
        private void UpdateChildNode(string code, string PreCode)
        {
            int len = PreCode.Length;
            //更新子节点
            string sql = string.Format(@"UPDATE	dbo.tb_sys_Module
                                         SET ModuleCode=(REPLACE(LEFT(ModuleCode,'{0}'),LEFT(ModuleCode,{1}),'{2}')+SUBSTRING(ModuleCode,{3},LEN(ModuleCode)))
                                         FROM dbo.tb_sys_Module
                                         WHERE LEFT(ModuleCode,{4})='{5}' AND ModuleCode<>'{2}'", len, len, code, len + 1, len, PreCode);
            DataProvider.DBHelper.ExecuteNonQuery(CommandType.Text, sql.ToString());
        }
        /// <summary>
        /// 检测当前节点
        /// </summary>
        /// <param name="module"></param>
        private void IsValidNode(tb_sys_Module module)
        {
            module.ModuleCode = module.ModuleCode.Trim('.');
            if (module.ModuleCode.Equals("")) throw new Exception("输入节点无效.");
            string[] itemNos = module.ModuleCode.Split('.');
            //获取最大的级次
            int maxLevelId = GetMaxLevel();
            if (itemNos.Length - maxLevelId > 1) throw new Exception("输入的级次超出范围.");
            if (maxLevelId == 0)//未添加任何节点，直接返回ParentID
            {
                module.ParentID = 0;
                module.NodeLevel = 1;
                return;
            }
            string tempNo = "";
            string where = "1=1";
            for (int i = 0; i < itemNos.Length; i++)
            {
                module.NodeLevel = i + 1;
                tempNo += itemNos[i] + ".";
                //查找记录
                where = string.Format("ModuleCode='{0}' AND NodeLevel={1}", tempNo.TrimEnd('.'), module.NodeLevel);
                if (module.ID > 0)
                    where += " AND ID<>" + module.ID;
                tb_sys_Module record = SelectT(where);
                if (record == null)
                {
                    if (itemNos.Length == 1)
                        module.ParentID = 0;
                    else
                    {
                        if (i < itemNos.Length - 1) { throw new Exception("输入的级次超出范围."); }//当前节次没有重复，还有下级节次的情况
                        //查找上级节点
                        string preCode = tempNo.TrimEnd('.').Substring(0, tempNo.TrimEnd('.').LastIndexOf('.'));
                        where = string.Format("ModuleCode='{0}' AND NodeLevel={1}", preCode, module.NodeLevel - 1);
                        tb_sys_Module parentItem = SelectT(where);
                        if (parentItem.IsPage)
                            throw new Exception("不能在页面节点下面添加模块.");
                        module.ParentID = parentItem.ID;
                    }
                    return;
                }
            }
            throw new Exception("当前代码重复.");
        }
        private int GetMaxLevel()
        {
            try
            {
                string sql = "SELECT ISNULL(MAX(NodeLevel),0) FROM dbo.tb_sys_Module";
                return (int)DataProvider.DBHelper.ExecuteScalar(CommandType.Text, sql.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
