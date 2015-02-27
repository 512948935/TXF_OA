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
                string sql = string.Format(@"SELECT * FROM (SELECT a.ID
                                                            ,a.ParentID
                                                            ,a.NodeCode
                                                            ,a.NodeName
                                                            ,a.NodeType
                                                            ,b.PageUrl
                                                            ,a.NodeState
                                                     FROM   dbo.tb_sys_Item a
                                                            LEFT JOIN dbo.tb_sys_Module b ON b.ID=a.NodeType
                                                    ) AS tt WHERE {0} ORDER BY NodeCode", where);
                return DataProvider.DBHelper.ExecuteDataTable(CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SaveItemInfo(tb_sys_Item entity, string preCode)
        {
            try
            {
                if (entity.ID == 0)
                    Add(entity);
                else
                {
                    tb_sys_Item item = SelectT("ID=" + entity.ID);
                    DataProvider.DBHelper.BeginTransaction();
                    item.NodeCode = entity.NodeCode;
                    item.NodeName = entity.NodeName;
                    item.NodeLevel = entity.NodeLevel;
                    item.ParentID = entity.ParentID;
                    Update(item);
                    string tableName = GetTabelName(preCode);
                    UpdateChildNode(entity.NodeCode, preCode, tableName);
                    DataProvider.DBHelper.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                DataProvider.DBHelper.RollbackTransaction();
                throw ex;
            }
        }
        private string GetTabelName(string code)
        {
            try
            {
                string sql = string.Format("SELECT DISTINCT TableName FROM dbo.View_tb_sys_Item WHERE LEFT(NodeCode,{0})='{1}'", code.Length, code);
                object obj = DataProvider.DBHelper.ExecuteScalar(CommandType.Text, sql).ToString();
                return obj == null ? "" : obj.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void UpdateChildNode(string code, string preCode, string tableName)
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
                    sql += string.Format(@";UPDATE {0} SET ItemNo= (REPLACE(LEFT(ItemNo,'{1}'),LEFT(ItemNo,{2}),'{3}')+SUBSTRING(ItemNo,{4},LEN(ItemNo)))
                                            ,ItemName=a.NodeName FROM dbo.tb_sys_Item a
                           WHERE a.ID=ItemID AND LEFT(ItemNo,{5})='{6}'",tableName,len, len, code, len + 1, len, preCode);
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
                DataProvider.DBHelper.ExecuteNonQuery(CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //根据节点代码删除当前节点及其下面的子节点
        public void DeleteByNodeCode(string code)
        {
            try
            {
                string tableName = GetTabelName(code);
                if (!string.IsNullOrEmpty(tableName))
                    throw new Exception("当前节点或子节点已存在明细.");
                string sql = string.Format("DELETE FROM dbo.tb_sys_Item WHERE LEFT(NodeCode,{0})='{1}'", code.Length, code);
                DataProvider.DBHelper.ExecuteNonQuery(CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region 检测节点
        /// <summary>
        /// 检测当前节点
        /// </summary>
        /// <param name="item"></param>
        public void IsValidNode(tb_sys_Item item)
        {
            item.NodeCode = item.NodeCode.Trim('.');
            if (item.NodeCode.Equals("")) throw new Exception("输入节点无效.");
            string[] itemNos = item.NodeCode.Split('.');
            //获取最大的级次
            int maxLevelId = GetMaxLevel();
            if (itemNos.Length - maxLevelId > 1) throw new Exception("输入的级次超出范围.");
            if (maxLevelId == 0)//未添加任何节点，直接返回ParentID
            {
                item.ParentID = 0;
                item.NodeLevel = 1;
                return;
            }
            string tempNo = "";
            string where = "1=1";
            for (int i = 0; i < itemNos.Length; i++)
            {
                item.NodeLevel = i + 1;
                tempNo += itemNos[i] + ".";
                //查找记录
                where = string.Format("NodeCode='{0}' AND NodeLevel={1}", tempNo.TrimEnd('.'), item.NodeLevel);
                if (item.ID > 0)
                    where += " AND ID<>" + item.ID;
                string sql = string.Format("SELECT COUNT(1) FROM dbo.View_tb_sys_Item WHERE {0}", where);
                int cnt = Convert.ToInt32(DataProvider.DBHelper.ExecuteScalar(CommandType.Text, sql));
                if (cnt == 0)
                {
                    if (itemNos.Length == 1)
                    {
                        item.ParentID = 0;
                        //检测节点类型是否重复添加
                        if (!CheckNodeType(item))
                            throw new Exception("当前节点类型已添加.");
                    }
                    else
                    {
                        if (i < itemNos.Length - 1) { throw new Exception("输入的级次超出范围."); }//当前节次没有重复，还有下级节次的情况
                        //查找上级节点
                        string preCode = tempNo.TrimEnd('.').Substring(0, tempNo.TrimEnd('.').LastIndexOf('.'));
                        where = string.Format("NodeCode='{0}' AND NodeLevel={1}", preCode, item.NodeLevel - 1);
                        sql = string.Format("SELECT * FROM dbo.View_tb_sys_Item WHERE {0}", where);
                        DataTable dt = DataProvider.DBHelper.ExecuteDataTable(CommandType.Text, sql);
                        DataRow row = dt.Rows[0];
                        if (!string.IsNullOrEmpty(row["TableName"].ToString()))
                            throw new Exception("不允许在明细节点下面添加子节点.");
                        if (Convert.ToInt32(row["NodeType"]) != item.NodeType)
                            throw new Exception("类别不一致.");
                        item.ParentID = Convert.ToInt32(row["ID"]);
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
                string sql = "SELECT ISNULL(MAX(NodeLevel),0) FROM dbo.tb_sys_Item";
                return (int)DataProvider.DBHelper.ExecuteScalar(CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool CheckNodeType(tb_sys_Item item)
        {
            try
            {
                string sql = string.Format("SELECT COUNT(1)CNT FROM dbo.tb_sys_Item WHERE ParentID=0 AND NodeType={0} AND ID<>{1}", item.NodeType, item.ID);
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
        #endregion
    }
}
