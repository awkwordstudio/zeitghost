using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using NDde.Client;
using System.IO;
using System.Data.SQLite;
using System.Data.OleDb;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Interop;
using SHDocVw;
namespace SysInfo
{
    public partial class Main_Form : Form
    {
        //private System.Data.SqlClient.SqlConnection conn1 = null;
        //private string ConnectionString1 = "Integrated Security=SSPI;Initial Catalog=;Data Source=localhost;";
        string curFile;
        OleDbConnection m_dbConnection1;

        /*private void ExecuteSQLStmt1(string sql)
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
            ConnectionString = "Integrated Security=SSPI;" +
            "Initial Catalog=mydb;" +
            "Data Source=localhost;";
            conn.ConnectionString = ConnectionString;
            conn.Open();
            cmd = new System.Data.SqlClient.SqlCommand(sql, conn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ae)
            {
                MessageBox.Show(ae.Message.ToString());
            }
        }
        private void CreateDBBtn_Click()
        {
            // Create a connection
            conn = new System.Data.SqlClient.SqlConnection(ConnectionString);
            // Open the connection
            if (conn.State != ConnectionState.Open)
                conn.Open();
            string sql = "CREATE DATABASE mydb ON PRIMARY"
            + "(Name=test_data, filename = 'C:\\mysql\\mydb_data.mdf', size=3,"
            + "maxsize=5, filegrowth=10%)log on"
            + "(name=mydbb_log, filename='C:\\mysql\\mydb_log.ldf',size=3,"
            + "maxsize=20,filegrowth=1)";
            ExecuteSQLStmt(sql);
        }
        */
        public Main_Form()
        {
            InitializeComponent();
            processStartEvent.EventArrived += new EventArrivedEventHandler(processStartEvent_EventArrived);
            processStartEvent.Start();
            processStopEvent.EventArrived += new EventArrivedEventHandler(processStopEvent_EventArrived);
            processStopEvent.Start();

            HookManager.SubscribeToWindowEvents();

            //To create DB of application
            string str_Path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string subPath = str_Path.ToString() + @"\Documents\WindowsMonitor\";              // your code goes here
            bool exists = System.IO.Directory.Exists(subPath);
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(subPath);
            }
            else
            {
                string curFile1 = subPath + "MonitorAppdb.laccdb";
                if (File.Exists(curFile1))
                {
                    File.Delete(curFile1);
                }
            }
            curFile = subPath + "MonitorAppdb.accdb";
            if (File.Exists(curFile))
            {
            }
            else
            {
                //CreateDBBtn_Click();
                //create_db(subPath);
                string exe_path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)+ "/Database11.accdb";
                File.Copy(exe_path, curFile);
            }
            m_dbConnection1 = new OleDbConnection();
            m_dbConnection1.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + curFile + ";Persist Security Info=False;";

