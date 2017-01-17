using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;

namespace SysInfo
{
    public partial class Main_Form
    {
        private void field_child_data(string t_range)
        {
            SqlConnection m_dbConnection1 = Custom_obj.get_connectionString();
            Color Color1 = Color.FromArgb(197, 90, 17);
            Color Color2 = Color.FromArgb(197, 200, 20);
            DateTime d1 = DateTime.Now;
            DateTime d2 = DateTime.Now;
            string start_date = "";
            string end_date = "";
            if (t_range == "Week")
            {
                d2 = d1.AddDays(-6);                    // get previous 6 days
            }
            if (t_range == "Month")
            {
                d2 = d1.AddDays(-29);                  // get last 30 days 
            }
            start_date = new DateTime(d2.Year, d2.Month, d2.Day, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
            end_date = new DateTime(d1.Year, d1.Month, d1.Day, 23, 59, 59).ToString("yyyy-MM-dd HH:mm:ss");

            if (Load_type == "mail")    // Graph 1
            {   ////////////// Time analysis 
                try
                {
                    foreach (var series in _grph_1.Series)
                    {
                        series.Points.Clear();
                    }
                    string qstr = "";
                    if (Load_type == "mail")
                        qstr = @"select r.email,count(r.email) from recipients r,outlook_mail_inbox omi where 
                                omi.received_time between '" + start_date + "' and '" + end_date + "' and omi.parent_folder ='Inbox' and omi.received_time is not null and omi.entry_id = r.mail_id group by r.email order by count(r.email) desc";

                    Console.WriteLine("Line Graph Query -: " + qstr);
                    if (qstr != "")
                    {
                        if (m_dbConnection1.State == System.Data.ConnectionState.Open)
                        {
                            m_dbConnection1.Close();
                        }
                        m_dbConnection1.Open();
                        SqlCommand cmd = new SqlCommand(qstr, m_dbConnection1);
                        SqlDataReader process_id = cmd.ExecuteReader();
                        //SqlDataReader process_id = Custom_obj.get_SqlDataReader_obj(qstr, "Load donut chart2 value" + loading_type + " ");
                        while (process_id.Read())
                        {
                            _grph_1.Series["Series1"].Points.AddXY(process_id.GetString(0), process_id.GetInt32(1));
                        }
                    }
                }
                catch (System.InvalidOperationException ex)
                {
                    Custom_obj.write_log_file("Load Child graph 4 ", Load_type + " --1 ", ex.Message);          // call method to write error log
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    Custom_obj.write_log_file("Load Child graph 4 ", Load_type + " --2 ", ex.Message);          // call method to write error log
                }
                catch (System.Exception ex)
                {
                    Custom_obj.write_log_file("Load Child graph 4 ", Load_type + " --3 ", ex.Message);          // call method to write error log
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

            if (Load_type == "mail")
            {   ////////////// recipient analysis graph 
                try
                {
                    foreach (var series in _grph_4.Series)
                    {
                        series.Points.Clear();
                    }
                    string qstr = "";
                    if (Load_type == "mail")
                        qstr = @"select r.email,count(r.email) from recipients r,outlook_mail_inbox omi where 
                                omi.received_time between '" + start_date + "' and '" + end_date + "' and omi.parent_folder ='Inbox' and omi.received_time is not null and omi.entry_id = r.mail_id group by r.email order by count(r.email) desc";

                    Console.WriteLine("Line Graph Query -: " + qstr);
                    if (qstr != "")
                    {
                        if (m_dbConnection1.State == System.Data.ConnectionState.Open)
                        {
                            m_dbConnection1.Close();
                        }
                        m_dbConnection1.Open();
                        SqlCommand cmd = new SqlCommand(qstr, m_dbConnection1);
                        SqlDataReader process_id = cmd.ExecuteReader();
                        //SqlDataReader process_id = Custom_obj.get_SqlDataReader_obj(qstr, "Load donut chart2 value" + loading_type + " ");
                        while (process_id.Read())
                        {
                            _grph_4.Series["Series1"].Points.AddXY(process_id.GetString(0), process_id.GetInt32(1));
                        }
                    }
                }
                catch (System.InvalidOperationException ex)
                {
                    Custom_obj.write_log_file("Load Child graph 4 ", Load_type + " --1 ", ex.Message);          // call method to write error log
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    Custom_obj.write_log_file("Load Child graph 4 ", Load_type + " --2 ", ex.Message);          // call method to write error log
                }
                catch (System.Exception ex)
                {
                    Custom_obj.write_log_file("Load Child graph 4 ", Load_type + " --3 ", ex.Message);          // call method to write error log
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

            if (Load_type == "mail" )
            {   ////////////// recipient analysis graph 
                try
                {
                    foreach (var series in _grph_6.Series)
                    {
                        series.Points.Clear();
                    }
                    string qstr = "";
                    if (Load_type == "mail")
                        qstr = @"select r.email,count(r.email) from recipients r,outlook_mail_inbox omi where 
                                omi.received_time between '" + start_date + "' and '" + end_date + "' and omi.parent_folder = 'Sent Items' and omi.received_time is not null and omi.entry_id = r.mail_id group by r.email order by count(r.email) desc";

                    Console.WriteLine("Line Graph Query -: " + qstr);
                    if (qstr != "")
                    {
                        if (m_dbConnection1.State == System.Data.ConnectionState.Open)
                        {
                            m_dbConnection1.Close();
                        }
                        m_dbConnection1.Open();
                        SqlCommand cmd = new SqlCommand(qstr, m_dbConnection1);
                        SqlDataReader process_id = cmd.ExecuteReader();
                        //SqlDataReader process_id = Custom_obj.get_SqlDataReader_obj(qstr, "Load donut chart2 value" + loading_type + " ");
                        while (process_id.Read())
                        {
                            _grph_6.Series["Series1"].Points.AddXY(process_id.GetString(0), process_id.GetInt32(1));
                        }
                    }
                }
                catch (System.InvalidOperationException ex)
                {
                    Custom_obj.write_log_file("Load Child graph 6 ", Load_type + " --1", ex.Message);          // call method to write error log
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    Custom_obj.write_log_file("Load Child graph 6 ", Load_type + " --2", ex.Message);          // call method to write error log
                }
                catch (System.Exception ex)
                {
                    Custom_obj.write_log_file("Load Child graph 6 ", Load_type + " --3", ex.Message);          // call method to write error log
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

            if (Load_type == "mail" || Load_type == "project")
            {   ////////////// outbox analysis graph (when you send emails and how many)
                try
                {
                    foreach (var series in _grpMostCommonAtt.Series)
                    {
                        series.Points.Clear();
                    }
                    string qstr = "";
                    if (Load_type == "mail")
                        
                        qstr = @"select r.email,count(r.email) from recipients r,outlook_mail_inbox omi where 
                                omi.received_time between '" + start_date + "' and '" + end_date + "' and omi.parent_folder = 'Sent Items' and omi.received_time is not null and omi.entry_id = r.mail_id group by r.email order by count(r.email) desc";

                    Console.WriteLine("Line Graph Query -: " + qstr);
                    if (qstr != "")
                    {
                        if (m_dbConnection1.State == System.Data.ConnectionState.Open)
                        {
                            m_dbConnection1.Close();
                        }
                        m_dbConnection1.Open();
                        SqlCommand cmd = new SqlCommand(qstr, m_dbConnection1);
                        SqlDataReader process_id = cmd.ExecuteReader();
                        //SqlDataReader process_id = Custom_obj.get_SqlDataReader_obj(qstr, "Load donut chart2 value" + loading_type + " ");
                        while (process_id.Read())
                        {
                            _grpMostCommonAtt.Series["time"].Points.AddXY(process_id.GetString(0), process_id.GetInt32(1));
                        }
                    }
                }
                catch (System.InvalidOperationException ex)
                {
                    Custom_obj.write_log_file("Load Line graph ", Load_type + " 1", ex.Message);          // call method to write error log
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    Custom_obj.write_log_file("Load Line graph ", Load_type + " 2", ex.Message);          // call method to write error log
                }
                catch (System.Exception ex)
                {
                    Custom_obj.write_log_file("Load Line graph ", Load_type + " 3", ex.Message);          // call method to write error log
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

        }
    }
}
