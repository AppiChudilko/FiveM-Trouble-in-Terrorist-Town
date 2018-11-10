using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using CitizenFX.Core;
using MySql.Data.MySqlClient;
using Server;
using static CitizenFX.Core.Native.API;

//do not touch namespace!!!
namespace Appi
{
    public class MySql : BaseScript
    {
        /* Variables */
        private static string _connStr;
        private static Dictionary<string, MySqlDataAdapter> _dataAdapters;

        /* Constructor */
        
        public MySql()
        {
            Debug.WriteLine("DATABASE: START " + GetCurrentResourceName());
            Connect();
        }

        /* Exports */
        public static DataTable ExecuteQueryWithResult(string sql, bool isRecconect = false)
        {
            using (var conn = new MySqlConnection(_connStr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    DataTable results = new DataTable();
                    results.Load(rdr);
                    rdr.Close();
                    conn.ClearAllPoolsAsync();
                    conn.CloseAsync();
                    return results;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("DATABASE: [CONNECT] " + (conn.State == ConnectionState.Open));
                    Debug.WriteLine("DATABASE: [ERROR] " + ex);
                }
            }
            
            if(isRecconect)
                return new DataTable();
            
            Thread.Sleep(5000);
            return ExecuteQueryWithResult(sql, true); 
        }

        public static DataTable ExecutePreparedQueryWithResult(string sql, Dictionary<string, string> parameters)
        {
            using (MySqlConnection conn = new MySqlConnection(_connStr))
            {
                try
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    foreach (KeyValuePair<string, string> entry in parameters)
                    {
                        cmd.Parameters.AddWithValue(entry.Key, entry.Value);
                    }

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    DataTable results = new DataTable();
                    results.Load(rdr);
                    rdr.Close();
                    return results;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("DATABASE: [CONNECT] " + (conn.State == ConnectionState.Open));
                    Debug.WriteLine("DATABASE: [ERROR] " + ex);
                }
            }
            Thread.Sleep(5000);
            return ExecutePreparedQueryWithResult(sql, parameters);
        }

        public static void ExecuteQuery(string sql, bool isRecconect = false)
        {
            using (MySqlConnection conn = new MySqlConnection(_connStr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    return;

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("DATABASE: [CONNECT] " + (conn.State == ConnectionState.Open));
                    Debug.WriteLine("DATABASE: [ERROR] " + ex);
                }
            }
            
            if(isRecconect)
                return;
            
            Thread.Sleep(5000);
            ExecuteQuery(sql, true);
        }

        public static void ExecutePreparedQuery(string sql, Dictionary<string, string> parameters)
        {
            using (MySqlConnection conn = new MySqlConnection(_connStr))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    foreach (KeyValuePair<string, string> entry in parameters)
                    {
                        cmd.Parameters.AddWithValue(entry.Key, entry.Value);
                    }
                    cmd.ExecuteNonQuery();
                    return;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("DATABASE: [CONNECT] " + (conn.State == ConnectionState.Open));
                    Debug.WriteLine("DATABASE: [ERROR] " + ex);
                }
            }
            Thread.Sleep(5000);
            ExecutePreparedQuery(sql, parameters);
        }

        public static DataTable CreateDataTable(string sql, string uniqueName)
        {
            using (MySqlConnection conn = new MySqlConnection(_connStr))
            {
                try
                {
                    var dataAdapter = new MySqlDataAdapter(sql, conn);
                    //MySqlCommandBuilder cb = new MySqlCommandBuilder(dataAdapter);
                    _dataAdapters[uniqueName] = dataAdapter;
                    var dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    return dataTable;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("DATABASE: [CONNECT] " + (conn.State == ConnectionState.Open));
                    Debug.WriteLine("DATABASE: [ERROR] " + ex);
                    return null;
                }
            }
        }

        public static void UpdateDataTable(string uniqueName, DataTable updatedTable)
        {
            try
            {
                _dataAdapters[uniqueName].Update(updatedTable);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DATABASE: [ERROR] " + ex);
            }
        }

        public static void CloseDataTable(string uniqueName)
        {
            try
            {
                MySqlDataAdapter data = _dataAdapters[uniqueName];
                _dataAdapters.Remove(uniqueName);
                data.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DATABASE: [ERROR] " + ex);
            }
        }

        public void Connect()
        {
            Debug.WriteLine("DATABASE: [INFO] Start connect to MySQL");
            _dataAdapters = new Dictionary<string, MySqlDataAdapter>();

            var dbCfg = Main.LoadJson($@"resources/{GetCurrentResourceName()}/database.json");
          
            string host = (string) dbCfg["host"];
            string port = (string) dbCfg["port"];
            string database = (string) dbCfg["database"];
            string user = (string) dbCfg["user"];
            string pass = (string) dbCfg["pass"];
            string minPool = (string) dbCfg["minPool"];
            string maxPool = (string) dbCfg["maxPool"];
            
            _connStr = "server=" + host +
                       ";user=" + user +
                       ";database=" + database +
                       ";port=" + port +
                       ";password=" + pass +
                       ";min pool size=" + minPool +
                       ";max pool size=" + maxPool + ";" +
                       "charset=utf8;";
            
            using (MySqlConnection conn = new MySqlConnection(_connStr))
            {
                try
                {
                    Debug.WriteLine("DATABASE: [INFO] Attempting connecting to MySQL");
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        Debug.WriteLine("DATABASE: [INFO] Connected to MySQL");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("DATABASE: [CONNECT] " + (conn.State == ConnectionState.Open));
                    Debug.WriteLine("DATABASE: [ERROR] " + ex);
                    if (conn.State != ConnectionState.Open)
                    {
                        Thread.Sleep(5000);
                        Connect();
                    }
                }
            }
        }
    }
}