using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TXF.Attributes;

namespace Model
{
    public class tb_item_User : BaseRepository
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
        private int m_ItemID;
        /// <summary>
        ///数据字典ID
        /// </summary>
        [Model(Name = "数据字典ID", Empty = false, DataType = DbType.Int32, MaxLength = 4)]
        public int ItemID
        {
            get { return m_ItemID; }
            set { m_ItemID = value; }
        }
        private string m_ItemNo;
        /// <summary>
        ///用户编号
        /// </summary>
        [Model(Name = "用户编号", Empty = false, DataType = DbType.String, MaxLength = 100)]
        public string ItemNo
        {
            get { return m_ItemNo; }
            set { m_ItemNo = value; }
        }
        private string m_ItemName;
        /// <summary>
        ///登录帐号
        /// </summary>
        [Model(Name = "登录帐号", Empty = false, DataType = DbType.String, MaxLength = 50)]
        public string ItemName
        {
            get { return m_ItemName; }
            set { m_ItemName = value; }
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
        [Model(Name = "用户姓名", Empty = true, DataType = DbType.String, MaxLength = 50)]
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

        #region 人事信息
        private string m_Sex;
        /// <summary>
        ///性别
        /// </summary>
        [Model(Name = "性别", Empty = true, DataType = DbType.String, MaxLength = 4)]
        public string Sex
        {
            get { return m_Sex; }
            set { m_Sex = value; }
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
        private string m_BankNo;
        /// <summary>
        ///身份证
        /// </summary>
        [Model(Name = "银行帐号", Empty = true, DataType = DbType.String, MaxLength = 50)]
        public string BankNo
        {
            get { return m_BankNo; }
            set { m_BankNo = value; }
        }
        private string m_IsWedding;
        /// <summary>
        ///婚宴状况
        /// </summary>
        [Model(Name = "婚宴状况", Empty = true, DataType = DbType.String, MaxLength = 5)]
        public string IsWedding
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
        /// 户籍所在地
        /// </summary>
        [Model(Name = "户籍所在地", Empty = true, DataType = DbType.String, MaxLength = 200)]
        public string Household
        {
            get { return m_Household; }
            set { m_Household = value; }
        }
        private string m_Education;
        /// <summary>
        /// 学历
        /// </summary>
        [Model(Name = "学历", Empty = true, DataType = DbType.String, MaxLength = 20)]
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
        ///  专业
        /// </summary>
        [Model(Name = "", Empty = true, DataType = DbType.String, MaxLength = 40)]
        public string Major
        {
            get { return m_Major; }
            set { m_Major = value; }
        }
        private DateTime? m_JoinTime;
        /// <summary>
        ///入职时间
        /// </summary>
        [Model(Name = "入职时间", Empty = true, DataType = DbType.DateTime)]
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
        private bool m_IsWorking = true;
        /// <summary>
        ///是否在岗
        /// </summary>
        [Model(Name = "是否在岗", Empty = true, DataType = DbType.Boolean)]
        public bool IsWorking
        {
            get { return m_IsWorking; }
            set { m_IsWorking = value; }
        }
        #endregion
    }
}
