using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class OnlineUsers : tb_item_User
    {
        public string ClientInfo { get; set; }
        public string IP { get; set; }
        public DateTime LoginTime { get; set; }
        public Guid UniqueID { get; set; }
        public string LastPage { get; set; }
    }
}
