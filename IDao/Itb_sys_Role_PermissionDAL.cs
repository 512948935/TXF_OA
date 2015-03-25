using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.Data;

namespace IDao
{
    public interface Itb_sys_Role_PermissionDAL : IBaseDAL<tb_sys_Role_Permission>
    {
        void SavePermissions(List<tb_sys_Role_Permission> Permissions);
        DataTable GetPermissions(int roleID);
    }
}
