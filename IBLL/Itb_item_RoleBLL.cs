using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.Data;

namespace IBLL
{
    public interface Itb_item_RoleBLL : IBaseBLL<tb_item_Role>
    {
        DataTable GetPageList(bool fromCache, int page, int pagesize, out int total, string code, string disabled, List<WhereField> listWhere);
        DataRow GetModel(int id);
        int CheckItemNo(int id, string itemNo);
        void DeleteDepinfo(string id);
    }
}
