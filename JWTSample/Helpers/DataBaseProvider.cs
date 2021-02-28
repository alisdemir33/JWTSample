using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace VakifIlan
{
    public class DataBaseProvider :IDisposable
    {
        string conStr;

        public DataBaseProvider(string constr) {
            conStr = constr;        
        }

        private ArrayList _parameter = new ArrayList();
        private SqlCommand CreateCommand(string commandText, CommandType commandType)
        {
            SqlCommand command = ConnectionProvider.CreateConnection(this.conStr).CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;
            return command;

        }

        private void ProcessParameter(SqlCommand command)
        {
            foreach (SqlParameter myParameter in _parameter)
            {
                command.Parameters.Add(myParameter);
            }
        }

        public void AddParameter(string parameterName, DbType dbType, object value)
        {

            SqlParameter myParameter = new SqlParameter();
            myParameter.ParameterName = parameterName;
            myParameter.DbType = dbType;
            myParameter.Value = (value == null) ? System.DBNull.Value : value;
            _parameter.Add(myParameter);
        }

        public int ExecuteNonQuery(string commandText, CommandType commandType)
        {
            int returnValue = 0;

            SqlCommand myCommand = this.CreateCommand(commandText, commandType);
            this.ProcessParameter(myCommand);
            try
            {
                myCommand.Connection.Open();
                returnValue = myCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                myCommand.Connection.Close();
                this.ClearParameter();
            }
            return returnValue;

        }

        public DataTable ExecuteDataTable(string commandText, CommandType commandType)
        {
            DataTable dt = new DataTable();

            SqlDataAdapter myAdapter = new SqlDataAdapter();
            myAdapter.SelectCommand = this.CreateCommand(commandText, commandType);
            this.ProcessParameter(myAdapter.SelectCommand);
            try
            {
                myAdapter.SelectCommand.Connection.Open();
                myAdapter.Fill(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                myAdapter.SelectCommand.Connection.Close();
                this.ClearParameter();
            }
            return dt;

        }



        public object ExecuteScalar(string commandText, CommandType commandType)
        {
            object returnValue = null;
            SqlCommand mycommand = this.CreateCommand(commandText, commandType);
            this.ProcessParameter(mycommand);
            try
            {
                mycommand.Connection.Open();
                returnValue = mycommand.ExecuteScalar();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                mycommand.Connection.Close();
                this.ClearParameter();

            }
            return returnValue;
        }

        public SqlDataReader ExecuteReader(string commandText, CommandType commandType)
        {
            SqlDataReader returnValue = null;

            SqlCommand mycommand = this.CreateCommand(commandText, commandType);
            this.ProcessParameter(mycommand);
            try
            {
                mycommand.Connection.Open();
                returnValue = mycommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                this.ClearParameter();
            }
            return returnValue;
        }
        public DataRow ExecuteDataRow(string commandText, CommandType commandType)
        {
            DataTable dt = new DataTable();

            SqlDataAdapter myAdapter = new SqlDataAdapter();
            myAdapter.SelectCommand = this.CreateCommand(commandText, commandType);
            this.ProcessParameter(myAdapter.SelectCommand);
            try
            {
                myAdapter.SelectCommand.Connection.Open();
                myAdapter.Fill(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            finally
            {
                myAdapter.SelectCommand.Connection.Close();
                this.ClearParameter();
            }
            return dt.Rows[0];

        }

        private void ClearParameter()
        {
            _parameter.Clear();
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
