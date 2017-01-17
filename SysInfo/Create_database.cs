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
using System.Text.RegularExpressions;

namespace SysInfo
{
    public partial class Custom_methods
    {
        public SqlCommand cmd = null;

        public SqlConnection get_connectionString()
        {
            string str_Path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string subPath = str_Path.ToString() + @"\Documents\WindowsMonitor\";
            string curFile = subPath + "Database1.mdf";
            string connetionString = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename =" + curFile + "; Integrated Security = True;MultipleActiveResultSets=true";
            SqlConnection m_dbConnection1 = new SqlConnection(connetionString);
            return m_dbConnection1;
        }
        // Check for DataBase Existance
        private static bool CheckDatabaseExists(string connectionString, string databaseName)
        {
            bool bRet = false;
            using (var connection = new SqlConnection(connectionString))
            {
                Custom_methods cust_obj = new Custom_methods();
                string qry = "select * from master.dbo.sysdatabases where name='" + databaseName + "'";
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
                        cust_obj.write_log_file("Database Checking", "error ------", ex.Message);          // call method to write error log
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return bRet;
        }
        // Create Database 
        public void create_db(string curFile, string log_file)
        {
            String str;
            SqlConnection myConn = new SqlConnection("Server=(LocalDB)\\MSSQLLocalDB;Integrated security=SSPI;database=master;MultipleActiveResultSets=true");
            if (CheckDatabaseExists("Server=(LocalDB)\\MSSQLLocalDB;Integrated security=SSPI;database=master;MultipleActiveResultSets=true", "WindowsMonitor"))
            {
                Console.WriteLine("file is not Found so Data base is deleted");
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
                    write_log_file("old Database is Droped", "-----", ex.Message);          // call method to write error log
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
                Console.WriteLine("Create New DataBase Filellllllllllllllllll\n");
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
                str = @"CREATE DATABASE WindowsMonitor ON PRIMARY (NAME = Database1,FILENAME = '" + curFile + @"',SIZE = 9MB, MAXSIZE = 200MB,
                    FILEGROWTH = 10%) LOG ON (NAME = Database1log, FILENAME = '" + log_file + "', SIZE = 1MB, MAXSIZE = 5MB, FILEGROWTH = 10%)";
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
                write_log_file("Database File creation", "---Error ---", ex.Message);          // call method to write error log
            }
            finally
            {
                myConn.Close();
            }
            return;
        }

        // Check table Existane 
        private static bool CheckTableExists(string table_name)
        {
            bool bRet = false;
            Custom_methods cust_obj = new Custom_methods();
            using (var connection = cust_obj.get_connectionString())
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
                        cust_obj.write_log_file("Table Checking", "---error--", ex.Message);          // call method to write error log
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return bRet;
        }

        // create table string from Dictionary
        private string create_table_string(Dictionary<string, string> table_column, string table_name)
        {
            string str = "CREATE TABLE [dbo].[" + table_name + "] (";
            foreach (KeyValuePair<string, string> author in table_column)
            {
                string key = author.Key.ToString();
                string value = author.Value.ToString();
                str += " " + key + " " + value + ",";
                //Console.WriteLine("Key: {0}, Value: {1}", author.Key, author.Value);
            }
            str += "PRIMARY KEY  CLUSTERED([Id] ASC))";
            return str;
        }

