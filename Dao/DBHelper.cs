using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace Dao
{
    public class DBHelper
    {
        #region 创建链接字符串
        //获取解密后的字符串
        public static string connectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ToString();
        private static SqlConnection conn;
        public static SqlConnection Conn
        {
            get
            {
                if (conn == null)
                    conn = new SqlConnection(connectionString);
                return conn;
            }
            set { conn = value; }
        }
        private static object locker = new object();
        /// <summary>
        /// 打开数据库链接
        /// </summary>
        private static void OpenConnection()
        {
            if (Conn.State == ConnectionState.Closed)
                Conn.Open();
            else if (conn.State == ConnectionState.Broken)
            {
                Conn.Close();
                Conn.Open();
            }
        }
        /// <summary>
        /// 关闭数据库链接
        /// </summary>
        public static void CloseConnection()
        {
            if (Conn.State == ConnectionState.Open || Conn.State == ConnectionState.Broken)
                Conn.Close();
        }
        #endregion

        #region CRUD
        /// <summary>
        /// 查找单个记录
        /// </summary>
        /// <param name="strSql">sql字符串</param>
        /// <param name="cmdParms">sql参数</param>
        /// <returns>object</returns>
        public static object SingleQuery(string strSql, SqlParameter[] cmdParms = null)
        {
            lock (locker)
            {
                try
                {
                    OpenConnection();
                    using (SqlCommand cmd = new SqlCommand(strSql, Conn))
                    {
                        if (cmdParms != null)
                            cmd.Parameters.AddRange(cmdParms);
                        return cmd.ExecuteScalar();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    CloseConnection();
                }
            }
        }
        /// <summary>
        /// 查找多条记录绑定到DateTable（带条件参数）
        /// </summary>
        /// <param name="strSql">sql语句，Sql参数集</param>
        /// <returns>DataTable</returns>
        public static DataTable Query(string strSql, SqlParameter[] cmdParms = null, CommandType cmdType = CommandType.Text)
        {
            lock (locker)
            {
                try
                {

                    OpenConnection();
                    DataSet ds = new DataSet();
                    using (SqlCommand cmd = new SqlCommand(strSql, Conn))
                    {
                        if (cmdParms != null)
                            cmd.Parameters.AddRange(cmdParms);
                        cmd.CommandType = cmdType;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                        return ds.Tables[0];
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    CloseConnection();
                }
            }
        }
        /// <summary>
        /// 添加(返回自动增长列)
        /// </summary>
        public static int Add(string strSql, SqlParameter[] cmdParms = null, CommandType cmdType = CommandType.Text)
        {
            try
            {
                OpenConnection();
                using (SqlCommand cmd = new SqlCommand(strSql, Conn))
                {
                    if (cmdParms != null)
                        cmd.Parameters.AddRange(cmdParms);
                    cmd.CommandType = cmdType;
                    int id = Convert.ToInt32(cmd.ExecuteScalar());
                    return id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }
        /// <summary>
        /// 增、删、改（带条件参数）
        /// </summary>
        /// <param name="strSql">sql语句，Sql参数集,sql语句类型</param>
        /// <param name="cmdParms"></param>
        /// <returns>int值</returns>
        public static int ExecuteNonQuery(string strSql, SqlParameter[] cmdParms = null, CommandType cmdType = CommandType.Text)
        {
            int iRet = -1;
            try
            {
                OpenConnection();
                using (SqlCommand cmd = new SqlCommand(strSql, Conn))
                {
                    if (cmdParms != null)
                        cmd.Parameters.AddRange(cmdParms);
                    cmd.CommandType = cmdType;
                    iRet = cmd.ExecuteNonQuery();
                    return iRet;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }
        #endregion
    }
}
