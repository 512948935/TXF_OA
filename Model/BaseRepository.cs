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
        public static tb_sys_User User { get; set; }
        /// <summary>
        ///创建时间
        /// </summary>
        [Model(Name = "创建时间", Empty = true, DataType = DbType.DateTime, Readonly = true, IsNotUpdate = true)]
        public DateTime CreateOn
        {
            get { return DateTime.Now; }
        }
        private int m_CreateUserID;
        /// <summary>
        ///创建人ID
        /// </summary>
        [Model(Name = "创建人ID", Empty = true, DataType = DbType.Int32, Readonly = true, IsNotUpdate = true)]
        public int CreateUserID
        {
            get
            {
                if (User != null) m_CreateUserID = User.ID;
                return m_CreateUserID;
            }
        }
        private string m_CreateBy;
        /// <summary>
        ///创建人
        /// </summary>
        [Model(Name = "创建人", Empty = true, DataType = DbType.String, MaxLength = 20, Readonly = true, IsNotUpdate = true)]
        public string CreateBy
        {
            get
            {
                if (User != null) m_CreateBy = User.UserName;
                return m_CreateBy;
            }
        }
        private DateTime m_ModifiedOn;
        /// <summary>
        ///修改时间
        /// </summary>
        [Model(Name = "修改时间", Empty = true, DataType = DbType.DateTime, Readonly = true)]
        public DateTime ModifiedOn
        {
            set { m_ModifiedOn = value; }
            get { return DateTime.Now; }
        }
        private int m_ModifiedUserID;
        /// <summary>
        ///修改人ID
        /// </summary>
        [Model(Name = "修改人ID", Empty = true, DataType = DbType.Int32, Readonly = true)]
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
        [Model(Name = "修改人", Empty = true, DataType = DbType.String, MaxLength = 20, Readonly = true)]
        public string ModifiedBy
        {
            set { m_ModifiedBy = value; }
            get
            {
                if (User != null) m_ModifiedBy = User.UserName;
                return m_ModifiedBy;
            }
        }
    }
}
