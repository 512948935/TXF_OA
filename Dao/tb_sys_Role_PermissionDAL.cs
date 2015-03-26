using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using IDao;
using System.Data;

namespace Dao
{
    public class tb_sys_Role_PermissionDAL : BaseDAL<tb_sys_Role_Permission>, Itb_sys_Role_PermissionDAL
    {
        public void SavePermissions(List<tb_sys_Role_Permission> Permissions)
        {
            try
            {
                DataProvider.DBHelper.BeginTransaction();
                foreach (tb_sys_Role_Permission permission in Permissions)
                {
                    if (permission.ID == 0)
                        Add(permission);
                    else
                        Update(permission);
                }
                DataProvider.DBHelper.CommitTransaction();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DataProvider.DBHelper.RollbackTransaction();
            }
        }
        public DataTable GetPermissions(int roleID)
        {
            try
            {
                string sql = string.Format(@"SELECT	a.ID
	                      ,a.RoleID
	                      ,a.ModuleID
	                      ,a.ButtonID
                          ,a.IsChecked
	                      ,b.PageUrl
                     FROM  dbo.tb_sys_Role_Permission a
		                   JOIN dbo.tb_sys_Module b ON b.ID=a.ModuleID WHERE a.RoleID={0}", roleID);
                return DataProvider.DBHelper.ExecuteDataTable(CommandType.Text, sql);
            }
            catch (Exception ex)
            { 
                throw ex;
            }
        }
    }
}
