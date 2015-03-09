using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TXF.Attributes;
using System.Data;

namespace Dao.ORM
{
    public class ModelCheck
    {
        /// <summary>
        /// 检测输入数据的合法性，统一在这里处理
        /// </summary>
        /// <param name="modelAttribute"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void CheckInput(ModelAttribute modelAttribute, string value)
        {
            //判断是否允许为空，如果不允许为空
            this.CheckEmpty(modelAttribute, value);//非空验证
            if (!string.IsNullOrEmpty(value))
            {
                this.CheckLength(modelAttribute, value);//长度验证
                this.CheckRule(modelAttribute, value);//正则验证
                this.CheckEqual(modelAttribute, value);//对等验证
                this.CheckType(modelAttribute, value);//数据类型验证
            }
        }
        /// <summary>
        /// 检测是否为空
        /// </summary>
        /// <param name="modelAttribute"></param>
        /// <param name="value"></param>
        private void CheckEmpty(ModelAttribute modelAttribute, string value)
        {
            if (!modelAttribute.Empty && !modelAttribute.AutoIncrement && !modelAttribute.NotAdd && !modelAttribute.NotUpdate)
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception(modelAttribute.Name + "：不能为空.<br/>");
                }
            }
        }
        /// <summary>
        /// 检测长度
        /// </summary>
        /// <param name="modelAttribute"></param>
        /// <param name="value"></param>
        private void CheckLength(ModelAttribute modelAttribute, string value)
        {
            if (modelAttribute.MinLength > 0 || modelAttribute.MaxLength > 0)
            {
                if (value.Length < modelAttribute.MinLength || value.Length > modelAttribute.MaxLength)
                {
                    throw new Exception(modelAttribute.Name + "：数据长度不对.<br/>");
                }
            }
        }
        /// <summary>
        /// 检测正则表达式
        /// </summary>
        /// <param name="modelAttribute"></param>
        /// <param name="value"></param>
        private void CheckRule(ModelAttribute modelAttribute, string value)
        {
            if (!string.IsNullOrEmpty(modelAttribute.Rule))
            {
                if (!IsValidType.RegexMatch(value, modelAttribute.Rule))
                {
                    throw new Exception(modelAttribute.Name + "：数据格式不正确.<br/>");
                }
            }
        }
        /// <summary>
        /// 检测是否相等
        /// </summary>
        /// <param name="modelAttribute"></param>
        /// <param name="value"></param>
        private void CheckEqual(ModelAttribute modelAttribute, string value)
        {
            if (modelAttribute.isEqual)
            {
                if (!value.Equals(modelAttribute.isEqualText))
                {
                    throw new Exception(modelAttribute.Name + "：输入不一致.<br/>");
                }
            }
            else
            {
                if (value.Equals(modelAttribute.isEqualText))
                {
                    throw new Exception(modelAttribute.Name + "：请选择数据.<br/>");
                }
            }
        }
        /// <summary>
        /// 判断数据类型
        /// </summary>
        /// <param name="modelAttribute"></param>
        /// <param name="value"></param>
        private void CheckType(ModelAttribute modelAttribute, string value)
        {
            switch (modelAttribute.DataType)
            {
                case DbType.AnsiString:
                    break;
                case DbType.AnsiStringFixedLength:
                    break;
                case DbType.Binary:
                    break;
                case DbType.Boolean:
                    if (!IsValidType.IsBool(value))
                    {
                         throw new Exception(modelAttribute.Name + "：数据类型不对.<br/>");
                    }
                    break;
                case DbType.Byte:
                    break;
                case DbType.Currency:
                    break;
                case DbType.Date:
                    break;
                case DbType.DateTime:
                    if (!IsValidType.IsDateTime(value))
                    {
                         throw new Exception(modelAttribute.Name + "：时间格式不对.<br/>");
                    }
                    break;
                case DbType.DateTime2:
                    break;
                case DbType.DateTimeOffset:
                    break;
                case DbType.Decimal:
                    if (!IsValidType.IsDecimal(value))
                    {
                         throw new Exception(modelAttribute.Name + "：数据类型不对.<br/>");
                    }
                    break;
                case DbType.Double:
                    if (!IsValidType.IsDouble(value))
                    {
                        throw new Exception(modelAttribute.Name + "：数据类型不对.<br/>");
                    }
                    break;
                case DbType.Guid:
                    break;
                case DbType.Int16:
                    if (!IsValidType.IsInt16(value))
                    {
                        throw new Exception(modelAttribute.Name + "：数据类型不对.<br/>");
                    }
                    break;
                case DbType.Int32:
                    if (!IsValidType.IsInt32(value))
                    {
                        throw new Exception(modelAttribute.Name + "：数据类型不对.<br/>");
                    }
                    break;
                case DbType.Int64:
                    if (!IsValidType.IsInt64(value))
                    {
                        throw new Exception(modelAttribute.Name + "：数据类型不对.<br/>");
                    }
                    break;
                case DbType.Object:
                    break;
                case DbType.SByte:
                    break;
                case DbType.Single:
                    break;
                case DbType.String:
                    break;
                case DbType.StringFixedLength:
                    break;
                case DbType.Time:
                    if (!IsValidType.IsDateTime(value))
                    {
                        throw new Exception(modelAttribute.Name + "：数据类型不对.<br/>");
                    }
                    break;
                case DbType.UInt16:
                    break;
                case DbType.UInt32:
                    break;
                case DbType.UInt64:
                    break;
                case DbType.VarNumeric:
                    break;
                case DbType.Xml:
                    break;
                default:
                    break;
            }
        }
    }
}
