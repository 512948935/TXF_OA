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
        public int GetMaxLevel()
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
        public void UpdateChildNode(string code, string preCode)
        {
            int len = preCode.Length;
            StringBuilder sql = new StringBuilder();
            //更新子节点
            sql.AppendFormat("UPDATE dbo.tb_sys_Module SET ModuleCode =(REPLACE(LEFT(ModuleCode,'{0}')"
                       + ",LEFT(ModuleCode,{1}),'{2}')+SUBSTRING(ModuleCode,{3},LEN(ModuleCode))) FROM dbo.tb_sys_Module"
                       + " WHERE LEFT(ModuleCode,{4})='{5}'", len, len, code, len + 1, len, preCode);
            DataProvider.DBHelper.ExecuteNonQuery(CommandType.Text, sql.ToString());
        }
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
                                 ,PageUrl,Icon,IsDelete FROM dbo.tb_sys_Module WHERE {1}) as tt WHERE RowNo>{2}", pagesize, where, rowNo);
            return DataProvider.DBHelper.ExecuteDataTable(CommandType.Text, sql.ToString());
        }
    }
}
