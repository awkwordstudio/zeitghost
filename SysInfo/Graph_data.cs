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
        private void load_graphData(string t_range,string loading_type)
        {
            //main page graph data
            Console.WriteLine("Loading Graph Data.................."+ loading_type);
            SqlConnection m_dbConnection1 = Custom_obj.get_connectionString();
            try
            {
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
                try
                {  //_lblDataH1
                    string qstr = "";
                    string text_val = "0";
                    if (loading_type == "mail")
                        qstr = @"SELECT CONVERT(varchar(10),count(*)) from outlook_mail_inbox where parent_folder_name = 'Inbox' and received_time between 
                            '" + start_date + "' and '" + end_date + "' ";

                    else if (loading_type == "calender")
                        qstr = @"SELECT CONVERT(varchar(10),count(*)) from calender where start_time is not null and start_time between '" + start_date + "' and '" + end_date + "' ";

                    else if (loading_type == "web")
                        qstr = @"SELECT  CONVERT(varchar(10),count(total.url)) FROM (select distinct (" + _returnURLStirng() + @") as url from Url_Info 
                                where start_time between '" + start_date + "' and '" + end_date + "' and len(url)>1) as total ";

                    if (qstr != "" )
                        text_val = Custom_obj.get_query_data_char(qstr, "Load Heading ONE value "+loading_type+" ");

                    _lblDataH1.Text = text_val;
                }
                catch (System.InvalidOperationException ex)
                {
                    Custom_obj.write_log_file("Load Heading1 ", loading_type, ex.Message);          // call method to write error log
                }
                Console.WriteLine("Heading 1 done");

                try
                {  //_lblDataH2
                    string qstr = "";
                    string text_val = "0.0";
                    if (loading_type == "mail")
                        qstr = @"SELECT " + _returnTimeString(3600,"ad") + @" from application_details ad where 
                                                            start_time is not null and end_time is not null and process_name='OUTLOOK'";

                    else if (loading_type == "calender")
                        qstr = @"SELECT " + _returnTimeString(3600,"c") + @" from calender c where 
                                                            start_time >'" + start_date + "' and end_time <'" + end_date + "' ";

                    else if (loading_type == "web")
                        qstr = @"SELECT  " + _returnTimeString(3600,"ui") + @" from Url_Info ui where 
                                                                    start_time between '" + start_date + "' and '" + end_date + "'";
                    
                    if (qstr != "")
                        text_val = Custom_obj.get_query_data_char(qstr, "Load Heading TWo value " + loading_type + " ");
                    
                    _lblDataH2.Text = text_val;
                }
                catch (System.InvalidOperationException ex)
                {
                    Custom_obj.write_log_file("Load Heading2 ", loading_type, ex.Message);          // call method to write error log
                }
                Console.WriteLine("Heading 2 done");

                try
                {   //_lblDataH3
                    string qstr = "";
                    string text_val = "0";
                    if (loading_type == "mail")
                        qstr = "SELECT CONVERT(varchar(10),count(*)) from outlook_mail_inbox where parent_folder_name = 'Sent' and received_time between '" + start_date + "' and '" + end_date + "' ";

                    else if (loading_type == "web")
                        qstr = @"SELECT  CONVERT(varchar(10),count(*)) from url_info where start_time is not null and end_time is not null and start_time between '" + start_date + "' and '" + end_date + "' ";

                    else if (loading_type == "calender")
                        qstr = @"SELECT  CONVERT(varchar(10),count(*)) from calender where start_time >CURRENT_TIMESTAMP";
                    if (qstr != "")
                        text_val = Custom_obj.get_query_data_char(qstr, "Load Heading THREE value " + loading_type + " ");
                    _lblDataH3.Text = text_val;
                }
                catch (System.InvalidOperationException ex)
                {
                    Custom_obj.write_log_file("Load Heading3 ", loading_type, ex.Message);          // call method to write error log
                }
                Console.WriteLine("Heading 3 done");

                try
                {   /////// Donut Chart No-: 1
                    string qstr = "";
                    foreach (var series1 in main_page_chart_1.Series)
                    {
                        series1.Points.Clear();
                    }
                    if (loading_type == "mail")
                        qstr = @"SELECT ," + _returnTimeString(60, "asd") + @" as period
                               from outlook_mail_inbox ad where asd.start_time is not null and asd.end_time is not null 
                                and asd.app_detail_id = ad.id and ad.process_name in (select name from browsers) and 
                                asd.start_time between '" + start_date + "' and '" + end_date + "'";
                    else if (loading_type == "calender")
                        qstr = @"SELECT MeetingStatus,count(MeetingStatus)
                                from calender where asd.start_time is not null and asd.end_time is not null 
                                and start_time between '" + start_date + "' and '" + end_date + "' group by MeetingStatus";
                    else if (loading_type == "web")
                        qstr = @"SELECT ad.process_name ," + _returnTimeString(60, "asd") + @" as period
                                from application_session_details asd,application_details ad where asd.start_time is not null and asd.end_time is not null 
                                and asd.app_detail_id = ad.id and ad.process_name in (select name from browsers) and 
                                asd.start_time between '" + start_date + "' and '" + end_date + "' group by ad.process_name";
                    Console.WriteLine("Donut 1 QQQQ-:  " + qstr);
                    if (qstr != "")
                    {
                        if (m_dbConnection1.State == ConnectionState.Open)
                        {
                            m_dbConnection1.Close();
                        }
                        m_dbConnection1.Open();
                        SqlCommand cmd = new SqlCommand(qstr, m_dbConnection1);
                        SqlDataReader process_id = cmd.ExecuteReader();

                        Dictionary<string, int> dictionary = new Dictionary<string, int>();
                        int sum_time = 0;
                        while (process_id.Read())
                        {
                            int current_time = Convert.ToInt32(process_id.GetValue(1));
                            dictionary.Add(process_id.GetValue(0).ToString(), Convert.ToInt32(process_id.GetValue(1)));
                            sum_time += current_time;
                        }

                        foreach (KeyValuePair<string, int> entry in dictionary)
                        {
                            double dic_val = ((entry.Value / Convert.ToDouble(sum_time)) * 100);
                            main_page_chart_1.Series["Website"].Points.AddXY(entry.Key, Convert.ToInt32(dic_val));
                        }
                    }
                }
                catch (System.InvalidOperationException ex)
                {
                    Custom_obj.write_log_file("Load Donut 1 ", loading_type+" 1", ex.Message);          // call method to write error log
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    Custom_obj.write_log_file("Load Donut 1 ", loading_type+" 2", ex.Message);          // call method to write error log
                }
                catch (System.Exception ex)
                {
                    Custom_obj.write_log_file("Load Donut 1 ", loading_type+" 3", ex.Message);          // call method to write error log
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
                Console.WriteLine("Donut Chart 1 Done....");

                try
                {   /////// Donut Chart No-: 2
                    string qstr = "";
                    /*if (loading_type == "mail")
                        qstr = "select count(*) type from outlook_mail_inbox";
                    else if (loading_type == "calender")
                        qstr = @"select ui.u_type,round(sum(ui.u_time)/60,0) as uti from from calender ui where ui.start_time between '" + start_date + "' and '" + end_date +
                            @"' and ui.start_time is not null and ui.end_time is not null group by ui.u_type";

                    else */if (loading_type == "web")
                        qstr = @"select tt.u_type,round(sum(tt.u_time)/60,0) as uti from (SELECT url,ISNULL(DateDiff(SECOND, ui.start_time, ui.end_time), 0.0) as u_time,
                                case when(select smu.id from social_media_urls smu where smu.url = (case when Substring(ui.url, 1, Charindex('.', ui.url) - 1) = 'www'
                                then Substring(ui.url, Charindex('.', ui.url) + 1, Charindex('.', Substring(ui.url, Charindex('.', ui.url) + 1, LEN(ui.url))) - 1) 
                                else Substring(ui.url, 1, Charindex('.', ui.url) - 1) end))> 0 then 'social' else 'other' end as u_type from url_info ui
                                where ui.start_time between '" + start_date + "' and '" + end_date + "' and ui.start_time is not null and ui.end_time is not null)  as tt group by tt.u_type";

                    Console.WriteLine("chart 2 QQQ=:-  " + qstr);
                    if (qstr != "")
                    {
                        if (m_dbConnection1.State == ConnectionState.Open)
                        {
                            m_dbConnection1.Close();
                        }
                        m_dbConnection1.Open();
                        SqlCommand cmd = new SqlCommand(qstr, m_dbConnection1);
                        SqlDataReader process_id = cmd.ExecuteReader();
                        //SqlDataReader process_id = Custom_obj.get_SqlDataReader_obj(qstr, "Load donut chart3 value" + loading_type + " ");
                        Dictionary<string, int> dictionary = new Dictionary<string, int>();
                        int sum_time = 0;
                        foreach (var series1 in main_page_chart_2.Series)
                        {
                            series1.Points.Clear();
                        }
                        while (process_id.Read())
                        {
                            int current_time = Convert.ToInt32(process_id.GetValue(1));
                            dictionary.Add(process_id.GetValue(0).ToString(), Convert.ToInt32(process_id.GetValue(1)));
                            sum_time += current_time;
                        }

                        foreach (KeyValuePair<string, int> entry in dictionary)
                        {
                            double dic_val = ((entry.Value / Convert.ToDouble(sum_time)) * 100);
                            main_page_chart_2.Series["website"].Points.AddXY(entry.Key.ToString(), Convert.ToInt32(dic_val));
                        }
                    }
                }
                catch (System.InvalidOperationException ex)
                {
                    Custom_obj.write_log_file("Load Donut 1 ", loading_type + " 1", ex.Message);          // call method to write error log
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    Custom_obj.write_log_file("Load Donut 2 ", loading_type + " 2", ex.Message);          // call method to write error log
                }
                catch (System.Exception ex)
                {
                    Custom_obj.write_log_file("Load Donut 3 ", loading_type + " 3", ex.Message);          // call method to write error log
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
                Console.WriteLine("Donut Chart 2 Done....");

                try
                {   /////// Donut Chart No-: 3
                    string qstr = "";
                    foreach (var series1 in main_page_chart_3.Series)
                    {
                        series1.Points.Clear();
                    }
                    /*if (loading_type == "mail")
                        qstr = @"SELECT ," + _returnTimeString(60, "asd") + @" as period
                               from outlook_mail_inbox ad where asd.start_time is not null and asd.end_time is not null 
                                and asd.app_detail_id = ad.id and ad.process_name in (select name from browsers) and 
                                asd.start_time between '" + start_date + "' and '" + end_date + "'";
                    else*/
                    if (loading_type == "calender")
                        qstr = @"SELECT MeetingStatus,count(MeetingStatus)
                                from calender where asd.start_time is not null and asd.end_time is not null 
                                and start_time between '" + start_date + "' and '" + end_date + "' group by MeetingStatus";

                    Console.WriteLine("Donut 3 QQQQ-:  " + qstr);
                    if (qstr != "")
                    {
                        if (m_dbConnection1.State == ConnectionState.Open)
                        {
                            m_dbConnection1.Close();
                        }
                        m_dbConnection1.Open();
                        SqlCommand cmd = new SqlCommand(qstr, m_dbConnection1);
                        SqlDataReader process_id = cmd.ExecuteReader();

                        Dictionary<string, int> dictionary = new Dictionary<string, int>();
                        int sum_time = 0;
                        while (process_id.Read())
                        {
                            int current_time = Convert.ToInt32(process_id.GetValue(1));
                            dictionary.Add(process_id.GetValue(0).ToString(), Convert.ToInt32(process_id.GetValue(1)));
                            sum_time += current_time;
                        }

                        foreach (KeyValuePair<string, int> entry in dictionary)
                        {
                            double dic_val = ((entry.Value / Convert.ToDouble(sum_time)) * 100);
                            main_page_chart_2.Series["Website"].Points.AddXY(entry.Key, Convert.ToInt32(dic_val));
                        }
                    }
                }
                catch (System.InvalidOperationException ex)
                {
                    Custom_obj.write_log_file("Load Donut 3 ", loading_type + " 1", ex.Message);          // call method to write error log
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    Custom_obj.write_log_file("Load Donut 3 ", loading_type + " 2", ex.Message);          // call method to write error log
                }
                catch (System.Exception ex)
                {
                    Custom_obj.write_log_file("Load Donut 3 ", loading_type + " 3", ex.Message);          // call method to write error log
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
                Console.WriteLine("Donut Chart 3 Done....");

                try
                {   /////// Donut Chart No-: 4
                    string qstr = "";
                    foreach (var series1 in main_page_chart_4.Series)
                    {
                        series1.Points.Clear();
                    }
                    /*if (loading_type == "mail")
                        qstr = @"SELECT ," + _returnTimeString(60, "asd") + @" as period
                               from outlook_mail_inbox ad where asd.start_time is not null and asd.end_time is not null 
                                and asd.app_detail_id = ad.id and ad.process_name in (select name from browsers) and 
                                asd.start_time between '" + start_date + "' and '" + end_date + "'";
                    else*/
                    if (loading_type == "calender")
                        qstr = @"SELECT MeetingStatus,count(MeetingStatus)
                                from calender where asd.start_time is not null and asd.end_time is not null 
                                and start_time between '" + start_date + "' and '" + end_date + "' group by MeetingStatus";

                    Console.WriteLine("Donut 4 QQQQ-:  " + qstr);
                    if (qstr != "")
                    {
                        if (m_dbConnection1.State == ConnectionState.Open)
                        {
                            m_dbConnection1.Close();
                        }
                        m_dbConnection1.Open();
                        SqlCommand cmd = new SqlCommand(qstr, m_dbConnection1);
                        SqlDataReader process_id = cmd.ExecuteReader();

                        Dictionary<string, int> dictionary = new Dictionary<string, int>();
                        int sum_time = 0;
                        while (process_id.Read())
                        {
                            int current_time = Convert.ToInt32(process_id.GetValue(1));
                            dictionary.Add(process_id.GetValue(0).ToString(), Convert.ToInt32(process_id.GetValue(1)));
                            sum_time += current_time;
                        }

                        foreach (KeyValuePair<string, int> entry in dictionary)
                        {
                            double dic_val = ((entry.Value / Convert.ToDouble(sum_time)) * 100);
                            main_page_chart_4.Series["Website"].Points.AddXY(entry.Key, Convert.ToInt32(dic_val));
                        }
                    }
                }
                catch (System.InvalidOperationException ex)
                {
                    Custom_obj.write_log_file("Load Donut 4 ", loading_type + " 1", ex.Message);          // call method to write error log
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    Custom_obj.write_log_file("Load Donut 4 ", loading_type + " 2", ex.Message);          // call method to write error log
                }
                catch (System.Exception ex)
                {
                    Custom_obj.write_log_file("Load Donut 4 ", loading_type + " 3", ex.Message);          // call method to write error log
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
                Console.WriteLine("Chart 4 Done....");

                try
                {   /////// Donut Chart No-: 5
                    string qstr = "";
                    foreach (var series1 in main_page_chart_5.Series)
                    {
                        series1.Points.Clear();
                    }
                    if (loading_type == "mail")
                        qstr = @"select count(*) from outlook_mail_inbox omi,calender cl where omi.received_time between cl.start_time and cl.end_time and omi.parent_folder_name='Sent Items' and cl.start_time is not null and cl.end_time is not null 
                                and omi.received_time between '" + start_date + "' and '" + end_date + "'";
                    else
                    if (loading_type == "calender")
                        qstr = @"SELECT MeetingStatus,count(MeetingStatus)
                                from calender where asd.start_time is not null and asd.end_time is not null 
                                and start_time between '" + start_date + "' and '" + end_date + "' group by MeetingStatus";

                    Console.WriteLine("Donut 5 QQQQ-:  " + qstr);
                    if (qstr != "")
                    {
                        if (m_dbConnection1.State == ConnectionState.Open)
                        {
                            m_dbConnection1.Close();
                        }
                        m_dbConnection1.Open();
                        SqlCommand cmd = new SqlCommand(qstr, m_dbConnection1);
                        SqlDataReader process_id = cmd.ExecuteReader();

                        Dictionary<string, int> dictionary = new Dictionary<string, int>();
                        int sum_time = 0;
                        while (process_id.Read())
                        {
                            int current_time = Convert.ToInt32(process_id.GetValue(1));
                            dictionary.Add(process_id.GetValue(0).ToString(), Convert.ToInt32(process_id.GetValue(1)));
                            sum_time += current_time;
                        }

                        foreach (KeyValuePair<string, int> entry in dictionary)
                        {
                            double dic_val = ((entry.Value / Convert.ToDouble(sum_time)) * 100);
                            main_page_chart_5.Series["Website"].Points.AddXY(entry.Key, Convert.ToInt32(dic_val));
                        }
                    }
                }
                catch (System.InvalidOperationException ex)
                {
                    Custom_obj.write_log_file("Load Donut 5 ", loading_type + " 1", ex.Message);          // call method to write error log
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    Custom_obj.write_log_file("Load Donut 5 ", loading_type + " 2", ex.Message);          // call method to write error log
                }
                catch (System.Exception ex)
                {
                    Custom_obj.write_log_file("Load Donut 5 ", loading_type + " 3", ex.Message);          // call method to write error log
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
                Console.WriteLine("donut Chart 5 Done....");

                try
                {   /////// Donut Chart No-: 6
                    string qstr = "";
                    foreach (var series1 in main_page_chart_5.Series)
                    {
                        series1.Points.Clear();
                    }
                    if (loading_type == "mail")
                    {
                        string internal_domain = "gmail.com";
                        qstr = @"select count(*) from outlook_mail_inbox omi where omi.parent_folder_name='Sent Items' and entry_id in (select mail_id from recipients where email not like'%"+internal_domain+"%') and omi.received_time between '" + start_date + "' and '" + end_date + "'";
                    }
                    else
                    if (loading_type == "calender")
                        qstr = @"SELECT MeetingStatus,count(MeetingStatus)
                                from calender where asd.start_time is not null and asd.end_time is not null 
                                and start_time between '" + start_date + "' and '" + end_date + "' group by MeetingStatus";

                    if (qstr != "")
                    {
                        if (m_dbConnection1.State == ConnectionState.Open)
                        {
                            m_dbConnection1.Close();
                        }
                        m_dbConnection1.Open();
                        SqlCommand cmd = new SqlCommand(qstr, m_dbConnection1);
                        SqlDataReader process_id = cmd.ExecuteReader();

                        Dictionary<string, int> dictionary = new Dictionary<string, int>();
                        int sum_time = 0;
                        while (process_id.Read())
                        {
                            int current_time = Convert.ToInt32(process_id.GetValue(1));
                            dictionary.Add(process_id.GetValue(0).ToString(), Convert.ToInt32(process_id.GetValue(1)));
                            sum_time += current_time;
                        }

                        foreach (KeyValuePair<string, int> entry in dictionary)
                        {
                            double dic_val = ((entry.Value / Convert.ToDouble(sum_time)) * 100);
                            main_page_chart_5.Series["Website"].Points.AddXY(entry.Key, Convert.ToInt32(dic_val));
                        }
                    }
                }
                catch (System.InvalidOperationException ex)
                {
                    Custom_obj.write_log_file("Load Donut 6 ", loading_type + " 1", ex.Message);          // call method to write error log
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    Custom_obj.write_log_file("Load Donut 6 ", loading_type + " 2", ex.Message);          // call method to write error log
                }
                catch (System.Exception ex)
                {
                    Custom_obj.write_log_file("Load Donut 6 ", loading_type + " 3", ex.Message);          // call method to write error log
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
                Console.WriteLine("donut Chart 6 Done....");



                if (loading_type == "web" || loading_type == "project")
                {   ////////////// Line graph 
                    try
                    { 
                        foreach (var series in _grpMostCommonAtt.Series)
                        {
                            series.Points.Clear();
                        }
                        string qstr = "";
                        if (loading_type == "web")
                            qstr = @"SELECT  top 6 total.url,round(sum(total.time_diff)/60,2) FROM 
                                (select case when Substring(url, 1,Charindex('.', url)-1)='www' then
                                Substring(url, Charindex('.', url)+1,Charindex('.', Substring(url, Charindex('.', url)+1, LEN(url)))-1) else
                                Substring(url, 1,Charindex('.', url)-1) end as url,DateDiff(SECOND,start_time,end_time) AS time_diff from Url_Info where 
                                start_time between '" + start_date + "' and '" + end_date + "' and start_time is not null and len(url)>1) as total group by total.url order by sum(total.time_diff) desc";

                        Console.WriteLine("Line Graph Query -: " + qstr);
                        if (qstr != "")
                        {
                            if (m_dbConnection1.State == ConnectionState.Open)
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
                        Custom_obj.write_log_file("Load Line graph ", loading_type + " 1", ex.Message);          // call method to write error log
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        Custom_obj.write_log_file("Load Line graph ", loading_type + " 2", ex.Message);          // call method to write error log
                    }
                    catch (System.Exception ex)
                    {
                        Custom_obj.write_log_file("Load Line graph ", loading_type + " 3", ex.Message);          // call method to write error log
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
                Console.WriteLine("Line Graph Done....");

                try
                {   // time line Graph heading
                    try
                    { //Heading ONE 
                        string total_hrs_H1 = "";
                        int total_min_H1 = 0;
                        string qstr = "";
                        if (loading_type == "mail")              /// Query to count hours and minutes spend on mail 
                            qstr = @"SELECT " + _returnTimeString(3600, "ui") + "," + _returnTimeString(60, "ui") + @" from outlook_mail_draftbox ui where 
                                send_time between '" + start_date + "' and '" + end_date + @"' ";
                        else if (loading_type == "calender")              /// Query to count hours and minutes spend on calender 
                            qstr = @"SELECT " + _returnTimeString(3600, "ui") + "," + _returnTimeString(60, "ui") + @" from calender ui where start_time 
                                    between '" + start_date + "' and '" + end_date + @"' ";
                        else if (loading_type == "web")              /// Query to count hours and minutes spend on web 
                            qstr = @"SELECT " + _returnTimeString(3600, "ui") + "," + _returnTimeString(60, "ui") + @" from Url_Info ui where start_time 
                                    between '" + start_date + "' and '" + end_date + @"' and (case when Substring(url, 1,Charindex('.', url)-1)='www' then 
                                    Substring(url, Charindex('.', url)+1,Charindex('.', Substring(url, Charindex('.', url)+1, LEN(url)))-1) else 
                                    Substring(url, 1,Charindex('.', url)-1) end) in (select url from social_media_urls) and len(url)>1";

                        Console.WriteLine("TIME Line Graph Query:- " + qstr);
                        if (qstr != "")
                        {
                            try
                            {
                                if (m_dbConnection1.State == ConnectionState.Open)
                                {
                                    m_dbConnection1.Close();
                                }
                                m_dbConnection1.Open();
                                SqlCommand cmd = new SqlCommand(qstr, m_dbConnection1);
                                SqlDataReader process_id = cmd.ExecuteReader();
                                if (process_id.Read())
                                {
                                    total_hrs_H1 = process_id.GetValue(0).ToString();
                                    total_min_H1 = Convert.ToInt32(Convert.ToDouble(process_id.GetValue(1)));
                                }
                            }
                            catch (System.InvalidOperationException ex)
                            {
                                Custom_obj.write_log_file("Time line heading 1.1 ", loading_type + " 1", ex.Message);          // call method to write error log
                            }
                            catch (System.Data.SqlClient.SqlException ex)
                            {
                                Custom_obj.write_log_file("Time line heading 1.1 ", loading_type + " 2", ex.Message);          // call method to write error log
                            }
                            catch (System.Exception ex)
                            {
                                Custom_obj.write_log_file("Time line heading 1.1 ", loading_type + " 3", ex.Message);          // call method to write error log
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

                        string total_count = "";
                        qstr = "";
                        if (loading_type == "web")          /// Query to count nno of visits
                            qstr = @"select count(url) from url_info where (case when Substring(url, 1,Charindex('.', url)-1)='www' then 
                        Substring(url, Charindex('.', url)+1,Charindex('.', Substring(url, Charindex('.', url)+1, LEN(url)))-1) else 
                        Substring(url, 1,Charindex('.', url)-1) end) in (select url from social_media_urls) and len(url)>1 and start_time between '" + start_date + "' and '" + end_date + "'";
                        try
                        {
                            if (m_dbConnection1.State == ConnectionState.Open)
                            {
                                m_dbConnection1.Close();
                            }
                            m_dbConnection1.Open();
                            SqlCommand cmd = new SqlCommand(qstr, m_dbConnection1);
                            SqlDataReader process_id = cmd.ExecuteReader();
                            //process_id = Custom_obj.get_SqlDataReader_obj(qstr, "Time line Graph heading value" + loading_type + " ");
                            if (process_id.Read())
                                total_count = process_id.GetValue(0).ToString();
                        }
                        catch (System.InvalidOperationException ex)
                        {
                            Custom_obj.write_log_file("Time line heading 1.2 ", loading_type + " 1", ex.Message);          // call method to write error log
                        }
                        catch (System.Data.SqlClient.SqlException ex)
                        {
                            Custom_obj.write_log_file("Time line heading 1.2 ", loading_type + " 2", ex.Message);          // call method to write error log
                        }
                        catch (System.Exception ex)
                        {
                            Custom_obj.write_log_file("Time line heading 1.2 ", loading_type + " 3", ex.Message);          // call method to write error log
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
                        string total_visit = total_count == "0" ? "1" : total_count;
                        _lblweb1.ForeColor = Color1;
                        string _H1 = "";
                        if (loading_type == "mail")
                            _H1 = total_hrs_H1 + " hours total spent on drafting emails\n average " + (total_min_H1 / Convert.ToInt32(total_visit)).ToString() + " mins per email";

                        else if (loading_type == "calender")
                            _H1 = total_hrs_H1 + " hours total spent in team meeting\n  average " + (total_min_H1 / Convert.ToInt32(total_visit)).ToString() + " mins per meeting";

                        else if (loading_type == "web")
                            _H1 = total_hrs_H1 + " hours total spent on social media\n" + total_visit + " visits, average " + (total_min_H1 / Convert.ToInt32(total_visit)).ToString() + " mins per visit";

                        else if (loading_type == "system")
                            _H1 = total_hrs_H1 + " hours total spent on Messaging\n average " + (total_min_H1 / Convert.ToInt32(total_visit)).ToString() + " mins per message";

                        _lblweb1.Text = _H1;
                    }
                    catch (System.InvalidOperationException ex)
                    {
                        Custom_obj.write_log_file("Time line heading 1 ", loading_type + " 1", ex.Message);          // call method to write error log
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        Custom_obj.write_log_file("Time line heading 1 ", loading_type + " 2", ex.Message);          // call method to write error log
                    }
                    catch (System.Exception ex)
                    {
                        Custom_obj.write_log_file("Time line heading 1 ", loading_type + " 3", ex.Message);          // call method to write error log
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

                    try
                    {
                        string qstr = "";
                        if (loading_type == "web")
                            qstr = @"SELECT " + _returnTimeString(3600, "ui") + @"," + _returnTimeString(60, "ui") + @" 
                            from Url_Info ui where start_time between '" + start_date + "' and '" + end_date + @"' and
                            (case when Substring(url, 1,Charindex('.', url)-1)='www' then Substring(url, Charindex('.', url)+1,
                            Charindex('.', Substring(url, Charindex('.', url)+1, LEN(url)))-1) else Substring(url, 1,Charindex('.', url)-1) end) 
                            not in (select url from social_media_urls) and len(url)>1";

                        string total_hrs_H2 = "";
                        int total_min_H2 = 0;

                        try
                        {
                            //process_id = Custom_obj.get_SqlDataReader_obj(qstr, "Time line Graph heading value" + loading_type + " ");
                            if (m_dbConnection1.State == ConnectionState.Open)
                            {
                                m_dbConnection1.Close();
                            }
                            m_dbConnection1.Open();
                            SqlCommand cmd = new SqlCommand(qstr, m_dbConnection1);
                            SqlDataReader process_id = cmd.ExecuteReader();
                            if (process_id.Read())
                            {
                                total_hrs_H2 = process_id.GetValue(0).ToString();
                                total_min_H2 = Convert.ToInt32(Convert.ToDouble(process_id.GetValue(1)));
                            }
                        }
                        catch (System.InvalidOperationException ex)
                        {
                            Custom_obj.write_log_file("Time line heading 3 ", loading_type + " 1", ex.Message);          // call method to write error log
                        }
                        catch (System.Data.SqlClient.SqlException ex)
                        {
                            Custom_obj.write_log_file("Time line heading 3 ", loading_type + " 2", ex.Message);          // call method to write error log
                        }
                        catch (System.Exception ex)
                        {
                            Custom_obj.write_log_file("Time line heading 3 ", loading_type + " 3", ex.Message);          // call method to write error log
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
                        Console.WriteLine("XPPPPPPPPPPPPPPPPPPPPPPPPPPPP");
                        qstr = "";
                        if (loading_type == "web")
                            qstr = @"select count(url) from url_info where (case when Substring(url, 1,Charindex('.', url)-1)='www' then 
                        Substring(url, Charindex('.', url)+1,Charindex('.', Substring(url, Charindex('.', url)+1, LEN(url)))-1) else 
                        Substring(url, 1,Charindex('.', url)-1) end) not in (select url from social_media_urls) and len(url)>1 and 
                        start_time between '" + start_date + "' and '" + end_date + "'";
                        string total_count = "";
                        try
                        {
                            if (m_dbConnection1.State == ConnectionState.Open)
                            {
                                m_dbConnection1.Close();
                            }
                            m_dbConnection1.Open();
                            SqlCommand cmd = new SqlCommand(qstr, m_dbConnection1);
                            SqlDataReader process_id = cmd.ExecuteReader();
                            if (process_id.Read())
                                total_count = process_id.GetValue(0).ToString();
                            //total_visits_nsm = Custom_obj.get_query_data(qstr, "Time line Graph heading value" + loading_type + " ");
                        }
                        catch (System.InvalidOperationException ex)
                        {
                            Custom_obj.write_log_file("Time line heading 4 ", loading_type + " 1", ex.Message);          // call method to write error log
                        }
                        catch (System.Data.SqlClient.SqlException ex)
                        {
                            Custom_obj.write_log_file("Time line heading 4 ", loading_type + " 2", ex.Message);          // call method to write error log
                        }
                        catch (System.Exception ex)
                        {
                            Custom_obj.write_log_file("Time line heading 4 ", loading_type + " 3", ex.Message);          // call method to write error log
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

                        string total_visit = total_count == "0" ? "1" : total_count;
                        _lblweb2.ForeColor = Color2;
                        string _H2 = "";
                        if (loading_type == "mail")
                            _H2 = total_hrs_H2 + " hours total spent on reading emails\n average " + (total_min_H2 / Convert.ToInt32(total_visit)).ToString() + " mins per email";

                        else if (loading_type == "calender")
                            _H2 = total_hrs_H2 + " hours total spent in non-team meeting\n  average " + (total_min_H2 / Convert.ToInt32(total_visit)).ToString() + " mins per meeting";

                        else if (loading_type == "web")
                            _H2 = total_hrs_H2 + " hours total spent on non-social media\n" + total_visit + " visits, average " + (total_min_H2 / Convert.ToInt32(total_visit)).ToString() + " mins per visit";

                        else if (loading_type == "system")
                            _H2 = total_hrs_H2 + " hours total spent on call\n average " + (total_min_H2 / Convert.ToInt32(total_visit)).ToString() + " mins per call";


                        _lblweb2.Text = _H2;
                    }
                    catch (System.InvalidOperationException ex)
                    {
                        Custom_obj.write_log_file("TimeLine Heading 2.2", loading_type + " 1", ex.Message);          // call method to write error log
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        Custom_obj.write_log_file("TimeLine Heading 2.2", loading_type + " 2", ex.Message);          // call method to write error log
                    }
                    catch (System.Exception ex)
                    {
                        Custom_obj.write_log_file("TimeLine Heading 2.2", loading_type + " 3", ex.Message);          // call method to write error log
                    }
                }
                catch (System.InvalidOperationException ex)
                {
                    Custom_obj.write_log_file("TimeLine Heading ", loading_type + " 1", ex.Message);          // call method to write error log
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    Custom_obj.write_log_file("TimeLine Heading ", loading_type + " 2", ex.Message);          // call method to write error log
                }
                catch (System.Exception ex)
                {
                    Custom_obj.write_log_file("TimeLine Heading ", loading_type + " 3", ex.Message);          // call method to write error log
                }

                try
                {
                    /// TIME CHART GRAPH..
                    main_page_chart_0.Series.Clear();
                    System.Windows.Forms.DataVisualization.Charting.ChartArea CA = main_page_chart_0.ChartAreas[0];
                    main_page_chart_0.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                    main_page_chart_0.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                    //chart4.ChartAreas[0].AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
                    // chart4.ChartAreas[0].AxisX.Title = "Hour";
                    //chart4.ChartAreas[0].AxisY.Title = "Minutes";
                    main_page_chart_0.ChartAreas[0].AxisX.Interval = 1;
                    main_page_chart_0.ChartAreas[0].AxisX.IsLabelAutoFit = false;
                    main_page_chart_0.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
                    // our only Series
                    System.Windows.Forms.DataVisualization.Charting.Series agentSeries = main_page_chart_0.Series.Add(" ");
                    agentSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.RangeColumn;
                    agentSeries.Color = System.Drawing.Color.Transparent;  // hide the default series entry!
                    agentSeries["PixelPointWidth"] = "10";
                    int index = 0;
                    int index1 = 0;

                    string str1 = "select url from social_media_urls ";
                    if (m_dbConnection1.State == ConnectionState.Open)
                    {
                        m_dbConnection1.Close();
                    }
                    m_dbConnection1.Open();
                    SqlCommand cmd = new SqlCommand(str1, m_dbConnection1);
                    SqlDataReader process_id = cmd.ExecuteReader();
                    //SqlDataReader process_id = Custom_obj.get_SqlDataReader_obj(str1, "Time line Graph heading value" + loading_type + " ");
                    string[] urls = new string[40];
                    while (process_id.Read())
                    {
                        urls[index1] = process_id.GetValue(0).ToString();
                        index1++;
                    }
                    if (m_dbConnection1 != null)
                    {
                        if (m_dbConnection1.State == ConnectionState.Open)
                        {
                            m_dbConnection1.Close();
                        }
                    }
                    List<int> list = new List<int>();
                    int val = 0;
                    if (t_range == "day")
                    {
                        val = 24;
                    }
                    if (t_range == "Week")
                    {
                        main_page_chart_0.ChartAreas[0].AxisX.LabelStyle.Angle = 0;
                        val = 7;
                        agentSeries["PixelPointWidth"] = "25"; // <- your choice of width!
                    }
                    if (t_range == "Month")
                        val = 30;
                    for (int i = 0; i < val; i++)
                    {
                        list.Add(i);
                    }
                    int[] _agents1 = list.ToArray();

                    foreach (int a in _agents1)
                    {
                        string str = "";
                        if (t_range == "day")
                        {
                            start_date = new DateTime(d1.Year, d1.Month, d1.Day, a, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                            end_date = new DateTime(d1.Year, d1.Month, d1.Day, a, 59, 59).ToString("yyyy-MM-dd HH:mm:ss");
                            
                            if (loading_type == "web")
                                str = @"select DATEPART(minute, start_time),DATEPART(minute, end_time),case when Substring(url, 1,Charindex('.', url)-1)='www' then 
                                       Substring(url, Charindex('.', url)+1,Charindex('.', Substring(url, Charindex('.', url)+1, LEN(url)))-1) else Substring(url, 1,Charindex('.', url)-1) end,
                                        DATEPART(hour, start_time),DATEPART(hour, end_time) from url_info where start_time is not null and end_time is not null and len(url)>1 and 
                                        start_time between '" + start_date + "' and '" + end_date + "'";
                        }
                        else if (t_range == "Week" || t_range == "Month")
                        {
                            start_date = new DateTime(d2.Year, d2.Month, d2.Day, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                            end_date = new DateTime(d1.Year, d1.Month, d1.Day, 23, 59, 59).ToString("yyyy-MM-dd HH:mm:ss");
                            if (loading_type == "web")
                                str = @"select DATEPART(hour, start_time),DATEPART(hour, end_time),case when Substring(url, 1,Charindex('.', url)-1)='www' then
                                    Substring(url, Charindex('.', url)+1,Charindex('.', Substring(url, Charindex('.', url)+1, LEN(url)))-1) else Substring(url, 1,Charindex('.', url)-1) end,
                                    DATEPART(day, start_time),DATEPART(day, end_time) from url_info where start_time is not null and 
                                    end_time is not null and len(url)>1 and start_time between '" + start_date + "' and '" + end_date + "'";
                        }
                        if (m_dbConnection1.State == ConnectionState.Open)
                        {
                            m_dbConnection1.Close();
                        }
                        m_dbConnection1.Open();
                        cmd = new SqlCommand(str, m_dbConnection1);
                        process_id = cmd.ExecuteReader();
                        //process_id = Custom_obj.get_SqlDataReader_obj(str, "Time line Graph heading value" + loading_type + " ");
                        Boolean bool1 = false;
                        while (process_id.Read())
                        {
                            string t1 = process_id.GetValue(0).ToString();
                            string t2 = process_id.GetValue(1).ToString();
                            string t3 = process_id.GetValue(2).ToString();
                            string t4 = process_id.GetValue(3).ToString();
                            string t5 = process_id.GetValue(4).ToString();

                            System.Drawing.Color color = urls.Contains(t3) ? Color1 : Color2;
                            bool1 = true;
                            int p;
                            if (t_range == "Week" || t_range == "Month")
                            {
                                if (t4 != t5)
                                {
                                    string t2_new = t2;
                                    int index11 = index;
                                    for (int i = Convert.ToInt32(t4); i <= Convert.ToInt32(t5); i++)
                                    {
                                        if (i != Convert.ToInt32(t5) && i != Convert.ToInt32(t4))
                                        {
                                            t1 = "0";
                                            t2 = "24";
                                        }
                                        else if (i != Convert.ToInt32(t5))
                                        {
                                            t2 = "24";
                                        }
                                        else
                                        {
                                            t1 = "0";
                                            t2 = t2_new;
                                        }
                                        p = agentSeries.Points.AddXY(index11, t1, t2);
                                        agentSeries.Points[p].Color = color;
                                        index11++;
                                    }
                                }
                                p = agentSeries.Points.AddXY(index, t1, t2); //d2.ToString("ddd")
                            }
                            else
                            {
                                if (t4 != t5)
                                {
                                    string t2_new = t2;
                                    int index11 = index;
                                    for (int i = Convert.ToInt32(t4); i <= Convert.ToInt32(t5); i++)
                                    {
                                        if (i != Convert.ToInt32(t5) && i != Convert.ToInt32(t4))
                                        {
                                            t1 = "0";
                                            t2 = "60";
                                        }
                                        else if (i != Convert.ToInt32(t5))
                                        {
                                            t2 = "60";
                                        }
                                        else
                                        {
                                            t1 = "0";
                                            t2 = t2_new;
                                        }
                                        p = agentSeries.Points.AddXY(index11, t1, t2);
                                        agentSeries.Points[p].Color = color;
                                        index11++;
                                    }
                                }
                                else
                                {
                                    p = agentSeries.Points.AddXY(index, t1, t2);
                                    agentSeries.Points[p].Color = color;
                                }
                            }
                        }
                        if (bool1 == false)
                        {
                            if (t_range == "Week" || t_range == "Month")
                            {
                                agentSeries.Points.AddXY(index, 0, 0);
                            }
                            else
                                agentSeries.Points.AddXY(index, 0, 0);
                        }
                        index++;
                        d2 = d2.AddDays(1);
                    }
                }
                catch (System.InvalidOperationException ex)
                {
                    Custom_obj.write_log_file("Load TimeLine graph ", loading_type + " 1", ex.Message);          // call method to write error log
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    Custom_obj.write_log_file("Load TimeLine graph ", loading_type + " 2", ex.Message);          // call method to write error log
                }
                catch (System.Exception ex)
                {
                    Custom_obj.write_log_file("Load TimeLine graph ", loading_type + " 3", ex.Message);          // call method to write error log
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
            catch (System.InvalidOperationException ex)
            {
                Custom_obj.write_log_file("Loading graph Page ", loading_type + " 1", ex.Message);          // call method to write error log
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Custom_obj.write_log_file("Loading graph Page ", loading_type + " 2", ex.Message);          // call method to write error log
            }
            catch (System.Exception ex)
            {
                Custom_obj.write_log_file("Loading graph Page ", loading_type + " 3", ex.Message);          // call method to write error log
            }
        }
        public void Graph_load()
        {
            //label10.Text += "↑";

           /* chart7.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
            chart7.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
            chart7.ChartAreas["ChartArea1"].AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chart7.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            chart7.ChartAreas["ChartArea1"].AxisX.LabelStyle.Angle = -30;
            chart7.ChartAreas["ChartArea1"].AxisX.IsLabelAutoFit = false;
            chart7.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 7.5F);*/

            //ResponseTimeAnlysis
            _grph_2.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
            _grph_2.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
            _grph_2.ChartAreas["ChartArea1"].AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;

            _grph_2.Series["Website"].Points.AddXY("Total Time taken to Reply", 60);
            _grph_2.Series["Website"].Points.AddXY("Time taken to Draft", 60, 50);
            _grph_2.Series["Website"].Points.AddXY("lag Time", 50, 30);
            _grph_2.Series["Website"].Points.AddXY("Time taken to Read", 30, 25);
            _grph_2.Series["Website"].Points.AddXY("Time taken to Open", 0, 25);

            _grph_1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = true;
            _grph_1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
            _grph_1.ChartAreas["ChartArea1"].AxisY.Interval = 5;
            _grph_1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            _grph_1.ChartAreas["ChartArea1"].AxisY.Minimum = -30;
            _grph_1.ChartAreas["ChartArea1"].AxisY.Maximum = 100;
            _grph_1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Angle = -30;
            _grph_1.ChartAreas["ChartArea1"].AxisX.IsLabelAutoFit = false;


            _grph_1.Series["time1"].Points.AddXY("12 am", 5);
            _grph_1.Series["time1"].Points.AddXY("01 am", 10);
            _grph_1.Series["time1"].Points.AddXY("02 am", 15);
            _grph_1.Series["time1"].Points.AddXY("03 am", 20);
            _grph_1.Series["time1"].Points.AddXY("04 am", 25);
            _grph_1.Series["time1"].Points.AddXY("05 am", 30);
            _grph_1.Series["time1"].Points.AddXY("06 am", 30);
            _grph_1.Series["time1"].Points.AddXY("07 am", 40);
            _grph_1.Series["time1"].Points.AddXY("08 am", 40);
            _grph_1.Series["time1"].Points.AddXY("09 am", 50);
            _grph_1.Series["time1"].Points.AddXY("10 am", 45);
            _grph_1.Series["time1"].Points.AddXY("11 am", 40);
            _grph_1.Series["time1"].Points.AddXY("12 pm", 50);
            _grph_1.Series["time1"].Points.AddXY("01 pm", 45);
            _grph_1.Series["time1"].Points.AddXY("02 pm", 40);
            _grph_1.Series["time1"].Points.AddXY("03 pm", 36);
            _grph_1.Series["time1"].Points.AddXY("04 pm", 35);
            _grph_1.Series["time1"].Points.AddXY("05 pm", 40);
            _grph_1.Series["time1"].Points.AddXY("06 pm", 50);
            _grph_1.Series["time1"].Points.AddXY("07 pm", 30);
            _grph_1.Series["time1"].Points.AddXY("08 pm", 50);
            _grph_1.Series["time1"].Points.AddXY("09 pm", 50);
            _grph_1.Series["time1"].Points.AddXY("10 pm", 45);
            _grph_1.Series["time1"].Points.AddXY("11 pm", 50);

            _grph_1.Series["time2"].Points.AddXY("12 am", -1);
            _grph_1.Series["time2"].Points.AddXY("01 am", -1);
            _grph_1.Series["time2"].Points.AddXY("02 am", -1);
            _grph_1.Series["time2"].Points.AddXY("03 am", -1);
            _grph_1.Series["time2"].Points.AddXY("04 am", -2);
            _grph_1.Series["time2"].Points.AddXY("05 am", -1);
            _grph_1.Series["time2"].Points.AddXY("06 am", -2);
            _grph_1.Series["time2"].Points.AddXY("07 am", -2);
            _grph_1.Series["time2"].Points.AddXY("08 am", -3);
            _grph_1.Series["time2"].Points.AddXY("09 am", -5);
            _grph_1.Series["time2"].Points.AddXY("10 am", -10);
            _grph_1.Series["time2"].Points.AddXY("11 am", -15);
            _grph_1.Series["time2"].Points.AddXY("12 pm", -15);
            _grph_1.Series["time2"].Points.AddXY("01 pm", -20);
            _grph_1.Series["time2"].Points.AddXY("02 pm", -15);
            _grph_1.Series["time2"].Points.AddXY("03 pm", -20);
            _grph_1.Series["time2"].Points.AddXY("04 pm", -15);
            _grph_1.Series["time2"].Points.AddXY("05 pm", -20);
            _grph_1.Series["time2"].Points.AddXY("06 pm", -10);
            _grph_1.Series["time2"].Points.AddXY("07 pm", -5);
            _grph_1.Series["time2"].Points.AddXY("08 pm", -5);
            _grph_1.Series["time2"].Points.AddXY("09 pm", -1);
            _grph_1.Series["time2"].Points.AddXY("10 pm", -1);
            _grph_1.Series["time2"].Points.AddXY("11 pm", -1);

            _grph_1.Series["time3"].Points.AddXY("12 am", 5);
            _grph_1.Series["time3"].Points.AddXY("01 am", 5);
            _grph_1.Series["time3"].Points.AddXY("02 am", 10);
            _grph_1.Series["time3"].Points.AddXY("03 am", 10);
            _grph_1.Series["time3"].Points.AddXY("04 am", 15);
            _grph_1.Series["time3"].Points.AddXY("05 am", 15);
            _grph_1.Series["time3"].Points.AddXY("06 am", 20);
            _grph_1.Series["time3"].Points.AddXY("07 am", 25);
            _grph_1.Series["time3"].Points.AddXY("08 am", 30);
            _grph_1.Series["time3"].Points.AddXY("09 am", 40);
            _grph_1.Series["time3"].Points.AddXY("10 am", 45);
            _grph_1.Series["time3"].Points.AddXY("11 am", 40);
            _grph_1.Series["time3"].Points.AddXY("12 pm", 50);
            _grph_1.Series["time3"].Points.AddXY("01 pm", 45);
            _grph_1.Series["time3"].Points.AddXY("02 pm", 45);
            _grph_1.Series["time3"].Points.AddXY("03 pm", 36);
            _grph_1.Series["time3"].Points.AddXY("04 pm", 55);
            _grph_1.Series["time3"].Points.AddXY("05 pm", 55);
            _grph_1.Series["time3"].Points.AddXY("06 pm", 50);
            _grph_1.Series["time3"].Points.AddXY("07 pm", 45);
            _grph_1.Series["time3"].Points.AddXY("08 pm", 50);
            _grph_1.Series["time3"].Points.AddXY("09 pm", 55);
            _grph_1.Series["time3"].Points.AddXY("10 pm", 60);
            _grph_1.Series["time3"].Points.AddXY("11 pm", 50);

            

            //chart4.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
            //chart4.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
            //chart4.ChartAreas["ChartArea1"].AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            //chart4.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            //chart4.ChartAreas["ChartArea1"].AxisX.LabelStyle.Angle = -30;
            //chart4.ChartAreas["ChartArea1"].AxisX.IsLabelAutoFit = false;
            //chart4.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new System.Drawing.Font("Times New Roman", 8.5F);



           /* chart5.Series["email"].Points.AddXY("6", 60);
            chart5.Series["email"].Points.AddXY("4 ", 40);

            chart6.Series["email"].Points.AddXY("8", 80);
            chart6.Series["email"].Points.AddXY("3", 30);
            */
            // email read and draft
           /* chart7.Series["draftMail"].Points.AddXY("12 am", 0);
            chart7.Series["draftMail"].Points.AddXY("01 am", 0);
            chart7.Series["draftMail"].Points.AddXY("02 am", 0);
            chart7.Series["draftMail"].Points.AddXY("03 am", 0);
            chart7.Series["draftMail"].Points.AddXY("04 am", 0);
            chart7.Series["draftMail"].Points.AddXY("05 am", 0);
            chart7.Series["draftMail"].Points.AddXY("06 am", 0);
            chart7.Series["draftMail"].Points.AddXY("07 am", 0);
            chart7.Series["draftMail"].Points.AddXY("08 am", 0);
            chart7.Series["draftMail"].Points.AddXY("09 am", 0);
            chart7.Series["draftMail"].Points.AddXY("10 am", 15);
            chart7.Series["draftMail"].Points.AddXY("11 am", 40);
            chart7.Series["draftMail"].Points.AddXY("12 pm", 50);
            chart7.Series["draftMail"].Points.AddXY("01 pm", 45);
            chart7.Series["draftMail"].Points.AddXY("02 pm", 25);
            chart7.Series["draftMail"].Points.AddXY("03 pm", 36);
            chart7.Series["draftMail"].Points.AddXY("04 pm", 15);
            chart7.Series["draftMail"].Points.AddXY("05 pm", 21);
            chart7.Series["draftMail"].Points.AddXY("06 pm", 50);
            chart7.Series["draftMail"].Points.AddXY("07 pm", 0);
            chart7.Series["draftMail"].Points.AddXY("08 pm", 0);
            chart7.Series["draftMail"].Points.AddXY("09 pm", 0);
            chart7.Series["draftMail"].Points.AddXY("10 pm", 0);
            chart7.Series["draftMail"].Points.AddXY("11 pm", 0);

            chart7.Series["readMail"].Points.AddXY("12 am", 0);
            chart7.Series["readMail"].Points.AddXY("01 am", 0);
            chart7.Series["readMail"].Points.AddXY("02 am", 0);
            chart7.Series["readMail"].Points.AddXY("03 am", 0);
            chart7.Series["readMail"].Points.AddXY("04 am", 0);
            chart7.Series["readMail"].Points.AddXY("05 am", 0);
            chart7.Series["readMail"].Points.AddXY("06 am", 0);
            chart7.Series["readMail"].Points.AddXY("07 am", 0);
            chart7.Series["readMail"].Points.AddXY("08 am", 0);
            chart7.Series["readMail"].Points.AddXY("09 am", 0);
            chart7.Series["readMail"].Points.AddXY("10 am", 17);
            chart7.Series["readMail"].Points.AddXY("11 am", 42);
            chart7.Series["readMail"].Points.AddXY("12 pm", 52);
            chart7.Series["readMail"].Points.AddXY("01 pm", 47);
            chart7.Series["readMail"].Points.AddXY("02 pm", 27);
            chart7.Series["readMail"].Points.AddXY("03 pm", 34);
            chart7.Series["readMail"].Points.AddXY("04 pm", 13);
            chart7.Series["readMail"].Points.AddXY("05 pm", 19);
            chart7.Series["readMail"].Points.AddXY("06 pm", 52);
            chart7.Series["readMail"].Points.AddXY("07 pm", 0);
            chart7.Series["readMail"].Points.AddXY("08 pm", 0);
            chart7.Series["readMail"].Points.AddXY("09 pm", 0);
            chart7.Series["readMail"].Points.AddXY("10 pm", 0);
            chart7.Series["readMail"].Points.AddXY("11 pm", 0);*/

            // for graph page pie chart 
            /*qstr = "SELECT  top 6 url,count(url) from outlook_mail_inbox where parent_folder_name = 'Sent' ";//and received_time between '" + start_date + "' and '" + end_date + "' ";
            total_count1 = "0";
            if (m_dbConnection1.State == ConnectionState.Open)
            {
                m_dbConnection1.Close();
            }
            m_dbConnection1.Open();
            cmd = new OleDbCommand(qstr, m_dbConnection1);
            process_id = cmd.ExecuteReader();
            while (process_id.Read())
            {
                _grphSenderAnlysis.Series["IncomingMail"].Points.AddXY(process_id.GetString(0), process_id.GetInt32(1));
            }
            m_dbConnection1.Close();*/
            _grph_3.Series["IncomingMail"].Points.AddXY("Harshal", 60);
            _grph_3.Series["IncomingMail"].Points.AddXY("Avinash", 50);
            _grph_3.Series["IncomingMail"].Points.AddXY("Mahesh", 30);
            _grph_3.Series["IncomingMail"].Points.AddXY("Sweta", 50);
            _grph_3.Series["IncomingMail"].Points.AddXY("Rahul", 10);
            _grph_3.Series["IncomingMail"].Points.AddXY("Mihir", 20);
            _grph_3.Series["IncomingMail"].Points.AddXY("other", 10);
        }
    }
}
