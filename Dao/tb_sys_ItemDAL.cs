using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using IDao;
using System.Data;

namespace Dao
{
    public class tb_sys_ItemDAL : BaseDAL<tb_sys_Item>, Itb_sys_ItemDAL
    {
        public DataTable SelectTreeList(string url)
        {
            try
            {
                string where = "1=1";
                if (!string.IsNullOrEmpty(url))
                    where += " AND PageUrl='" + url + "'";
                string sql = string.Format(@" SELECT * FROM (SELECT a.ID
                                                            ,a.ParentID
                                                            ,a.NodeCode
                                                            ,a.NodeName
                                                            ,a.NodeType
                                                            ,b.PageUrl
                                                            ,a.NodeState
                                                            ,ISNULL(a.TableName,'') TableName
                                                     FROM   dbo.tb_sys_Item a
                                                            LEFT JOIN dbo.tb_sys_Module b ON b.ID=a.NodeType
                                                            AND TableName IS NULL
                                                    ) AS tt WHERE {0} ORDER BY NodeCode", where);
                return DataProvider.DBHelper.ExecuteDataTable(CommandType.Text,sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetTabelName(string code)
        {
            try
            {
                string sql = string.Format("SELECT ISNULL(MIN(TableName),'')TableName FROM dbo.tb_sys_Item WHERE LEFT(NodeCode,{0})='{1}'", code.Length, code);
                return DataProvider.DBHelper.ExecuteScalar(CommandType.Text, sql).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetMaxLevel()
        {
            try
            {
                string sql = "SELECT ISNULL(MAX(NodeLevel),0) FROM dbo.tb_sys_Item";
                return (int)DataProvider.DBHelper.ExecuteScalar(CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool CheckNodeType(int type)
        {
            try
            {
                string sql = "SELECT COUNT(*)CNT FROM dbo.tb_sys_Item WHERE NodeType=" + type;
                int cnt = (int)DataProvider.DBHelper.ExecuteScalar(CommandType.Text, sql);
                if (cnt > 0)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateChildNode(string code, string preCode, string tableName)
        {
            try
            {
                int len = preCode.Length;
                //更新节点
                string sql = string.Format(@"UPDATE dbo.tb_sys_Item SET NodeCode =(REPLACE(LEFT(NodeCode,'{0}')
                             ,LEFT(NodeCode,{1}),'{2}')+SUBSTRING(NodeCode,{3},LEN(NodeCode))) FROM dbo.tb_sys_Item
                              WHERE LEFT(NodeCode,{4})='{5}'", len, len, code, len + 1, len, preCode);
                //更新业务表(目前只更新名称和代码)
                if (!string.IsNullOrEmpty(tableName))
                {
                    sql += string.Format(@";UPDATE {0} SET ItemNo= a.NodeCode,ItemName=a.NodeName FROM dbo.tb_sys_Item a
                           WHERE a.ID=ItemID AND LEFT(ItemNo,{1})='{2}'", tableName, len, preCode);
                }
                DataProvider.DBHelper.ExecuteNonQuery(CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 更新树形节点状态
        /// </summary>
        public void UpdateNodeState(int id, string state)
        {
            try
            {
                string sql = "UPDATE dbo.tb_sys_Item SET NodeState = '" + state + "' WHERE ID=" + id;
                DataProvider.DBHelper.ExecuteNonQuery(CommandType.Text,sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteByNodeCode(string code)
        {
            try
            {
                string sql = string.Format("DELETE FROM dbo.tb_sys_Item WHERE LEFT(NodeCode,{0})='{1}'", code.Length, code);
                DataProvider.DBHelper.ExecuteNonQuery(CommandType.Text, sql);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