        //Check for column Existance 
        private static bool CheckColumnExists(string columnName, string tableName)
        {
            bool bRet = false;
            Custom_methods cust_obj = new Custom_methods();
            using (var connection = cust_obj.get_connectionString())
            {
                columnName = columnName.Replace('[', ' ');
                columnName = columnName.Replace(']', ' ');
                columnName = columnName.Trim();
                string query = "select column_name from information_schema.columns where column_name = '" + columnName + "' and table_name ='" + tableName + "'";
                using (var command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        SqlDataReader process_id = command.ExecuteReader();
                        if (process_id.Read())
                        {
                            bRet = true;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        cust_obj.write_log_file("Column Checking in " + tableName + " for column " + columnName + "", "---error--", ex.Message);          // call method to write error log
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return bRet;
        }
        // Create table Structure
        private void create_new_table(Dictionary<string, string> table_column, string table_name)
        {
            Custom_methods cust_obj = new Custom_methods();
            SqlConnection m_dbConnection = cust_obj.get_connectionString();
            try
            {
                string query = create_table_string(table_column, table_name);       // call method to get create table string
                m_dbConnection.Open();
                SqlCommand create_table1 = new SqlCommand(query, m_dbConnection);
                create_table1.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                cust_obj.write_log_file("Create Table " + table_name + " ", "---error--", ex.Message);    // call method to write error log
            }
            finally
            {
                m_dbConnection.Close();
            }
        }

        // Alter Table TO Add Column
        private void alter_Column(Dictionary<string, string> table_column, string table_name)
        {
            foreach (KeyValuePair<string, string> author in table_column)
            {
                string key = author.Key.ToString();
                string values = author.Value.ToString();
                if (!CheckColumnExists(key, table_name))
                {
                    Custom_methods cust_obj = new Custom_methods();
                    SqlConnection connection = cust_obj.get_connectionString();
                    connection.Open();
                    SqlCommand command = new SqlCommand("alter table " + table_name + " add " + key + " " + values, connection);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        cust_obj.write_log_file("Add Column " + key + " in Table " + table_column + " ", "---error--", ex.Message);          // call method to write error log
                    }
                    catch (Exception ex)
                    {
                        cust_obj.write_log_file("Add Column " + key + " in Table " + table_column + " ", "---error--", ex.Message);          // call method to write error log
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        // Checking For Table Structure 
        public void cteate_table(string curFile)
        {
            String create_table;
            SqlConnection m_dbConnection6 = get_connectionString();
            Console.WriteLine("Checking for Tablesssssssssssssssssssssss\n");
            try
            {
                Dictionary<string, string> table_column = new Dictionary<string, string>();
                table_column.Add("Id", "INT IDENTITY (1, 1) NOT NULL");
                table_column.Add("process_id", "INT NULL");
                table_column.Add("process_name", "VARCHAR(512) NULL");
                table_column.Add("start_time", "DATETIME NULL");
                table_column.Add("end_time", "DATETIME NULL");
                table_column.Add("title", "VARCHAR(512) NULL");
                table_column.Add("location", "VARCHAR(512) NULL");
                table_column.Add("test2", "VARCHAR(512) NULL");
                table_column.Add("test5", "VARCHAR(512) NULL");
                if (!CheckTableExists("application_details"))
                {
                    create_new_table(table_column, "application_details");
                }
                else
                {
                    Console.WriteLine("Alter Database column");
                    alter_Column(table_column, "application_details");
                }
                Console.WriteLine("done Application detail");

                //application session details table
                table_column.Clear();
                table_column.Add("Id", "INT IDENTITY (1, 1) NOT NULL");
                table_column.Add("app_detail_id", "INT NULL");
                table_column.Add("title", "VARCHAR(512) NULL");
                table_column.Add("start_time", "DATETIME NULL");
                table_column.Add("end_time", "DATETIME NULL");
                if (!CheckTableExists("application_session_details"))
                {
                    create_new_table(table_column, "application_session_details");
                }
                else
                {
                    alter_Column(table_column, "application_session_details");
                }
                Console.WriteLine("done application sesion detail");

                //outlook table
                table_column.Clear();
                table_column.Add("Id", "INT IDENTITY (1, 1) NOT NULL");
                table_column.Add("folder", "VARCHAR(512) NULL");
                if (!CheckTableExists("outlook"))
                {
                    create_new_table(table_column, "outlook");
                }
                else
                {
                    alter_Column(table_column, "outlook");
                }
                Console.WriteLine("done outlook");

                //outlook mail table
                table_column.Clear();
                table_column.Add("Id", "INT IDENTITY (1, 1) NOT NULL");
                table_column.Add("outlook_id", "INT NULL");
                table_column.Add("folder_name", "VARCHAR(512) NULL");
                table_column.Add("parent_folder_name", "VARCHAR(512) NULL");
                if (!CheckTableExists("outlook_mail"))
                {
                    create_new_table(table_column, "outlook_mail");
                }
                else
                {
                    alter_Column(table_column, "outlook_mail");
                }
                Console.WriteLine("done outlook mail");

                //outlook mail draftbox table
                table_column.Clear();
                table_column.Add("Id", "INT IDENTITY (1, 1) NOT NULL");
                table_column.Add("outlook_mail_id", "INT NULL");
                table_column.Add("subject", "VARCHAR(512) NULL");
                table_column.Add("start_time", "DATETIME NULL");
                table_column.Add("send_time", "DATETIME NULL");
                table_column.Add("body", "TEXT NULL");
                table_column.Add("state", "VARCHAR(512) NULL");
                if (!CheckTableExists("outlook_mail_draftbox"))
                {
                    create_new_table(table_column, "outlook_mail_draftbox");
                }
                else
                {
                    alter_Column(table_column, "outlook_mail_draftbox");
                }
                Console.WriteLine("done outlook_mail_draftbox");

                //outlook mail box table
                table_column.Clear();
                table_column.Add("Id", "INT IDENTITY (1, 1) NOT NULL");
                table_column.Add("mail_box_id", "INT NULL");
                table_column.Add("subject", "VARCHAR(512) NULL");
                table_column.Add("body", "TEXT NULL");
                table_column.Add("received_time", "DATETIME NULL");
                table_column.Add("read_time", "DATETIME NULL");
                table_column.Add("reply_bool", "BIT NULL");
                table_column.Add("reply_time", "DATETIME NULL");
                table_column.Add("reply_all_bool", "BIT NULL");
                table_column.Add("attachment", "TEXT NULL");
                table_column.Add("no_of_attachment", "INT NULL");
                table_column.Add("parent_folder_name", "VARCHAR(512) NULL");
                table_column.Add("unread_bool", "BIT NULL");
                table_column.Add("entry_id", "VARCHAR(1024) NULL");
                table_column.Add("outlook_mail_id", "INT NULL");

                if (!CheckTableExists("outlook_mail_inbox"))
                {
                    create_new_table(table_column, "outlook_mail_inbox");
                }
                else
                {
                    alter_Column(table_column, "outlook_mail_inbox");
                }
                Console.WriteLine("done outlook_mail_inbox");

                //calender table
                table_column.Clear();
                table_column.Add("Id", "INT IDENTITY (1, 1) NOT NULL");
                table_column.Add("entry_id", "VARCHAR(1024) NULL");
                table_column.Add("subject", "VARCHAR(512) NULL");
                table_column.Add("body", "TEXT NULL");
                table_column.Add("location", "TEXT NULL");
                table_column.Add("start_time", "DATETIME NULL");
                table_column.Add("end_time", "DATETIME NULL");
                table_column.Add("creation_date", "DATETIME NULL");
                table_column.Add("IsRecurring", "BIT NULL");
                table_column.Add("ModificationTime", "DATETIME NULL");
                table_column.Add("MeetingStatus", "VARCHAR(1024) NULL");
                if (!CheckTableExists("calender"))
                {
                    create_new_table(table_column, "calender");
                }
                else
                {
                    alter_Column(table_column, "calender");
                }
                Console.WriteLine("done calender");

                //url_info table
                table_column.Clear();
                table_column.Add("Id", "INT IDENTITY (1, 1) NOT NULL");
                table_column.Add("url", "VARCHAR(512) NULL");
                table_column.Add("start_time", "DATETIME NULL");
                table_column.Add("end_time", "DATETIME NULL");
                if (!CheckTableExists("url_info"))
                {
                    create_new_table(table_column, "url_info");
                }
                else
                {
                    alter_Column(table_column, "url_info");
                }
                Console.WriteLine("done url_info");

                //registration table
                table_column.Clear();
                table_column.Add("Id", "INT IDENTITY (1, 1) NOT NULL");
                table_column.Add("name", "VARCHAR(512) NULL");
                table_column.Add("password", "VARCHAR(512) NULL");
                table_column.Add("email", "VARCHAR(512) NULL");
                table_column.Add("phn_no", "VARCHAR(512) NULL");
                table_column.Add("skype", "VARCHAR(512) NULL");
                table_column.Add("other", "VARCHAR(512) NULL");
                if (!CheckTableExists("registration"))
                {
                    create_new_table(table_column, "registration");

                }
                else
                {
                    alter_Column(table_column, "registration");
                }
                Console.WriteLine("done registration");
            
            

                //insert data into registration table
        

        //social media table
        table_column.Clear();
                table_column.Add("Id", "INT IDENTITY (1, 1) NOT NULL");
                table_column.Add("url", "VARCHAR(512) NULL");
                table_column.Add("browser", "VARCHAR(512) NULL");
                if (!CheckTableExists("social_media_urls"))
                {
                    create_new_table(table_column, "social_media_urls");
                    create_table = "insert into social_media_urls values('facebook'),('twitter');";
                    SqlCommand create_table10 = new SqlCommand(create_table, m_dbConnection6);
                    create_table10.ExecuteNonQuery();
                }
                else
                {
                    alter_Column(table_column, "social_media_urls");
                }
                Console.WriteLine("done social_media");

                table_column.Clear();
                table_column.Add("Id", "INT IDENTITY (1, 1) NOT NULL");
                table_column.Add("name", "VARCHAR(512) NULL");
                if (!CheckTableExists("browsers"))
                {
                    create_new_table(table_column, "browsers");
                    create_table = "insert into browsers values ('firefox'),('chrome'),('explorer');";
                    SqlCommand create_table11 = new SqlCommand(create_table, m_dbConnection6);
                    create_table11.ExecuteNonQuery();
                }
                else
                {
                    alter_Column(table_column, "browsers");
                }
                Console.WriteLine("done brower");

                table_column.Clear();
                table_column.Add("Id", "INT IDENTITY (1, 1) NOT NULL");
                table_column.Add("name", "VARCHAR(512) NULL");
                table_column.Add("email", "VARCHAR(512) NULL");
                table_column.Add("mail_id", "VARCHAR(1024) NULL");
                if (!CheckTableExists("recipients "))
                {
                       create_new_table(table_column, "recipients");
                }
                else
                {
                    alter_Column(table_column, "recipients");
                }
                Console.WriteLine("done recipietnts");
            }
            catch (System.Exception ex)
            {
                write_log_file("Database Table", "Creation Error 2------", ex.Message);          // call method to write error log
            }
            finally
            {
                if (m_dbConnection6.State == ConnectionState.Open)
                {
                    m_dbConnection6.Close();
                }
            }
        }
        //insert into registration
        public void insertData(string name, string password, string email, string phn_no, string skype, string other)
        {
            SqlConnection conn = get_connectionString();
            conn.Open();
            Console.WriteLine("+++++++++++++++++++++++++++++++++++");
            try
            {
                SqlCommand command = new SqlCommand("insert into registration(name,password,email,phn_no,skype,other) values (@name,@password,@email,@phn_no,@skype,@other)", conn);
                command.Parameters.AddWithValue("name", name);
                command.Parameters.AddWithValue("password", password);
                command.Parameters.AddWithValue("email", email);
                command.Parameters.AddWithValue("phn_no", phn_no);
                command.Parameters.AddWithValue("skype", skype);
                command.Parameters.AddWithValue("other", other);
                command.ExecuteNonQuery();
                System.Windows.Forms.MessageBox.Show("Successfully inserted....");
            }
            catch (Exception ex)
            {
                write_log_file("----------Error------", "Insertion Error---------", ex.Message);
            }
            conn.Close();
        }
        
    }
}
