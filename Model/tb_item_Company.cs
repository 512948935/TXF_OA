using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TXF.Attributes;

namespace Model
{
    public class tb_item_Company : BaseRepository
    {
        private int m_ID;
        /// <summary>
        ///ID
        /// </summary>
        [Model(Name = "ID", Empty = false, DataType = DbType.Int32, MaxLength = 4, PrimaryKey = true, AutoIncrement = true)]
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
        ///公司代码
        /// </summary>
        [Model(Name = "公司代码", Empty = false, DataType = DbType.String, MaxLength = 100)]
        public string ItemNo
        {
            get { return m_ItemNo; }
            set { m_ItemNo = value; }
        }
        private string m_ItemName;
        /// <summary>
        ///公司名称
        /// </summary>
        [Model(Name = "公司名称", Empty = false, DataType = DbType.String, MaxLength = 100)]
        public string ItemName
        {
            get { return m_ItemName; }
            set { m_ItemName = value; }
        }
        private string m_ShortName;
        /// <summary>
        ///公司简称
        /// </summary>
        [Model(Name = "公司简称", Empty = true, DataType = DbType.String, MaxLength = 40)]
        public string ShortName
        {
            get { return m_ShortName; }
            set { m_ShortName = value; }
        }
        private string m_CompanyTel;
        /// <summary>
        ///公司电话
        /// </summary>
        [Model(Name = "公司电话", Empty = true, DataType = DbType.String, MaxLength = 20)]
        public string CompanyTel
        {
            get { return m_CompanyTel; }
            set { m_CompanyTel = value; }
        }
        private string m_CompanyAddress;
        /// <summary>
        ///公司地址
        /// </summary>
        [Model(Name = "公司地址", Empty = true, DataType = DbType.String, MaxLength = 20)]
        public string CompanyAddress
        {
            get { return m_CompanyAddress; }
            set { m_CompanyAddress = value; }
        }
        private DateTime? m_SetUpTime;
        /// <summary>
        ///公司成立时间
        /// </summary>
        [Model(Name = "公司成立时间", Empty = true, DataType = DbType.DateTime)]
        public DateTime? SetUpTime
        {
            get { return m_SetUpTime; }
            set { m_SetUpTime = value; }
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
        ///是否禁用
        /// </summary>
        [Model(Name = "是否禁用", Empty = true, DataType = DbType.Boolean)]
        public bool IsDisabled
        {
            get { return m_IsDisabled; }
            set { m_IsDisabled = value; }
        }
    }
}
