using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model;

namespace IBLL
{
    public interface Itb_sys_ButtonBLL : IBaseBLL<tb_sys_Button>
    {
        DataTable GetPageList(bool fromCache, int page, int pagesize, out int total, string code, string disabled, List<WhereField> listWhere);
        DataRow GetModel(int id);
        int CheckItemNo(int id, string itemNo);
        void DeleteDepinfo(string id);
    }
}
