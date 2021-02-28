using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using JWTSample.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace VakifIlan
{
    internal sealed class ConnectionProvider
    {
        //private static string strDBServerName;
        //private static string strDBName;
        //private static string strDBUserName;
        //private static string strDBPassword;
        internal static SqlConnection CreateConnection(string cStr)
        {  
            
            SqlConnection connection = new SqlConnection();
            try
            {
                connection.ConnectionString = cStr;//ConnectionProvider.ConnectionString();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


            return connection;

        }

        internal static string ConnectionString()
        {
            //strDBServerName = System.Configuration.ConfigurationManager.AppSettings["DB_Server_Name"].ToString();
            //strDBName = System.Configuration.ConfigurationManager.AppSettings["DB_Name"].ToString();
            //strDBUserName = System.Configuration.ConfigurationManager.AppSettings["DB_User_Name"].ToString();
            //strDBPassword = System.Configuration.ConfigurationManager.AppSettings["DB_Password"].ToString();

            // CustomConfigManager mng = new CustomConfigManager("212e383c-510b-4609-972e-a75c6642008d");
            ////mng.SetConnectionString("ConnectionString", "Data Source=172.20.40.78;Initial Catalog=PersonelAlim;User ID=personelalimi; pwd=PAlimUser!987;Integrated Security=false; Pooling=false;Connect Timeout=20");

            //return mng.GetConnectionString("ConnectionString");

            AppSettings settings = new AppSettings();


            //IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            //// Duplicate here any configuration sources you use.
            //configurationBuilder.AddJsonFile("../AppSettings.json");
            //IConfiguration configuration = configurationBuilder.Build();


            return ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        }



    }

    internal class ConnProvider
    {
        private readonly AppSettings _settings;
        public ConnProvider(IOptions<AppSettings> appSettings)
        {
            _settings = appSettings.Value;
            // _settings.HostName == "myhost.com";
        }


        public string getConnecionStr() {
            return _settings.ConnStr;
        }


    }





}
