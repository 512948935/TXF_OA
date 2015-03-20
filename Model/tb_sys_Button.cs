using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TXF.Attributes;

namespace Model
{
    public class tb_sys_Button:BaseRepository
    {
        private int m_ID;
        /// <summary>
        /// 主键ID
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
        ///按钮代码
        /// </summary>
        [Model(Name = "按钮代码", Empty = false, DataType = DbType.String, MaxLength = 100)]
        public string ItemNo
        {
            get { return m_ItemNo; }
            set { m_ItemNo = value; }
        }
        private string m_ItemName;
        /// <summary>
        /// 按钮名称
        /// </summary>
        [Model(Name = "按钮名称", Empty = false, DataType = DbType.String, MaxLength = 100)]
        public string ItemName
        {
            get { return m_ItemName; }
            set { m_ItemName = value; }
        }
        private string m_ButtonText;
        /// <summary>
        ///按钮说明
        /// </summary>
        [Model(Name = "按钮说明", Empty = false, DataType = DbType.String, MaxLength = 20)]
        public string ButtonText
        {
            get { return m_ButtonText; }
            set { m_ButtonText = value; }
        }
        private string m_ButtonIcon;
        /// <summary>
        /// 按钮图标
        /// </summary>
        [Model(Name = "按钮图标", Empty = true, DataType = DbType.String, MaxLength = 20)]
        public string ButtonIcon
        {
            get { return m_ButtonIcon; }
            set { m_ButtonIcon = value; }
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
    }
}
