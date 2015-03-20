using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TXF.Attributes;

namespace Model
{
    public class tb_sys_Module
    {
        private int m_ID;
        /// <summary>
        ///主键
        /// </summary>
        [Model(Name = "主键", Empty = false, DataType = DbType.Int32, PrimaryKey = true, AutoIncrement = true)]
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }
        private int m_ParentID;
        /// <summary>
        ///父节点ID
        /// </summary>
        [Model(Name = "父节点ID", Empty = false, DataType = DbType.Int32)]
        public int ParentID
        {
            get { return m_ParentID; }
            set { m_ParentID = value; }
        }
        private int m_NodeLevel;
        /// <summary>
        ///级次
        /// </summary>
        [Model(Name = "级次", Empty = false, DataType = DbType.Int32)]
        public int NodeLevel
        {
            get { return m_NodeLevel; }
            set { m_NodeLevel = value; }
        }
        private string m_NodeState;
        /// <summary>
        ///节点状态
        /// </summary>
        [Model(Name = "节点状态", Empty = true, DataType = DbType.String, NotAdd = true)]
        public string NodeState
        {
            get { return m_NodeState; }
            set { m_NodeState = value; }
        }
        private string m_ModuleCode;
        /// <summary>
        ///模块代码
        /// </summary>
        [Model(Name = "模块代码", Empty = false, DataType = DbType.String, MaxLength = 50)]
        public string ModuleCode
        {
            get { return m_ModuleCode; }
            set { m_ModuleCode = value; }
        }
        private string m_ModuleName;
        /// <summary>
        ///模块名称
        /// </summary>
        [Model(Name = "模块名称", Empty = false, DataType = DbType.String, MaxLength = 100)]
        public string ModuleName
        {
            get { return m_ModuleName; }
            set { m_ModuleName = value; }
        }
        private string m_PageUrl;
        /// <summary>
        ///页面地址
        /// </summary>
        [Model(Name = "页面地址", Empty = true, DataType = DbType.String, MaxLength = 50)]
        public string PageUrl
        {
            get { return m_PageUrl; }
            set { m_PageUrl = value; }
        }
        private string m_Icon;
        /// <summary>
        ///图标
        /// </summary>
        [Model(Name = "图标", Empty = true, DataType = DbType.String, MaxLength = 50)]
        public string Icon
        {
            get { return m_Icon; }
            set { m_Icon = value; }
        }
        private string m_ButtonID;
        /// <summary>
        ///按钮ID
        /// </summary>
        [Model(Name = "按钮ID", Empty = true, DataType = DbType.String, MaxLength = 20)]
        public string ButtonID
        {
            get { return m_ButtonID; }
            set { m_ButtonID = value; }
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
        private bool m_IsPage;
        /// <summary>
        ///是否是页面信息
        /// </summary>
        [Model(Name = "是否是页面信息", Empty = true, DataType = DbType.Boolean)]
        public bool IsPage
        {
            get { return m_IsPage; }
            set { m_IsPage = value; }
        }
        private bool m_IsItem;
        /// <summary>
        ///  是否为数据管理项
        /// </summary>
        [Model(Name = "是否为数据管理项", Empty = true, DataType = DbType.Boolean, NotAdd = true, NotUpdate = true)]
        public bool IsItem
        {
            get { return m_IsItem; }
            set { m_IsItem = value; }
        }
    }
}
