using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TXF.Attributes;

namespace Model
{
    public class tb_sys_User:BaseRepository
    {
        private int m_ID;
        /// <summary>
        ///ID
        /// </summary>
        [Model(Name = "ID", Empty = false, DataType = DbType.Int32, PrimaryKey = true, AutoIncrement = true)]
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }
        private string m_UserNo;
        /// <summary>
        ///用户编号
        /// </summary>
        [Model(Name = "用户编号", Empty = false, DataType = DbType.String, MaxLength = 50, IsWhere = true)]
        public string UserNo
        {
            get { return m_UserNo; }
            set { m_UserNo = value; }
        }
        private string m_UserName;
        /// <summary>
        ///登录帐号
        /// </summary>
        [Model(Name = "登录帐号", Empty = false, DataType = DbType.String, MaxLength = 50, IsWhere = true)]
        public string UserName
        {
            get { return m_UserName; }
            set { m_UserName = value; }
        }
        private string m_UserPwd;
        /// <summary>
        ///密码
        /// </summary>
        [Model(Name = "密码", Empty = false, DataType = DbType.String, MaxLength = 200)]
        public string UserPwd
        {
            get { return m_UserPwd; }
            set { m_UserPwd = value; }
        }
        private string m_RealName;
        /// <summary>
        ///用户姓名
        /// </summary>
        [Model(Name = "用户姓名", Empty = false, DataType = DbType.String, MaxLength = 50, IsWhere = true)]
        public string RealName
        {
            get { return m_RealName; }
            set { m_RealName = value; }
        }
        private int m_DepID;
        /// <summary>
        ///部门ID
        /// </summary>
        [Model(Name = "部门ID", Empty = true, DataType = DbType.Int32)]
        public int DepID
        {
            get { return m_DepID; }
            set { m_DepID = value; }
        }
        private string m_DepName;
        /// <summary>
        ///部门名称
        /// </summary>
        [Model(Name = "部门名称", Empty = true, DataType = DbType.String, MaxLength = 50, IsWhere = true)]
        public string DepName
        {
            get { return m_DepName; }
            set { m_DepName = value; }
        }
        private int? m_RoleID;
        /// <summary>
        ///角色ID
        /// </summary>
        [Model(Name = "角色ID", Empty = true, DataType = DbType.Int32)]
        public int? RoleID
        {
            get { return m_RoleID; }
            set { m_RoleID = value; }
        }
        private string m_RoleName;
        /// <summary>
        ///角色名称
        /// </summary>
        [Model(Name = "角色名称", Empty = true, DataType = DbType.String, MaxLength = 50)]
        public string RoleName
        {
            get { return m_RoleName; }
            set { m_RoleName = value; }
        }
        private string m_UserDuty;
        /// <summary>
        ///职位
        /// </summary>
        [Model(Name = "职位", Empty = true, DataType = DbType.String, MaxLength = 50)]
        public string UserDuty
        {
            get { return m_UserDuty; }
            set { m_UserDuty = value; }
        }
        private bool m_IsWorking;
        /// <summary>
        ///是否在岗
        /// </summary>
        [Model(Name = "是否在岗", Empty = true, DataType = DbType.Boolean)]
        public bool IsWorking
        {
            get { return m_IsWorking; }
            set { m_IsWorking = value; }
        }
        private string m_Email;
        /// <summary>
        ///邮箱
        /// </summary>
        [Model(Name = "邮箱", Empty = true, DataType = DbType.String, MaxLength = 50, Rule = @"([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$")]
        public string Email
        {
            get { return m_Email; }
            set { m_Email = value; }
        }
        private string m_FaceSrc;
        /// <summary>
        ///48×48
        /// </summary>
        [Model(Name = "48×48", Empty = true, DataType = DbType.String, MaxLength = 200)]
        public string FaceSrc
        {
            get { return m_FaceSrc; }
            set { m_FaceSrc = value; }
        }
        private string m_AvatarSrc;
        /// <summary>
        ///180×180
        /// </summary>
        [Model(Name = "180×180", Empty = true, DataType = DbType.String, MaxLength = 200)]
        public string AvatarSrc
        {
            get { return m_AvatarSrc; }
            set { m_AvatarSrc = value; }
        }
        private bool m_IsActive;
        /// <summary>
        ///是否激活
        /// </summary>
        [Model(Name = "是否激活", Empty = true, DataType = DbType.Boolean)]
        public bool IsActive
        {
            get { return m_IsActive; }
            set { m_IsActive = value; }
        }
        private int m_Sex;
        /// <summary>
        ///性别
        /// </summary>
        [Model(Name = "性别", Empty = true, DataType = DbType.Int32, MaxLength = 4)]
        public int Sex
        {
            get { return m_Sex; }
            set { m_Sex = value; }
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

        #region 人事部信息
        private DateTime? m_BirthDay;
        /// <summary>
        ///出生日期
        /// </summary>
        [Model(Name = "出生日期", Empty = true, DataType = DbType.DateTime)]
        public DateTime? BirthDay
        {
            get { return m_BirthDay; }
            set { m_BirthDay = value; }
        }
        private string m_Ethnic;
        /// <summary>
        ///名族
        /// </summary>
        [Model(Name = "名族", Empty = true, DataType = DbType.String, MaxLength = 20)]
        public string Ethnic
        {
            get { return m_Ethnic; }
            set { m_Ethnic = value; }
        }
        private string m_IDCardNo;
        /// <summary>
        ///身份证
        /// </summary>
        [Model(Name = "身份证", Empty = true, DataType = DbType.String, MaxLength = 50)]
        public string IDCardNo
        {
            get { return m_IDCardNo; }
            set { m_IDCardNo = value; }
        }
        private bool m_IsWedding;
        /// <summary>
        ///婚宴状况
        /// </summary>
        [Model(Name = "婚宴状况", Empty = true, DataType = DbType.Boolean)]
        public bool IsWedding
        {
            get { return m_IsWedding; }
            set { m_IsWedding = value; }
        }
        private string m_Faction;
        /// <summary>
        ///政治面貌
        /// </summary>
        [Model(Name = "政治面貌", Empty = true, DataType = DbType.String, MaxLength = 20)]
        public string Faction
        {
            get { return m_Faction; }
            set { m_Faction = value; }
        }
        private string m_Origin;
        /// <summary>
        ///籍贯
        /// </summary>
        [Model(Name = "籍贯", Empty = true, DataType = DbType.String, MaxLength = 20)]
        public string Origin
        {
            get { return m_Origin; }
            set { m_Origin = value; }
        }
        private string m_Household;
        /// <summary>
        ///
        /// </summary>
        [Model(Name = "", Empty = true, DataType = DbType.String, MaxLength = 200)]
        public string Household
        {
            get { return m_Household; }
            set { m_Household = value; }
        }
        private string m_Education;
        /// <summary>
        ///
        /// </summary>
        [Model(Name = "", Empty = true, DataType = DbType.String, MaxLength = 20)]
        public string Education
        {
            get { return m_Education; }
            set { m_Education = value; }
        }
        private string m_Technical;
        /// <summary>
        ///职称
        /// </summary>
        [Model(Name = "职称", Empty = true, DataType = DbType.String, MaxLength = 40)]
        public string Technical
        {
            get { return m_Technical; }
            set { m_Technical = value; }
        }
        private string m_University;
        /// <summary>
        ///毕业院校
        /// </summary>
        [Model(Name = "毕业院校", Empty = true, DataType = DbType.String, MaxLength = 40)]
        public string University
        {
            get { return m_University; }
            set { m_University = value; }
        }
        private string m_Major;
        /// <summary>
        ///
        /// </summary>
        [Model(Name = "", Empty = true, DataType = DbType.String, MaxLength = 40)]
        public string Major
        {
            get { return m_Major; }
            set { m_Major = value; }
        }
        private DateTime? m_WorkTime;
        /// <summary>
        ///参加工作时间
        /// </summary>
        [Model(Name = "参加工作时间", Empty = true, DataType = DbType.DateTime, MaxLength = 8)]
        public DateTime? WorkTime
        {
            get { return m_WorkTime; }
            set { m_WorkTime = value; }
        }
        private DateTime? m_JoinTime;
        /// <summary>
        ///加入本单位时间
        /// </summary>
        [Model(Name = "加入本单位时间", Empty = true, DataType = DbType.DateTime, MaxLength = 8)]
        public DateTime? JoinTime
        {
            get { return m_JoinTime; }
            set { m_JoinTime = value; }
        }
        private string m_Phone;
        /// <summary>
        ///手机号码
        /// </summary>
        [Model(Name = "手机号码", Empty = true, DataType = DbType.String, MaxLength = 20)]
        public string Phone
        {
            get { return m_Phone; }
            set { m_Phone = value; }
        }
        private string m_FamilyAddress;
        /// <summary>
        ///家庭住址
        /// </summary>
        [Model(Name = "家庭住址", Empty = true, DataType = DbType.String, MaxLength = 200)]
        public string FamilyAddress
        {
            get { return m_FamilyAddress; }
            set { m_FamilyAddress = value; }
        }
        #endregion
    }
}
