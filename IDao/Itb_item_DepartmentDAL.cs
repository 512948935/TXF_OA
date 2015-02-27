using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.Data;

namespace IDao
{
    public interface Itb_item_DepartmentDAL : IBaseDAL<tb_item_Department>
    {
        DataTable GetPageList(int page, int pagesize, out int total, string code, string disabled, List<WhereField> listWhere);
        void DeleteDepinfo(string id);
        int CheckItemNo(int id, string itemNo);
    }
}
