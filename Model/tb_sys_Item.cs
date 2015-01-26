using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TXF.Attributes;

namespace Model
{
    public class tb_sys_Item
    {
        private int m_ID;
        /// <summary>
        ///主键ID
        /// </summary>
        [Model(Name = "主键ID", Empty = false, DataType = DbType.Int32, MaxLength = 4, PrimaryKey = true, AutoIncrement = true)]
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }
        private int m_ParentID;
        /// <summary>
        ///父级ID
        /// </summary>
        [Model(Name = "父级ID", Empty = false, DataType = DbType.Int32, MaxLength = 4)]
        public int ParentID
        {
            get { return m_ParentID; }
            set { m_ParentID = value; }
        }
        private int m_NodeLevel;
        /// <summary>
        ///类别级次
        /// </summary>
        [Model(Name = "类别级次", Empty = false, DataType = DbType.Int32, MaxLength = 4)]
        public int NodeLevel
        {
            get { return m_NodeLevel; }
            set { m_NodeLevel = value; }
        }
        private string m_NodeCode;
        /// <summary>
        ///类别代码
        /// </summary>
        [Model(Name = "类别代码", Empty = false, DataType = DbType.String, MaxLength = 50)]
        public string NodeCode
        {
            get { return m_NodeCode; }
            set { m_NodeCode = value; }
        }
        private string m_NodeName;
        /// <summary>
        ///类别名称
        /// </summary>
        [Model(Name = "类别名称", Empty = false, DataType = DbType.String, MaxLength = 100)]
        public string NodeName
        {
            get { return m_NodeName; }
            set { m_NodeName = value; }
        }
        private int m_NodeType;
        /// <summary>
        ///
        /// </summary>
        [Model(Name = "", Empty = false, DataType = DbType.Int32, MaxLength = 4)]
        public int NodeType
        {
            get { return m_NodeType; }
            set { m_NodeType = value; }
        }
        private string m_NodeState;
        /// <summary>
        /// 节点状态
        /// </summary>
        [Model(Name = "节点状态", Empty = true, DataType = DbType.String, MaxLength = 10)]
        public string NodeState
        {
            get { return m_NodeState; }
            set { m_NodeState = value; }
        }
        private string m_TableName;
        /// <summary>
        ///表名
        /// </summary>
        [Model(Name = "表名", Empty = true, DataType = DbType.String, MaxLength = 50)]
        public string TableName
        {
            get { return m_TableName; }
            set { m_TableName = value; }
        }
    }
}
