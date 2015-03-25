using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model;

namespace IBLL
{
    public interface Itb_item_UserBLL : IBaseBLL<tb_item_User>
    {
        DataTable GetPageList(bool fromCache, int page, int pagesize, out int total, string code, string disabled, List<WhereField> listWhere);
        DataRow GetModel(int id);
        int CheckItemNo(int id, string itemNo);
        void DeleteDepinfo(string id);

        List<OnlineUsers> GetAll();
        void AddToCache(tb_item_User user, Guid uniqueID);
        bool Remove(Guid id);
        bool RemoveAll();
        OnlineUsers Get(Guid id);
    }
}
