using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.Data;

namespace IBLL
{
    public interface Itb_item_DepartmentBLL : IBaseBLL<tb_item_Department>
    {
        DataTable GetPageList(int page, int pagesize, out int total, string code, string isDelete, string where);
    }
}
