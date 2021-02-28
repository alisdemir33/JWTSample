using System;
using System.Data;
using System.Data.SqlClient;

namespace VakifIlan
{
    public class SqlDatabase
    {
        public string strDBServerName;
        public string strDBName;
        public string strDBUserName;
        public string strDBPassword;
        SqlConnection Con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        DataTable dtMax;
        DataTable dtMin;
        string strResult = "";
        int iResult = -1;

        public SqlDatabase()
        {
            try
            {
                strDBServerName = System.Configuration.ConfigurationManager.AppSettings["DB_Server_Name"].ToString();
                strDBName = System.Configuration.ConfigurationManager.AppSettings["DB_Name"].ToString();
                strDBUserName = System.Configuration.ConfigurationManager.AppSettings["DB_User_Name"].ToString();
                strDBPassword = System.Configuration.ConfigurationManager.AppSettings["DB_Password"].ToString();
            }
            catch
            { }
        }
        
        public SqlDatabase(string sServer_Name, string sDB_Name, string sUser_Name, string sPassword)
        {
            try
            {
                strDBServerName = sServer_Name;
                strDBName = sDB_Name;
                strDBUserName = sUser_Name;
                strDBPassword = sPassword;
            }
            catch
            { }
        }

        public string GetConnectionString()
        {
            string strConnectionString = "data source=" + strDBServerName + ";packet size=4096;database=" + strDBName + ";User id=" + strDBUserName + ";password=" + strDBPassword;
            return strConnectionString;
        }
        
        private SqlConnection GetDatabaseConnection()
        {
            Con = new SqlConnection(GetConnectionString());
            try
            {
                Con.Open();
            }
            catch (SqlException Exc)
            {
                throw new Exception(Exc.Message);
            }
            return Con;
        }

        public string ExecuteCommand(string strSQL)
        {
            strResult = "No Execution";
            Con = GetDatabaseConnection();
            strResult = ExecuteCommand(strSQL, Con);
            return strResult;
        }
        
        //public string ExecuteCommand(string strSQL, int iTimeout)
        //{
        //    strResult = "No Execution";
        //    Con = GetDatabaseConnection();
        //    strResult = ExecuteCommand(strSQL, Con, iTimeout);
        //    return strResult;
        //}

        public string ExecuteCommand(string strSQL, SqlConnection pCon)
        {
            strResult = "No Execution";
            strResult = ExecuteCommand(strSQL, pCon, 150);
            return strResult;
        }

        public string ExecuteCommand(string strSQL, SqlConnection pCon, int iTimeout)
        {
            strResult = "No Execution";

            try
            {
                cmd = new SqlCommand(strSQL, pCon);
                cmd.CommandTimeout = iTimeout;
                cmd.ExecuteNonQuery();
                strResult = "OK";
            }
            catch (SqlException SExc)
            {
                strResult = SExc.Number.ToString() + " - " + SExc.Message.ToString();
            }
            finally
            {

            }

            try
            {
                Con.Close();
            }
            catch { }

            return strResult;
        }

        //public int ExecuteCommandIdentity(string strSQL)
        //{
        //    iResult = -1;
        //    Con = GetDatabaseConnection();
        //    iResult = ExecuteCommandIdentity(strSQL, Con);
        //    return iResult;
        //}
        
        //public int ExecuteCommandIdentity(string strSQL, int iTimeout)
        //{
        //    iResult = -1;
        //    Con = GetDatabaseConnection();
        //    iResult = ExecuteCommandIdentity(strSQL, Con, iTimeout);
        //    return iResult;
        //}

        //public int ExecuteCommandIdentity(string strSQL, SqlConnection pCon)
        //{
        //    iResult = -1;
        //    iResult = ExecuteCommandIdentity(strSQL, pCon, 150);
        //    return iResult;
        //}
       
        //public int ExecuteCommandIdentity(string strSQL, SqlConnection pCon, int iTimeout)
        //{
        //    iResult = -1;

        //    try
        //    {
        //        cmd = new SqlCommand(strSQL, pCon);
        //        cmd.CommandTimeout = iTimeout;
        //        cmd.ExecuteNonQuery();
        //        cmd.CommandText = " Select @@Identity ";
        //        iResult = (int)cmd.ExecuteScalar();
        //    }
        //    catch { }

        //    try
        //    {
        //        pCon.Close();
        //    }
        //    catch { }

        //    return iResult;
        //}

        //public string ExecuteCommand(SqlCommand pcmd)
        //{
        //    Con = GetDatabaseConnection();
        //    strResult = ExecuteCommand(pcmd, Con);
        //    return strResult;
        //}
        
        //public string ExecuteCommand(SqlCommand pcmd, SqlConnection pCon)
        //{
        //    strResult = "No Execution";

        //    try
        //    {
        //        pcmd.Connection = pCon;
        //        pcmd.ExecuteNonQuery();
        //        strResult = "OK";
        //    }
        //    catch (SqlException SExc)
        //    {
        //        strResult = SExc.Number.ToString() + " - " + SExc.Message.ToString();
        //    }
        //    finally
        //    {

        //    }

        //    try
        //    {
        //        pCon.Close();
        //    }
        //    catch { }

        //    return strResult;
        //}

        public System.Data.DataTable GetDataTable(string strSQL)
        {
            dt = new DataTable();
            Con = GetDatabaseConnection();
            dt = GetDataTable(strSQL, Con);
            return dt;
        }
        
        public System.Data.DataTable GetDataTable(string strSQL, SqlConnection pCon)
        {
            dt = new DataTable();
            sda = new SqlDataAdapter(strSQL, pCon);

            try
            {
                sda.SelectCommand.CommandTimeout = 150;
                sda.Fill(dt);
            }
            catch
            {
                dt = new DataTable();
            }

            dt.TableName = "dtGeneral";

            try
            {
                pCon.Close();
            }
            catch { }

            return dt;
        }               

    }
}