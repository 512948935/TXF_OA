using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TXF.Attributes;

namespace Model
{
    public class tb_sys_Role_Permission:BaseRepository
    {
        private int m_ID;
        /// <summary>
        /// 主键ID
        /// </summary>
        [Model(Name = "主键ID", Empty = false, DataType = DbType.Int32, MaxLength = 4, PrimaryKey = true, AutoIncrement = true)]
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }
        private int m_RoleID;
        /// <summary>
        /// 角色ID
        /// </summary>
        [Model(Name = "角色ID", Empty = false, DataType = DbType.Int32)]
        public int RoleID
        {
            get { return m_RoleID; }
            set { m_RoleID = value; }
        }
        private int m_ModuleID;
        /// <summary>
        /// 模块ID
        /// </summary>
        [Model(Name = "模块ID", Empty = false, DataType = DbType.Int32)]
        public int ModuleID
        {
            get { return m_ModuleID; }
            set { m_ModuleID = value; }
        }
        private string m_ButtonID;
        /// <summary>
        ///  按钮ID
        /// </summary>
        [Model(Name = "按钮ID", Empty = true, DataType = DbType.String, MaxLength = 20)]
        public string ButtonID
        {
            get { return m_ButtonID; }
            set { m_ButtonID = value; }
        }
        private bool m_IsChecked;
        /// <summary>
        ///　是否选中
        /// </summary>
        [Model(Name = "是否选中", Empty = false, DataType = DbType.Boolean)]
        public bool IsChecked
        {
            get { return m_IsChecked; }
            set { m_IsChecked = value; }
        }
    }
}
