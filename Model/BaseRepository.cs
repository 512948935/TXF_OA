using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TXF.Attributes;

namespace Model
{
    public class BaseRepository
    {
        /// <summary>
        /// 当前登录人
        /// </summary>
        public tb_item_User User { get; set; }
        private DateTime m_CreateOn;
        /// <summary>
        ///创建时间
        /// </summary>
        [Model(Name = "创建时间", Empty = true, DataType = DbType.DateTime, NotUpdate = true)]
        public DateTime CreateOn
        {
            get { return DateTime.Now; }
            set { m_CreateOn = value; }
        }
        private int m_CreateUserID;
        /// <summary>
        ///创建人ID
        /// </summary>
        [Model(Name = "创建人ID", Empty = true, DataType = DbType.Int32, NotUpdate = true)]
        public int CreateUserID
        {
            get
            {
                if (User != null) m_CreateUserID = User.ID;
                return m_CreateUserID;
            }
            set { m_CreateUserID = value; }
        }
        private string m_CreateBy;
        /// <summary>
        ///创建人
        /// </summary>
        [Model(Name = "创建人", Empty = true, DataType = DbType.String, MaxLength = 20, NotUpdate = true)]
        public string CreateBy
        {
            get
            {
                if (User != null) m_CreateBy = User.ItemName;
                return m_CreateBy;
            }
            set { m_CreateBy = value; }
        }
        private DateTime m_ModifiedOn;
        /// <summary>
        ///修改时间
        /// </summary>
        [Model(Name = "修改时间", Empty = true, DataType = DbType.DateTime)]
        public DateTime ModifiedOn
        {
            set { m_ModifiedOn = value; }
            get { return DateTime.Now; }
        }
        private int m_ModifiedUserID;
        /// <summary>
        ///修改人ID
        /// </summary>
        [Model(Name = "修改人ID", Empty = true, DataType = DbType.Int32)]
        public int ModifiedUserID
        {
            set { m_ModifiedUserID = value; }
            get
            {
                if (User != null) ModifiedUserID = User.ID;
                return m_ModifiedUserID;
            }
        }
        private string m_ModifiedBy;
        /// <summary>
        ///修改人
        /// </summary>
        [Model(Name = "修改人", Empty = true, DataType = DbType.String, MaxLength = 20)]
        public string ModifiedBy
        {
            set { m_ModifiedBy = value; }
            get
            {
                if (User != null) m_ModifiedBy = User.ItemName;
                return m_ModifiedBy;
            }
        }
    }
}
