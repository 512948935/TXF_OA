using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TXF.Attributes;


namespace Model
{
    public class tb_item_Role : BaseRepository
    {
        private int m_ID;
        /// <summary>
        /// ID
        /// </summary>
        [Model(Name = "ID", Empty = false, DataType = DbType.Int32, MaxLength = 4, PrimaryKey = true, AutoIncrement = true)]
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }
        private int m_ItemID;
        /// <summary>
        /// 数据字典ID
        /// </summary>
        [Model(Name = "数据字典ID", Empty = false, DataType = DbType.Int32, MaxLength = 4)]
        public int ItemID
        {
            get { return m_ItemID; }
            set { m_ItemID = value; }
        }
        private string m_ItemNo;
        /// <summary>
        /// 角色代码
        /// </summary>
        [Model(Name = "角色代码", Empty = false, DataType = DbType.String, MaxLength = 100)]
        public string ItemNo
        {
            get { return m_ItemNo; }
            set { m_ItemNo = value; }
        }
        private string m_ItemName;
        /// <summary>
        /// 角色名称
        /// </summary>
        [Model(Name = "角色名称", Empty = false, DataType = DbType.String, MaxLength = 50)]
        public string ItemName
        {
            get { return m_ItemName; }
            set { m_ItemName = value; }
        }
        private int m_DepID;
        /// <summary>
        /// 部门ID
        /// </summary>
        [Model(Name = "部门ID", Empty = false, DataType = DbType.Int32, MaxLength = 4)]
        public int DepID
        {
            get { return m_DepID; }
            set { m_DepID = value; }
        }
        private string m_Remark;
        /// <summary>
        ///备注
        /// </summary>
        [Model(Name = "备注", Empty = true, DataType = DbType.String, MaxLength = 1000)]
        public string Remark
        {
            get { return m_Remark; }
            set { m_Remark = value; }
        }
        private bool m_IsDisabled;
        /// <summary>
        /// 是否禁用
        /// </summary>
        [Model(Name = "是否禁用", Empty = true, DataType = DbType.Boolean)]
        public bool IsDisabled
        {
            get { return m_IsDisabled; }
            set { m_IsDisabled = value; }
        }
    }
}
