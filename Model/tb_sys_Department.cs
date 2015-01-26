using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TXF.Attributes;

namespace Model
{
    public class tb_item_Department : BaseRepository
    {
        private int m_ID;
        /// <summary>
        ///
        /// </summary>
        [Model(Name = "主键ID", Empty = false, DataType = DbType.Int32, PrimaryKey = true, AutoIncrement = true)]
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }
        private int m_ItemID;
        /// <summary>
        ///数据字典ID
        /// </summary>
        [Model(Name = "数据字典ID", Empty = false, DataType = DbType.Int32)]
        public int ItemID
        {
            get { return m_ItemID; }
            set { m_ItemID = value; }
        }
        private string m_ItemNo;
        /// <summary>
        ///部门代码
        /// </summary>
        [Model(Name = "部门代码", Empty = false, DataType = DbType.String, MaxLength = 50)]
        public string ItemNo
        {
            get { return m_ItemNo; }
            set { m_ItemNo = value; }
        }
        private string m_ItemName;
        /// <summary>
        ///
        /// </summary>
        [Model(Name = "部门名称", Empty = false, DataType = DbType.String, MaxLength = 100)]
        public string ItemName
        {
            get { return m_ItemName; }
            set { m_ItemName = value; }
        }
        private int m_EmpID;
        /// <summary>
        ///部门电话
        /// </summary>
        [Model(Name = "部门电话", Empty = true, DataType = DbType.Int32, MaxLength = 4)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; }
        }
        private string m_DepTel;
        /// <summary>
        ///部门电话
        /// </summary>
        [Model(Name = "部门电话", Empty = true, DataType = DbType.String, MaxLength = 20)]
        public string DepTel
        {
            get { return m_DepTel; }
            set { m_DepTel = value; }
        }
        private string m_DepFax;
        /// <summary>
        ///部门传真
        /// </summary>
        [Model(Name = "部门传真", Empty = true, DataType = DbType.String, MaxLength = 20)]
        public string DepFax
        {
            get { return m_DepFax; }
            set { m_DepFax = value; }
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
        private bool m_IsDelete;
        /// <summary>
        ///
        /// </summary>
        [Model(Name = "是否禁用", Empty = true, DataType = DbType.Boolean)]
        public bool IsDelete
        {
            get { return m_IsDelete; }
            set { m_IsDelete = value; }
        }
    }
}
