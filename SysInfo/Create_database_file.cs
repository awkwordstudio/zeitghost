using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ADOX;
using ADODB;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace SysInfo
{
    public partial class Main_Form
    {
        private SqlCommand cmd = null;
        private string get_connectionString()
        {
            string str_Path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string subPath = str_Path.ToString() + @"\Documents\WindowsMonitor\";
            curFile = subPath + "Database1.mdf";
            string connetionString = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename =" + curFile + "; Integrated Security = True;MultipleActiveResultSets=true";

            return connetionString;
        }
        
        private static bool CheckDatabaseExists(string connectionString, string databaseName)
        {
            bool bRet = false;
            using (var connection = new SqlConnection(connectionString))
            {
                string qry = "select * from master.dbo.sysdatabases where name='" + databaseName+"'";
                using (var command = new SqlCommand(qry, connection))
                {
                    try
                    {
                        connection.Open();
                        SqlDataReader process_id = command.ExecuteReader();
                        if (process_id.Read())
                            bRet = true;
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        try
                        {
                            string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Documents\WindowsMonitor\WindowMonitorlog.txt";
                            File.AppendAllLines(path, new[] { "\n Database Checking error ------" + DateTime.Now.ToString() + "-" + ex.Message + "" });
                        }
                        catch (System.InvalidOperationException) { }
                    }
                }
            }
            return bRet;
        }
        private void create_db(string curFile,string log_file)
        {
            String str;
            SqlConnection myConn = new SqlConnection("Server=(LocalDB)\\MSSQLLocalDB;Integrated security=SSPI;database=master;MultipleActiveResultSets=true");
            if (CheckDatabaseExists("Server=(LocalDB)\\MSSQLLocalDB;Integrated security=SSPI;database=master;MultipleActiveResultSets=true", "WindowsMonitor"))
            {
                try
                {
                    string sqlCommandText = "DROP DATABASE WindowsMonitor";
                    SqlCommand sqlCommand = new SqlCommand(sqlCommandText, myConn);
                    myConn.Open();
                    sqlCommand.ExecuteNonQuery();
                    myConn.Close();
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    try
                    { 
                        string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Documents\WindowsMonitor\WindowMonitorlog.txt";
                        File.AppendAllLines(path, new[] { "\n old Database is Droped------" + DateTime.Now.ToString("h:mm:ss tt") + "-" + ex.Message + "" });
                    }
                    catch (System.InvalidOperationException) { }
                }
                finally
                {
                    if (myConn.State == ConnectionState.Open)
                    {
                        myConn.Close();
                    }
                }
            }
            try
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
                str = "CREATE DATABASE WindowsMonitor ON PRIMARY (NAME = Database1,FILENAME = '" + curFile + "',SIZE = 9MB, MAXSIZE = 200MB, FILEGROWTH = 10%) LOG ON (NAME = Database1log, FILENAME = '" + log_file + "', SIZE = 1MB, MAXSIZE = 5MB, FILEGROWTH = 10%)";
                SqlCommand create_db = new SqlCommand(str, myConn);

                myConn.Open();
                create_db.ExecuteNonQuery();
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Documents\WindowsMonitor\WindowMonitorlog.txt";
                    File.AppendAllLines(path, new[] { "\n Database File creation Error ------" + DateTime.Now.ToString() + "-" + ex.Message + "" });
                }
                catch (System.InvalidOperationException) { }
            }
            finally
            {
                myConn.Close();
            }
            return;
        }
        private static bool CheckTableExists(string connectionString, string table_name)
        {
            bool bRet = false;
            using (var connection = new SqlConnection(connectionString))
            {
                string qry = "select * from sys.tables where name='" + table_name + "'";
                using (var command = new SqlCommand(qry, connection))
                {
                    try
                    {
                        connection.Open();
                        SqlDataReader process_id = command.ExecuteReader();
                        if (process_id.Read())
                            bRet = true;
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        try
                        {
                            string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Documents\WindowsMonitor\WindowMonitorlog.txt";
                            File.AppendAllLines(path, new[] { "\n Table Checking error ------" + DateTime.Now.ToString() + "-" + ex.Message + "" });
                        }
                        catch (System.InvalidOperationException) { }

                    }
                }
            }
            return bRet;
        }
        
        private void cteate_table()
        {
            String create_table;
            string connectionString = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename =" + curFile + "; Integrated Security = True; MultipleActiveResultSets = True";
            SqlConnection m_dbConnection6 = new SqlConnection(connectionString);

            try
            {
                m_dbConnection6.Open();
                if (!CheckTableExists(connectionString, "application_details"))
                {
                    create_table = "CREATE TABLE [dbo].[application_details] ([Id]  INT IDENTITY (1, 1) NOT NULL,[process_id]  INT NULL,[process_name] VARCHAR(512) NULL,[start_time] DATETIME NULL,[end_time]   DATETIME NULL,[title]   VARCHAR(512) NULL,[location]   VARCHAR(512) NULL,PRIMARY KEY CLUSTERED([Id] ASC))";
                    SqlCommand create_table1 = new SqlCommand(create_table, m_dbConnection6);
                    create_table1.ExecuteNonQuery();
                }
                if (!CheckTableExists(connectionString, "application_session_details"))
                {
                    create_table = "CREATE TABLE [dbo].[application_session_details] ([Id]  INT IDENTITY (1, 1) NOT NULL,[app_detail_id] INT NULL,[title]   VARCHAR(512) NULL,[start_time] DATETIME NULL,[end_time]   DATETIME NULL,PRIMARY KEY CLUSTERED([Id] ASC));";
                    SqlCommand create_table1 = new SqlCommand(create_table, m_dbConnection6);
                    create_table1.ExecuteNonQuery();
                }
                if (!CheckTableExists(connectionString, "outlook"))
                {
                    create_table = "CREATE TABLE [dbo].[outlook] ([Id]  INT IDENTITY (1, 1) NOT NULL,[folder] VARCHAR(512) NULL,PRIMARY KEY CLUSTERED([Id] ASC));";
                    SqlCommand create_table1 = new SqlCommand(create_table, m_dbConnection6);
                    create_table1.ExecuteNonQuery();
                }
                if (!CheckTableExists(connectionString, "outlook_mail"))
                {
                    create_table = "CREATE TABLE [dbo].[outlook_mail] ([Id] INT IDENTITY (1, 1) NOT NULL,[folder_name] VARCHAR(512) NULL,[outlook_id] INT NULL,[parent_folder_name] VARCHAR(512) NULL,PRIMARY KEY CLUSTERED([Id] ASC));";
                    SqlCommand create_table1 = new SqlCommand(create_table, m_dbConnection6);
                    create_table1.ExecuteNonQuery();
                }
                if (!CheckTableExists(connectionString, "outlook_mail_draftbox"))
                {
                    create_table = "CREATE TABLE [dbo].[outlook_mail_draftbox] ([Id]  INT IDENTITY (1, 1) NOT NULL,[start_time]  DATETIME NULL,[send_time]   DATETIME NULL,[subject]   VARCHAR(512) NULL,[body] TEXT NULL,[state]   VARCHAR(512) NULL,[outlook_mail_id]  INT NULL, PRIMARY KEY CLUSTERED([Id] ASC));";
                    SqlCommand create_table1 = new SqlCommand(create_table, m_dbConnection6);
                    create_table1.ExecuteNonQuery();
                }
                if (!CheckTableExists(connectionString, "outlook_mail_inbox"))
                {
                    create_table = "CREATE TABLE [dbo].[outlook_mail_inbox] ([Id]  INT IDENTITY (1, 1) NOT NULL,[mail_box_id]  INT NULL,[received_time]    DATETIME NULL,[subject]   VARCHAR(512)  NULL,[body] TEXT NULL,[read_time]  DATETIME NULL,[reply_bool]   BIT NULL, [reply_all_bool]   BIT NULL, [reply_time]       DATETIME NULL, [outlook_mail_id]    INT NULL, [attachment]         VARCHAR(512)  NULL, [no_of_attachment] INT NULL, [parent_folder_name] VARCHAR(512)  NULL, [entry_id]           VARCHAR(1024) NULL, [unread_bool] BIT NULL, PRIMARY KEY CLUSTERED([Id] ASC));";
                    SqlCommand create_table1 = new SqlCommand(create_table, m_dbConnection6);
                    create_table1.ExecuteNonQuery();
                }
                if (!CheckTableExists(connectionString, "url_info"))
                {
                    create_table = "CREATE TABLE [dbo].[url_info] ([Id]   INT IDENTITY (1, 1) NOT NULL,[url]  VARCHAR(512) NULL,[start_time] DATETIME NULL,[end_time]   DATETIME NULL,PRIMARY KEY CLUSTERED([Id] ASC));";
                    SqlCommand create_table1 = new SqlCommand(create_table, m_dbConnection6);
                    create_table1.ExecuteNonQuery();
                }
                if (!CheckTableExists(connectionString, "social_media_urls"))
                {
                    create_table = "CREATE TABLE [dbo].[social_media_urls] ( [Id]  INT IDENTITY (1, 1) NOT NULL,[url] VARCHAR(512) NULL);";
                    SqlCommand create_table1 = new SqlCommand(create_table, m_dbConnection6);
                    create_table1.ExecuteNonQuery();
                    create_table = "insert into social_media_urls values('facebook'),('twitter');";
                    SqlCommand create_table10 = new SqlCommand(create_table, m_dbConnection6);
                    create_table10.ExecuteNonQuery();
                }
                
                if (!CheckTableExists(connectionString, "browsers"))
                {
                    create_table = "CREATE TABLE [dbo].[browsers] ([Id]     INT IDENTITY (1, 1) NOT NULL,[name] VARCHAR(512) NULL,PRIMARY KEY CLUSTERED([Id] ASC));";
                    SqlCommand create_table1 = new SqlCommand(create_table, m_dbConnection6);
                    create_table1.ExecuteNonQuery();
                    create_table = "insert into browsers values ('firefox'),('chrome'),('explorer');";
                    SqlCommand create_table11 = new SqlCommand(create_table, m_dbConnection6);
                    create_table11.ExecuteNonQuery();
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                try
                {
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Documents\WindowsMonitor\WindowMonitorlog.txt";
                    File.AppendAllLines(path, new[] { "\n Database Table Creation Error 1------" + DateTime.Now.ToString() + "-" + ex.Message + "" });
                }
                catch (System.InvalidOperationException) { }
            }
            catch (System.Exception ex)
            {
                try
                {
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Documents\WindowsMonitor\WindowMonitorlog.txt";
                    File.AppendAllLines(path, new[] { "\n Database Table Creation Error 2------" + DateTime.Now.ToString() + "-" + ex.Message + "" });
                }
                catch (System.InvalidOperationException) { }
            }
            finally
            {
                if (m_dbConnection6.State == ConnectionState.Open)
                {
                    m_dbConnection6.Close();
                }
            }
        }
    }
}
