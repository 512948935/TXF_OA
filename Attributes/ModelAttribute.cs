using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace TXF.Attributes
{
    /// <summary>
    /// 为Model添加列属性的特性类
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class ModelAttribute : Attribute
    {
        /// <summary>
        /// 字段是否允许为空
        /// </summary>
        public bool Empty { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        public DbType DataType { get; set; }
        /// <summary>
        /// 字段注释，比如字段为：Title，那么Name的值可以设置为：标题
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 字段最小长度
        /// </summary>
        public int MinLength { get; set; }
        /// <summary>
        /// 字段最大长度
        /// </summary>
        public int MaxLength { get; set; }
        /// <summary>
        /// 验证字段的正则表达式
        /// </summary>
        public string Rule { get; set; }
        /// <summary>
        /// 出错提示
        /// </summary>
        public string ErrorTip { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        public bool PrimaryKey { get; set; }
        /// <summary>
        /// 自增长
        /// </summary>
        public bool AutoIncrement { get; set; }
        /// <summary>
        /// 是否相等
        /// </summary>
        public bool isEqual { get; set; }
        /// <summary>
        /// 比较值
        /// </summary>
        public string isEqualText { get; set; }
        /// <summary>
        /// 只读字段
        /// </summary>
        public bool Readonly { get; set; }
    }
}
