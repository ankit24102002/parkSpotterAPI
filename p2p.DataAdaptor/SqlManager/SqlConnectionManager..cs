using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace p2p.DataAdaptor.SqlManager
{
    public class SqlConnectionManager
    {
        private static string connString = "server=ANKIT; database=p2p; trusted_connection=true; Encrypt=False;";
        public static void SetConfig()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            var dbConnectionString = configuration.GetConnectionString("dbcs");

            if (!string.IsNullOrEmpty(dbConnectionString))
            {
                connString = dbConnectionString;
            }
        }
        // Connection Metods
        public static IDbConnection GetConnection()
        {
            var conn = new SqlConnection(connString);
            CheckAndOpenConnection(conn);
            return conn;
        }
        private static void CheckAndOpenConnection(SqlConnection conn)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
        }
        private static void CheckAndCloseConnection(SqlConnection conn)
        {
            if (conn.State == ConnectionState.Open || conn.State == ConnectionState.Broken)
            {
                conn.Close();
            }
        }

        public static List<T> GetDataFromSql<T>(string query)
        {
            List<T> data = new List<T>();
            try
            {
                var conn = new SqlConnection(connString);
                CheckAndOpenConnection(conn);
                System.Diagnostics.Debug.WriteLine($"\nRunning Query : {query};\n");
                SqlCommand cmd = new SqlCommand(query);
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var dt = new DataTable();
                        dt.Load(dr);

                        CheckAndCloseConnection(conn);

                        foreach (DataRow row in dt.Rows)
                        {
                            T item = GetItem<T>(row);
                            data.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                System.Diagnostics.Debug.WriteLine($"\nException Occured in SqlConnectionManager-{System.Reflection.MethodBase.GetCurrentMethod().Name}\nMessage : {ex.Message}\nDetails : {ex} ");
            }
            return data;
        }
        public static T GetItem<T>(DataRow dr)
        {
            T obj = Activator.CreateInstance<T>();
            try
            {
                Type temp = typeof(T);

                foreach (DataColumn column in dr.Table.Columns)
                {
                    foreach (PropertyInfo pro in temp.GetProperties())
                    {
                        if (pro.Name.ToLower() == column.ColumnName.ToLower() && dr[column.ColumnName] != DBNull.Value)
                        {
                            System.Diagnostics.Debug.WriteLine($"\nAdding Property : {pro.Name.ToLower()} to {column.ColumnName.ToLower()}");
                            System.Diagnostics.Debug.WriteLine($"Property types : {pro.GetMethod?.ReturnParameter} to {dr[column.ColumnName].GetType()} values is : {dr[column.ColumnName]}\n");
                            if (dr[column.ColumnName].GetType() == typeof(Int16))
                                pro.SetValue(obj, (int?)Convert.ToInt32(dr[column.ColumnName]), null);
                            else
                                pro.SetValue(obj, dr[column.ColumnName], null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                System.Diagnostics.Debug.WriteLine($"\nException Occured in SqlConnectionManager-{System.Reflection.MethodBase.GetCurrentMethod().Name}\nMessage : {ex.Message}\nDetails : {ex} ");
            }
            return obj;
        }



        public static bool DeleteRecord(string query)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                        //  command.ExecuteNonQuery();
                        //  return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting post: {ex.Message}");
                return false;
            }


        }
    }
}

