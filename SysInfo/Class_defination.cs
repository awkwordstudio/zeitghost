using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.IO;
namespace SysInfo
{
    public partial class Custom_methods
    {
        public void write_log_file(string main_str, string loading_type, string Message)
        // Write error and exception log in file
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Documents\WindowsMonitor\WindowMonitorlog.txt";
                File.AppendAllLines(path, new[] { "\n" + main_str +"  "+ loading_type + " ------ " + DateTime.Now.ToString() + "-" + Message + "" });
            }
            catch (System.InvalidOperationException) { }
            catch (IOException) { }
        }

        //Geting Color on request
        public Color _get_color(string name)
        {
            Color n_color =new Color();
            if (name=="white")
                n_color= Color.FromArgb(255, 255, 255);
            else if(name == "grey")
                n_color= Color.FromArgb(245, 245, 245);
            else if (name == "mail")
                n_color = Color.FromArgb(91, 155, 213);
            else if (name == "meeting")
                n_color = Color.FromArgb(84, 130, 53);
            else if (name == "browser")
                n_color = Color.FromArgb(197, 90, 17);
            else if (name == "project")
                n_color = Color.FromArgb(173, 20, 87);
            else if (name == "user")
                n_color = Color.FromArgb(106, 27, 154);
            else if (name == "system")
                n_color = Color.FromArgb(249, 168, 37);
            else if (name == "grey")
                n_color = Color.FromArgb(245, 245, 245);
            return n_color;
        }

        // Accessing single data from query
        public string get_query_data(string qstr, string error_msg)
        {
            SqlConnection m_dbConnection1 = get_connectionString();
            string total_count1 = "0";
            try
            {
                if (m_dbConnection1.State == ConnectionState.Open)
                {
                    m_dbConnection1.Close();
                }
                m_dbConnection1.Open();
                cmd = new SqlCommand(qstr, m_dbConnection1);
                SqlDataReader process_id = cmd.ExecuteReader();
                if (process_id.Read())
                    total_count1 = process_id.GetInt32(0).ToString();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
               write_log_file(error_msg, "--- get_query_data error --", ex.Message);          // call method to write error log
            }
            finally
            {
                if (m_dbConnection1 != null)
                {
                    if (m_dbConnection1.State == ConnectionState.Open)
                    {
                        m_dbConnection1.Close();
                    }
                }
            }
            return total_count1;
        }

        // get List of data
        public List<string> get_query_data_list(string qstr, string error_msg)
        {
            List<string> Data_List = new List<string>();
            SqlConnection m_dbConnection1 = get_connectionString();
            try
            {
                if (m_dbConnection1.State == ConnectionState.Open)
                {
                    m_dbConnection1.Close();
                }
                m_dbConnection1.Open();
                cmd = new SqlCommand(qstr, m_dbConnection1);
                SqlDataReader process_id = cmd.ExecuteReader();
                for (int i = 0; i < process_id.FieldCount; i++)
                {
                    Data_List.Add(process_id.GetName(i));
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                write_log_file(error_msg, "--- get_query_data_list --", ex.Message);          // call method to write error log
            }
            finally
            {
                if (m_dbConnection1 != null)
                {
                    if (m_dbConnection1.State == ConnectionState.Open)
                    {
                        m_dbConnection1.Close();
                    }
                }
            }
            return Data_List;
        }

        public SqlDataReader get_SqlDataReader_obj(string qstr, string error_msg)
        //get SQl data reader object for query (not in use)
        {
            SqlConnection m_dbConnection1 = get_connectionString();
            Dictionary<string, string> table_column = new Dictionary<string, string>();
            SqlDataReader process_id =null;
            try
            {
                if (m_dbConnection1.State == ConnectionState.Open)
                {
                    m_dbConnection1.Close();
                }
                m_dbConnection1.Open();
                cmd = new SqlCommand(qstr, m_dbConnection1);
                process_id = cmd.ExecuteReader();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                write_log_file(error_msg, "--- get_SqlDataReader_obj --", ex.Message);          // call method to write error log
            }
            finally
            {
                if (m_dbConnection1 != null)
                {
                    if (m_dbConnection1.State == ConnectionState.Open)
                    {
                        m_dbConnection1.Close();
                    }
                }
            }
            return process_id;
        }

        // for Insert and Update  Query 
        public void execute_query(string query,string Log_name,string log_type)
        {
            SqlConnection m_dbConnection1 = get_connectionString();
            if (m_dbConnection1.State == ConnectionState.Open)
            {
                m_dbConnection1.Close();
            }
            m_dbConnection1.Open();
            try
            {
                cmd = new SqlCommand(query, m_dbConnection1);
                cmd.ExecuteNonQuery();                  //This line crashes
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                write_log_file(Log_name, log_type, ex.Message);          // call method to write error log
            }
            finally
            {
                if (m_dbConnection1 != null)
                {
                    if (m_dbConnection1.State == ConnectionState.Open)
                    {
                        m_dbConnection1.Close();
                    }
                }
            }
        }

        public string get_query_data_char(string qstr, string error_msg)
        {
            SqlConnection m_dbConnection1 = get_connectionString();
            string total_count1 = "0";
            try
            {
                if (m_dbConnection1.State == ConnectionState.Open)
                {
                    m_dbConnection1.Close();
                }
                m_dbConnection1.Open();
                cmd = new SqlCommand(qstr, m_dbConnection1);
                SqlDataReader process_id = cmd.ExecuteReader();
                if (process_id.Read())
                {
                    total_count1 = process_id.GetString(0);
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                write_log_file(error_msg, "--- get_query_data error --", ex.Message);          // call method to write error log
            }
            catch (Exception ex)
            {
                write_log_file(error_msg, "--- get_query_data error --", ex.Message);          // call method to write error log
            }
            finally
            {
                if (m_dbConnection1 != null)
                {
                    if (m_dbConnection1.State == ConnectionState.Open)
                    {
                        m_dbConnection1.Close();
                    }
                }
            }
            return total_count1;
        }
    }
}