            Microsoft.Office.Interop.Outlook.ApplicationClass oApp = null;
            Microsoft.Office.Interop.Outlook.NameSpace mapiNamespace = null;
            try
            {
                //Email
                oApp = new Microsoft.Office.Interop.Outlook.ApplicationClass();
                mapiNamespace = oApp.GetNamespace("MAPI");
                mapiNamespace.Logon(null, null, false, false);

                // Ring up the new message event.
                oApp.NewMailEx += new Outlook.ApplicationEvents_11_NewMailExEventHandler(outLookApp_NewMailEx);
                oApp.NewMailEx += new Outlook.ApplicationEvents_11_NewMailExEventHandler(outLookApp_send);
                Console.WriteLine("Please wait for new messages...");
                Console.ReadLine();
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Exception in Main " + e);
            }
            finally
            {
                mapiNamespace = null;
                oApp = null;
            }
        }
        public static Main_Form _main;
        ManagementEventWatcher processStartEvent = new ManagementEventWatcher("SELECT * FROM Win32_ProcessStartTrace");
        ManagementEventWatcher processStopEvent = new ManagementEventWatcher("SELECT * FROM Win32_ProcessStopTrace");

        void outLookApp_NewMailEx(string EntryIDCollection)
        {
            Console.WriteLine("OUTLOOK IS START as NEW MAIL RECEIVED..");

            Console.WriteLine("You've got a new mail whose EntryIDCollection is \n" + EntryIDCollection);
            Microsoft.Office.Interop.Outlook.Application app = new Microsoft.Office.Interop.Outlook.Application();
            // Microsoft.Office.Interop.Outlook.MailItem item = null;
            Microsoft.Office.Interop.Outlook._NameSpace ns = app.GetNamespace("MAPI");

            try
            {

                //ns.Logon(null, null, false, false);
                Console.WriteLine("testkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk");
                //item = ns.GetItemFromID(EntryIDCollection);
                //Console.WriteLine("test" + item.Subject);
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Exception in event Handler " + e);
            }
            finally
            {
                ns = null;
                app = null;

            }
        }
        void outLookApp_send(string EntryIDCollection)
        {
            Console.WriteLine("OUTLOOK IS START as NEW MAIL SEND..");

        }
        private void Form2_Load(object sender, EventArgs e)
        {
            _pnlHome.Visible = true;
            _pnlEmail.Visible = false;
            _plnMeeting.Visible = false;
            _pnlBrowser.Visible = false;
            _pnlProject.Visible = false;
            _pnlUserInfo.Visible = false;
            _pnlSystemInformation.Visible = false;
            _pnlSettingMain.Visible = false;
            _pnlEmailGraph.Visible = false;

            _ptr_mail_box.BorderStyle = BorderStyle.None;
            _ptr_calender.BorderStyle = BorderStyle.None;
            _ptr_browser.BorderStyle = BorderStyle.None;
            _ptrUser.BorderStyle = BorderStyle.None;
            _ptr_system.BorderStyle = BorderStyle.None;
            _ptrSetting.BorderStyle = BorderStyle.None;

            Graph_load();

            // get USER account Name
            String User_Account_Name = String.Empty;
            ManagementObjectSearcher searcher3 = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_UserAccount");
            foreach (ManagementObject wmi in searcher3.Get())
            {
                try
                {
                    User_Account_Name = wmi.GetPropertyValue("Name").ToString();
                }
                catch { }
            }
            // Get Log in Time
            String log_in = String.Empty;
            ManagementScope ms = new ManagementScope("\\root\\cimv2");
            ObjectQuery oq = new ObjectQuery("Select * from Win32_Session");
            ManagementObjectSearcher query = new ManagementObjectSearcher(ms, oq);
            ManagementObjectCollection queryCollection = query.Get();

            foreach (ManagementObject mo in queryCollection)
            {
                if (mo["LogonType"].ToString().Equals("0")) //  2 - for logged on User
                {
                    log_in = mo["StartTime"].ToString();
                }
            }
            _lblUserName.Text = " [ " + User_Account_Name + "  ]";
            _lblTimer.Font = new Font(_lblTimer.Font.Name, 8, FontStyle.Bold);
            _lblTimer.ForeColor = System.Drawing.Color.WhiteSmoke;
            System.Windows.Forms.Timer t = null;
            t = new System.Windows.Forms.Timer();
            t.Interval = 1000;
            t.Tick += new EventHandler(t_Tick);
            t.Enabled = true;
        }
        void t_Tick(object sender, EventArgs e)
        {
            _lblTimer.Text = DateTime.Now.ToString();
        }

        void processStartEvent_EventArrived(object sender, EventArrivedEventArgs e)
        {
            Console.WriteLine("start process call");
            try
            {
                string processName = e.NewEvent.Properties["ProcessName"].Value.ToString();
                string process_id = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value).ToString();
                Process localById = Process.GetProcessById(Int32.Parse(process_id));

                String file_path;
                Console.WriteLine("hhhhhhhhhhhhhhhhhhhhhh" + localById.MainWindowTitle.ToString());
                if (!string.IsNullOrEmpty(localById.MainWindowTitle))
                {

                    if (!Environment.Is64BitProcess)
                    {
                        file_path = null;
                    }
                    else
                    {
                        file_path = localById.MainModule.FileName;
                    }
                    if (m_dbConnection1.State == ConnectionState.Open)
                    {
                        m_dbConnection1.Close();
                    }
                    m_dbConnection1.Open();
                    string Main_str;
                    Main_str = "INSERT INTO application_details(process_ID,process_name,start_time,Title) VALUES(" + localById.Id + ",'" + localById.ProcessName + "','" + localById.StartTime.ToString("yyyy-MM-dd HH:mm:ss") + "','" + localById.MainWindowTitle + "')";
                    //Main_str = "INSERT INTO application_details(process_ID,process_name,start_time) VALUES(" + localById.Id + ",'" + localById.ProcessName + "','" + localById.StartTime.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    Console.WriteLine(">>>>>> Query To Insert " + Main_str);
                    OleDbCommand cmd = new OleDbCommand(Main_str, m_dbConnection1);
                    cmd.ExecuteNonQuery(); //This line crashes
                    m_dbConnection1.Close();
                }
            }
            catch (Exception ee)
            {
                if (m_dbConnection1.State == ConnectionState.Open)
                {
                    m_dbConnection1.Close();
                }
                Console.Write(" >>>>>>Exception Accured to insert process \n" + ee.Message);
            }
        }

        void processStopEvent_EventArrived(object sender, EventArrivedEventArgs e)
        {
            Console.WriteLine("stop process call");
            
                string processID = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value).ToString();
                if (m_dbConnection1.State == ConnectionState.Open)
                {
                    m_dbConnection1.Close();
                }

                string qstr;
                m_dbConnection1.Open();
                qstr = "SELECT id FROM application_details WHERE process_ID=" + Int32.Parse(processID) + " AND end_time is null;";
                OleDbCommand cmd = new OleDbCommand(qstr, m_dbConnection1);
                OleDbDataReader process_id = cmd.ExecuteReader();
                if (process_id.Read())
                {
                    try
                    {
                        string Main_str;
                        Main_str = "UPDATE Application_details SET end_time='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE id=" + process_id[0].ToString() + " AND end_time is null;";
                        cmd = new OleDbCommand(Main_str, m_dbConnection1);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Win32Exception ee)
                    {
                        if (m_dbConnection1.State == ConnectionState.Open)
                        {
                            m_dbConnection1.Close();
                        }
                        Console.Write("Error accured during update process end time" + ee.Message);
                    }
                }
            
            if (m_dbConnection1.State == ConnectionState.Open)
            {
                m_dbConnection1.Close();
            }
        }
        private void Main_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Hide();
                e.Cancel = true;
            }
            //processStartEvent.Stop();
            //processStopEvent.Stop();
            //HookManager.UnhookWinEvent(HookManager.windowEventHook);
        }
        private void _addSysTray_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Maximized;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //string connetionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=your .mdb file;";
            string sql = "SELECT * FROM Url_Info";
           // OleDbConnection connection = new OleDbConnection(m_dbConnection1);
            OleDbDataAdapter dataadapter = new OleDbDataAdapter(sql, m_dbConnection1);
            DataSet ds = new DataSet();
            m_dbConnection1.Open();
            dataadapter.Fill(ds, "Authors_table");
            m_dbConnection1.Close();
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "Authors_table";
        }
    }

    public static class HookManager
    {

        public static void SubscribeToWindowEvents()
        {
            if (windowEventHook == IntPtr.Zero)
            {
                _WindowEventCallback = new WinEventDelegate(WindowEventCallback);
                windowEventHook = SetWinEventHook(
                    EVENT_SYSTEM_FOREGROUND, // eventMin
                    EVENT_SYSTEM_FOREGROUND, // eventMax
                    IntPtr.Zero,             // hmodWinEventProc
                    _WindowEventCallback,//WindowEventCallback,     // lpfnWinEventProc
                    0,                       // idProcess
                    0,                       // idThread
                    WINEVENT_OUTOFCONTEXT | WINEVENT_SKIPOWNPROCESS);

                if (windowEventHook == IntPtr.Zero)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
        }

        
        private static void WindowEventCallback(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
        	string str_Path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string subPath = str_Path.ToString() + @"\Documents\WindowsMonitor\";
        string curFile = subPath + "MonitorAppdb.accdb";
            OleDbConnection m_dbConnection2 = new OleDbConnection();
            m_dbConnection2.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+curFile+";Persist Security Info=False;";
                       
            String active_process_id_temp = null;
            IntPtr fg = GetForegroundWindow();
            //SetForegroundWindow(fg);
            uint pid;
            if (m_dbConnection2.State == ConnectionState.Open)
            {
                m_dbConnection2.Close();
            }
            GetWindowThreadProcessId(hwnd, out pid);
            Process p = Process.GetProcessById((int)pid);
            if (!String.IsNullOrEmpty(p.MainWindowTitle))
            {
                try
                {
                    String file_path;
                    if (!Environment.Is64BitProcess)
                    {
                        file_path = null;
                    }
                    else
                    {
                        file_path = p.MainModule.FileName;
                    }
                    m_dbConnection2.Open();
                    OleDbCommand cmd = new OleDbCommand("SELECT id FROM application_session_details WHERE app_detail_id=(select id from application_details where  process_ID=" + p.Id + ") AND end_time is null;", m_dbConnection2);
                    OleDbDataReader old_process_id = cmd.ExecuteReader();
                    OleDbCommand app_cmd = new OleDbCommand("SELECT id FROM application_details WHERE process_ID=" + p.Id + " AND end_time is null;", m_dbConnection2);
                    OleDbDataReader active_process_id = app_cmd.ExecuteReader();
                    Boolean app_type = false;
                    if (active_process_id.Read())
                    {
                        app_type = true;
                    }
                    if (!app_type)
                    {
                        OleDbCommand new_cmd = new OleDbCommand("INSERT INTO application_details(process_ID,process_name,Title,start_time,location) VALUES(" + p.Id + ",'" + p.ProcessName + "','" + p.MainWindowTitle + "','" + p.StartTime + "','" + file_path + "')", m_dbConnection2);
                        new_cmd.ExecuteNonQuery();
                        app_cmd = new OleDbCommand("SELECT id FROM application_details WHERE process_ID=" + p.Id + " AND end_time is null;", m_dbConnection2);
                        active_process_id = app_cmd.ExecuteReader();
                    }
                    if (app_type)
                    {
                        active_process_id_temp = active_process_id["id"].ToString();
                        if (!old_process_id.Read())
                        {
                            String today_date = DateTime.Now.ToString();
                            OleDbCommand cmd1 = new OleDbCommand("UPDATE application_session_details SET end_time='" + today_date + "' WHERE end_time is null;", m_dbConnection2);
                            cmd1.ExecuteNonQuery();
                            OleDbCommand insert_cmd = new OleDbCommand("INSERT INTO application_session_details(app_detail_id,start_time,Title) VALUES(" + active_process_id_temp + ",'" + today_date + "','" + p.MainWindowTitle + "')", m_dbConnection2);
                            insert_cmd.ExecuteNonQuery();
                        }
                    }


                    m_dbConnection2.Close();
                }
                catch (OleDbException sqlce)
                {
                    if (m_dbConnection2.State == ConnectionState.Open)
                    {
                        m_dbConnection2.Close();
                    }
                    Console.WriteLine("EEEEEEEEEEEERRRRRRRRRRRRRRRRRRRORRRRRRRRR" + sqlce.Message);
                }
                finally
                {
                    if (m_dbConnection2.State == ConnectionState.Open)
                    {
                        m_dbConnection2.Close();
                    }
                    Get_browser_Url((int)pid, (String)p.ProcessName, hwnd);
                    Get_outlook_info((int)pid, (String)p.ProcessName, hwnd);
                }
                Console.WriteLine("Window event call back");
            }
        }

        public static IntPtr windowEventHook;
        private static WinEventDelegate _WindowEventCallback;
        delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();
        private delegate void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        [DllImport("user32.dll")]

        private static extern IntPtr SetWinEventHook(int eventMin, int eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, int idProcess, int idThread, int dwflags);

        private const int WINEVENT_INCONTEXT = 4;
        private const int WINEVENT_OUTOFCONTEXT = 0;
        private const int WINEVENT_SKIPOWNPROCESS = 2;
        private const int WINEVENT_SKIPOWNTHREAD = 1;
        private const int EVENT_SYSTEM_FOREGROUND = 3;
        public static void Get_browser_Url(int proc_id, String proc_name, IntPtr hwnd)
        {
        	string str_Path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string subPath = str_Path.ToString() + @"\Documents\WindowsMonitor\";
        string curFile = subPath + "MonitorAppdb.accdb";
            Boolean browse_bool = false;
            OleDbConnection m_dbConnection3 = new OleDbConnection();
            m_dbConnection3.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+curFile+";Persist Security Info=False;";
            String today_date;
            if (proc_name == "chrome") { browse_bool = true; }
            else if (proc_name == "iexplore") { browse_bool = true; } //iexplore
            else if (proc_name == "firefox") { browse_bool = true; }
            String temp_Url = null;
            String Url;
            String domain_url = null;
            while (browse_bool)
            {
                uint pid1;
                IntPtr fg = GetForegroundWindow();
                GetWindowThreadProcessId(fg, out pid1);
                Process proce = Process.GetProcessById((int)pid1);
                if ((String)proce.ProcessName == "chrome")
                {
                    Url = GetChromeUrl(proce);
                }
                else if ((String)proce.ProcessName == "firefox")
                {
                    Url = GetFirefoxUrl(proce);
                }
                else if ((String)proce.ProcessName == "iexplore")
                {
                    Url = GetInternetExplorerUrl(proce);
                }
                else
                {
                    if (m_dbConnection3.State == ConnectionState.Open)
                    {
                        m_dbConnection3.Close();
                    }
                    m_dbConnection3.Open();
                    today_date = DateTime.Now.ToString();
                    OleDbCommand cmd1 = new OleDbCommand("UPDATE Url_Info SET end_time='" + today_date + "' WHERE end_time is null;", m_dbConnection3);
                    cmd1.ExecuteNonQuery();
                    break;
                }
                if (temp_Url != Url)
                {
                    if (Url != null)
                    {
                        foreach (String url1 in Url.Split('/'))
                        {
                            if (url1.IndexOf(".") != -1)
                            {
                                domain_url = url1;
                                break;
                            }
                        }
                    }
                    if (m_dbConnection3.State == ConnectionState.Open)
                    {
                        m_dbConnection3.Close();
                    }
                    m_dbConnection3.Open();
                    today_date = DateTime.Now.ToString();
                    OleDbCommand app_cmd = new OleDbCommand("SELECT id FROM Url_Info WHERE url='" + domain_url + "' AND end_time is null;", m_dbConnection3);
                    OleDbDataReader active_process_id = app_cmd.ExecuteReader();
                    if (!active_process_id.Read())
                    {
                        OleDbCommand cmd1 = new OleDbCommand("UPDATE Url_Info SET end_time='" + today_date + "' WHERE end_time is null;", m_dbConnection3);
                        cmd1.ExecuteNonQuery();
                    }
                    OleDbCommand insert_cmd = new OleDbCommand("INSERT INTO Url_Info(url,start_time) VALUES('" + domain_url + "','" + today_date + "')", m_dbConnection3);
                    insert_cmd.ExecuteNonQuery();
                    temp_Url = Url;
                }
                System.Threading.Thread.Sleep(500);
            }
            if (m_dbConnection3.State == ConnectionState.Open)
            {
                m_dbConnection3.Close();
            }
        }
        public static string GetChromeUrl(Process process)
        {
            if (process == null)
                throw new ArgumentNullException("process");

            if (process.MainWindowHandle == IntPtr.Zero)
                return null;

            System.Windows.Automation.AutomationElement element = System.Windows.Automation.AutomationElement.FromHandle(process.MainWindowHandle);
            if (element == null)
                return null;
            System.Windows.Automation.AutomationElement elmUrlBar = element.FindFirst(System.Windows.Automation.TreeScope.Descendants,
              new System.Windows.Automation.PropertyCondition(System.Windows.Automation.AutomationElement.NameProperty, "Address and search bar"));

            if (elmUrlBar != null)
            {
                System.Windows.Automation.AutomationPattern[] patterns = elmUrlBar.GetSupportedPatterns();
                if (patterns.Length > 0)
                {
                    System.Windows.Automation.ValuePattern val = (System.Windows.Automation.ValuePattern)elmUrlBar.GetCurrentPattern(patterns[0]);
                    return val.Current.Value as String;
                }
            }
            return null;
        }
        public static string GetFirefoxUrl(Process process)
        {
            DdeClient dde = new DdeClient("Firefox", "WWW_GetWindowInfo");
            dde.Connect();
            string url = dde.Request("URL", int.MaxValue);
            dde.Disconnect();
            if (url != null)
                return url.Split(',')[0];
            else return null;
        }

        public static string GetInternetExplorerUrl(Process process)
        {

            if (process == null)
                throw new ArgumentNullException("process");

            if (process.MainWindowHandle == IntPtr.Zero)
                return null;

            System.Windows.Automation.AutomationElement element = System.Windows.Automation.AutomationElement.FromHandle(process.MainWindowHandle);
            if (element == null)
                return null;

            System.Windows.Automation.AutomationElement rebar = element.FindFirst(System.Windows.Automation.TreeScope.Children, new System.Windows.Automation.PropertyCondition(System.Windows.Automation.AutomationElement.ClassNameProperty, "ReBarWindow32"));
            if (rebar == null)
                return null;

            System.Windows.Automation.AutomationElement edit = rebar.FindFirst(System.Windows.Automation.TreeScope.Subtree, new System.Windows.Automation.PropertyCondition(System.Windows.Automation.AutomationElement.ControlTypeProperty, System.Windows.Automation.ControlType.Edit));

            return ((System.Windows.Automation.ValuePattern)edit.GetCurrentPattern(System.Windows.Automation.ValuePattern.Pattern)).Current.Value as string;
        }

        public static void Get_outlook_info(int proc_id, String proc_name, IntPtr hwnd)
        {
            Console.WriteLine("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFRRRRRRRRRRRRRRR");
            Boolean outlook_bool = false;
            string str_Path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string subPath = str_Path.ToString() + @"\Documents\WindowsMonitor\";
        string curFile = subPath + "MonitorAppdb.accdb";
            OleDbConnection m_dbConnection4 = new OleDbConnection();
            m_dbConnection4.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+curFile+";Persist Security Info=False;";
            //String today_date;
            if (proc_name == "OUTLOOK") { outlook_bool = true; }
            // while (outlook_bool)
            // {
            uint pid1;
            IntPtr fg = GetForegroundWindow();
            GetWindowThreadProcessId(fg, out pid1);
            Process proce = Process.GetProcessById((int)pid1);
            if ((String)proce.ProcessName == "OUTLOOK")
            {
                Outlook.Application app = new Outlook.Application();
                Outlook._NameSpace ns = app.GetNamespace("MAPI");
                try
                {
                    Console.WriteLine("TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTt");
                    Outlook._Folders oFolders;
                    Outlook.MAPIFolder oPublicFolder = (Outlook.MAPIFolder)ns.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox).Parent;
                    oFolders = oPublicFolder.Folders;
                    Outlook._NameSpace abc = (Outlook._NameSpace)oPublicFolder.Parent;
                    Outlook._Folders oFolders1 = abc.Folders;
                    foreach (Outlook.MAPIFolder Folder in oFolders1)
                    {
                        string foldername = Folder.Name;
                        m_dbConnection4.Open();
                        OleDbCommand app_cmd = new OleDbCommand("SELECT id FROM outlook WHERE folder='" + foldername + "';", m_dbConnection4);
                        OleDbDataReader main_folder = app_cmd.ExecuteReader();
                        if (!main_folder.Read())
                        {
                            OleDbCommand insert_cmd = new OleDbCommand("INSERT INTO outlook(folder) VALUES('" + foldername + "')", m_dbConnection4);
                            insert_cmd.ExecuteNonQuery();
                        }
                        m_dbConnection4.Close();
                        if (Folder.Folders.Count > 0)
                        {
                            while (true)
                            {
                                fg = GetForegroundWindow();
                                GetWindowThreadProcessId(fg, out pid1);
                                proce = Process.GetProcessById((int)pid1);
                                get_folder_detail(Folder);
                                if ((String)proce.ProcessName != "OUTLOOK")
                                    break;
                                System.Threading.Thread.Sleep(5000);
                            }
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Console.WriteLine("TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTt");
                    Console.WriteLine("Exception in event Handler " + e);
                }
                finally
                {
                    ns = null;
                    app = null;
                }
            }

            System.Threading.Thread.Sleep(500);
        }
        public static void get_folder_detail(Outlook.MAPIFolder folder)
        {
        	string str_Path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string subPath = str_Path.ToString() + @"\Documents\WindowsMonitor\";
        string curFile = subPath + "MonitorAppdb.accdb";
            OleDbConnection m_dbConnection5 = new OleDbConnection();
            m_dbConnection5.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+curFile+";Persist Security Info=False;";
            if (folder.Folders.Count == 0)
            {
                foreach (object item1 in folder.Items)
                {
                    Outlook.MailItem mailItem = item1 as Outlook.MailItem;
                    if (mailItem != null)
                    {
                        if (mailItem.Subject != null)
                        {
                            m_dbConnection5.Open();
                            OleDbCommand app_cmd = new OleDbCommand("SELECT id,unread_bool FROM outlook_mail_inbox WHERE entry_id='" + mailItem.EntryID + "';", m_dbConnection5);
                            OleDbDataReader main_folder = app_cmd.ExecuteReader();
                            if (!main_folder.Read())
                            {
                                int main_id, parent_id;
                                main_folder.Close();
                                Outlook.MAPIFolder parent_fname = (Outlook.MAPIFolder)folder.Parent;
                                OleDbCommand app_cmd1 = new OleDbCommand("SELECT id FROM outlook WHERE folder='" + parent_fname.Name + "';", m_dbConnection5);
                                OleDbDataReader main_folder1 = app_cmd1.ExecuteReader();
                                if (main_folder1.Read())
                                {
                                    main_id = main_folder1.GetInt32(0);
                                }
                                else main_id = 0;
                                main_folder1.Close();
                                app_cmd1 = new OleDbCommand("SELECT id FROM outlok_mail WHERE folder_name='" + folder.Name + "';", m_dbConnection5);
                                main_folder1 = app_cmd1.ExecuteReader();
                                if (main_folder1.Read())
                                {
                                    parent_id = main_folder1.GetInt32(0);
                                }
                                else parent_id = 0;
                                main_folder1.Close();
                                OleDbCommand insert_cmd = new OleDbCommand("INSERT INTO outlook_mail_inbox(mail_box_id, received_time, subject, outlook_mail_id, no_of_attachment,parent_folder_name,entry_id,unread_bool) VALUES(" + parent_id + ",'" + mailItem.ReceivedTime + "','" + mailItem.Subject.Replace("'","''") + "','" + main_id + "','" + mailItem.Attachments.Count + "','" + folder.Name + "','" + mailItem.EntryID + "'," + mailItem.UnRead + ")", m_dbConnection5);
                                insert_cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                Boolean unread_bool = main_folder.GetBoolean(1);
                                int unread_mail_id = main_folder.GetInt32(0);
                                Boolean mail_unread = mailItem.UnRead;
                                main_folder.Close();
                                String today_date = DateTime.Now.ToString();
                                Console.WriteLine("UPDATE outlook_mail_inbox SET read_time='" + today_date + "' WHERE id=" + unread_mail_id + ")");
                                if (mail_unread == false && unread_bool == true)
                                {

                                    OleDbCommand update_cmd = new OleDbCommand("UPDATE outlook_mail_inbox SET read_time='" + today_date + "', unread_bool=False WHERE id=" + unread_mail_id + ";", m_dbConnection5);
                                    update_cmd.ExecuteNonQuery();
                                }
                            }

                            m_dbConnection5.Close();
                        }
                    }
                }
            }
            else
            {
                foreach (Outlook.MAPIFolder subFolder in folder.Folders)
                {
                    String foldername = subFolder.Name;
                    int main_id;
                    m_dbConnection5.Open();
                    OleDbCommand app_cmd = new OleDbCommand("SELECT id FROM outlok_mail WHERE folder_name='" + foldername + "';", m_dbConnection5);
                    OleDbDataReader main_folder = app_cmd.ExecuteReader();
                    if (!main_folder.Read())
                    {
                        main_folder.Close();
                        OleDbCommand app_cmd1 = new OleDbCommand("SELECT id FROM outlook WHERE folder='" + folder.Name + "';", m_dbConnection5);
                        OleDbDataReader main_folder1 = app_cmd1.ExecuteReader();
                        if (main_folder1.Read())
                        {
                            main_id = main_folder1.GetInt32(0);
                        }
                        else main_id = 0;
                        OleDbCommand insert_cmd = new OleDbCommand("INSERT INTO outlok_mail(folder_name,outlook_id,parent_folder_name) VALUES('" + foldername + "'," + main_id + ",'" + folder.Name + "')", m_dbConnection5);
                        insert_cmd.ExecuteNonQuery();
                    }
                    m_dbConnection5.Close();
                    get_folder_detail(subFolder);
                }
            }
        }
    }

    public static class EventLoop
    {
        public static void Run()
        {
            MSG msg;
            while (true)
            {
                if (PeekMessage(out msg, IntPtr.Zero, 0, 0, PM_REMOVE))
                {
                    if (msg.Message == WM_QUIT)
                        break;

                    TranslateMessage(ref msg);
                    DispatchMessage(ref msg);
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSG
        {
            public IntPtr Hwnd;
            public uint Message;
            public IntPtr WParam;
            public IntPtr LParam;
            public uint Time;
            public System.Drawing.Point Point;
        }
        const uint PM_NOREMOVE = 0;
        const uint PM_REMOVE = 1;
        const uint WM_QUIT = 0x0012;

        [DllImport("user32.dll")]
        private static extern bool PeekMessage(out MSG lpMsg, IntPtr hwnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);
        [DllImport("user32.dll")]
        private static extern bool TranslateMessage(ref MSG lpMsg);
        [DllImport("user32.dll")]
        private static extern IntPtr DispatchMessage(ref MSG lpMsg);
    }
}
