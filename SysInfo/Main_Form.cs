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
using System.Data.OleDb;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Interop;
using SHDocVw;
using System.Data.SqlClient;

namespace SysInfo
{
    public partial class Main_Form : Form
    {

        private Custom_methods Custom_obj = new Custom_methods();
        private string Load_type="mail";
        public Main_Form()
        {
            InitializeComponent();
            panel_register.Visible = true;
            panel_integrate.Visible = false;
            panel_agree.Visible = false;
            panel_welcome.Visible = false;
            int formHeight = this.Height;
            int formWidth = this.Width;
            Console.WriteLine("eeeeeeeeeeeeeeeeeeeeeeee"+ formWidth.ToString()+"lllllllll"+ formHeight.ToString());
            string str_Path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string subPath = str_Path.ToString() + @"\Documents\WindowsMonitor\";
            bool exists = System.IO.Directory.Exists(subPath);
            
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(subPath);
            }
            string curFile = subPath + "Database1.mdf";
            string log_file = subPath + "Database1log.ldf";
            if (File.Exists(curFile))
            {
                Console.WriteLine("Database File Found...... ");
            }
            else
            {
                Console.WriteLine("DataBase file is not Foundddddddddddddddddddddddddd \n");
                Custom_obj.create_db(curFile, log_file);//To create DB of application
            }
            Custom_obj.cteate_table(curFile);           // to create tables

            processStartEvent.EventArrived += new EventArrivedEventHandler(processStartEvent_EventArrived);
            processStartEvent.Start();
            processStopEvent.EventArrived += new EventArrivedEventHandler(processStopEvent_EventArrived);
            processStopEvent.Start();
            Console.WriteLine("after start stop");
            HookManager.SubscribeToWindowEvents();
            try
            {
                Color white = Custom_obj._get_color("white");
                Color gray = Custom_obj._get_color("grey");

                _pnl.BackColor = gray;
                _pnlHome.BackColor          = white;
                _pnlMain.BackColor          = white;
                _pnlGraph.BackColor         = white;
                _pnlDataArchive.BackColor   = white;
                _pnlSettingMain.BackColor   = white;
                menu_color();
            }
            catch (System.InvalidOperationException ex)
            {
                Custom_obj.write_log_file("Initialization", "Error 1------", ex.Message);          // call method to write error log
            }
            catch (Exception ex)
            {
                Custom_obj.write_log_file("Initialization", "Error 3------" , ex.Message);          // call method to write error log
            }
            
        }
        public static Main_Form _main;
        ManagementEventWatcher processStartEvent = new ManagementEventWatcher("SELECT * FROM Win32_ProcessStartTrace");
        ManagementEventWatcher processStopEvent = new ManagementEventWatcher("SELECT * FROM Win32_ProcessStopTrace");
        
        private void Form2_Load(object sender, EventArgs e)
        {
            int formHeight = this.Height;
            int formWidth = this.Width;
            //setFormSize(formHeight,formWidth);
            Custom_methods Custom_obj = new Custom_methods();
            _pnlHome.BackColor      = Custom_obj._get_color("white");
            _pnlHome.Visible        = true;
            _pnlMain.Visible        = false;
            _pnlGraph.Visible       = false;
            _pnlDataArchive.Visible    = false;
            _pnlSettingMain.Visible = false;

            /*_grph_ResponseTimeAnlysis.Size = new System.Drawing.Size(380, 293);
            _grphSenderAnlysis.Size = new System.Drawing.Size(380, 293);
            chart1.Size = new System.Drawing.Size(380, 293);
            chart5.Size = new System.Drawing.Size(380, 293);*/

            Graph_load();
            
            try
            {
                // get USER account Name
                String User_Account_Name = String.Empty;
                ManagementObjectSearcher searcher3 = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_UserAccount");
                foreach (ManagementObject wmi in searcher3.Get())
                {
                    try
                    {
                        User_Account_Name = wmi.GetPropertyValue("Name").ToString();
                    }
                    catch (Exception ex)
                    {
                        Custom_obj.write_log_file("User account ", "name error" , ex.Message);          // call method to write error log
                    }
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
                        //get_connectionString()
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
            catch (System.InvalidOperationException ex)
            {
                Custom_obj.write_log_file("Form Load ", "Error------1", ex.Message);          // call method to write error log
            }
            catch (Exception ex)
            {
                Custom_obj.write_log_file("Form Load ", "Error----3", ex.Message);          // call method to write error log
            }
        }
        void t_Tick(object sender, EventArgs e)
        {
            _lblTimer.Text = DateTime.Now.ToString();
        }

        void processStartEvent_EventArrived(object sender, EventArrivedEventArgs e)
        {
            try
            {
                string processName = e.NewEvent.Properties["ProcessName"].Value.ToString();
                string process_id = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value).ToString();
                Process localById = Process.GetProcessById(Int32.Parse(process_id));
                
                String file_path;
                if (!string.IsNullOrEmpty(localById.MainWindowTitle))
                {
                    Console.WriteLine("Name " + localById.MainWindowTitle);
                    if (!Environment.Is64BitProcess)
                    {
                        file_path = null;
                    }
                    else
                    {
                        file_path = localById.MainModule.FileName;
                    }
                    string Main_str;
                    Main_str = "INSERT INTO application_details(process_ID,process_name,start_time,Title) VALUES(" + localById.Id + ",'" + localById.ProcessName + "','" + localById.StartTime.ToString("yyyy-MM-dd HH:mm:ss") + "','" + localById.MainWindowTitle + "')";
                     Console.WriteLine(">>>>>> Query To Insert " + Main_str);
                    Custom_obj.execute_query(Main_str, "Process start","Error----2");
                }
            }
            catch (System.InvalidOperationException ex)
            {
                Custom_obj.write_log_file("Process start", "Error----1", ex.Message);          // call method to write error log
            }
            catch (Exception ex)
            {
                Custom_obj.write_log_file("Process start", "Error----3", ex.Message);          // call method to write error log
            }
        }

        void processStopEvent_EventArrived(object sender, EventArrivedEventArgs e)
        {
            string processID = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value).ToString();
            string str_Path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            try
            {
                string Main_str = "UPDATE Application_details SET end_time='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE process_ID=" + Int32.Parse(processID) + " AND end_time is null;";
                Custom_obj.execute_query(Main_str, " Update Process end time ", " Error----2");
            }
            catch (System.InvalidOperationException ex)
            {
                Custom_obj.write_log_file("Process Stop ", " Updation error-----1 ", ex.Message);          // call method to write error log
            }
            catch (Exception ex)
            {
                Custom_obj.write_log_file("Process Stop ", " Updation error----3 ", ex.Message);          // call method to write error log
            }
        }
        private void Main_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)         //code for to preventing close app
            {
                Hide();
                e.Cancel = true;
            }
            //HookManager.UnhookWinEvent(HookManager.windowEventHook);
        }
        private void _addSysTray_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Maximized;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Console.WriteLine("OOOOOOOOOO");
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel_register.Visible = true;
            panel_integrate.Visible = false;
            panel_agree.Visible = false;
            panel_welcome.Visible = false;
            String name = txtname.Text;
            String pwd = txtpassword.Text;
            String email = txtemail.Text;
            String con = Convert.ToInt32(txtphn_no.Text).ToString();
            String skype = txtskype.Text;
            String other = txtother_program.Text;
            Custom_obj.insertData(name,pwd,email,con,skype,other);

        }

        private void btnintegrate_Click(object sender, EventArgs e)
        {
            panel_register.Visible = false;
            panel_integrate.Visible = true;
            panel_agree.Visible = false;
            panel_welcome.Visible = false;
        }

        private void btnagree_Click(object sender, EventArgs e)
        {
            panel_register.Visible = false;
            panel_integrate.Visible = false;
            panel_agree.Visible = true;
            panel_welcome.Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel_register.Visible = false;
            panel_integrate.Visible = false;
            panel_agree.Visible = false;
            panel_welcome.Visible = true;
        }

        private void btnsetup_Click(object sender, EventArgs e)
        {
            //this.Dispose();
            Setup s = new Setup();
            s.Show();
            
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

        private static void outLookApp_NewMailEx(string EntryIDCollection)
        {
            Console.WriteLine("You've got a new mail whose EntryIDCollection is \n" + EntryIDCollection);
            Microsoft.Office.Interop.Outlook.Application app = new Microsoft.Office.Interop.Outlook.Application();
            Microsoft.Office.Interop.Outlook._NameSpace ns = app.GetNamespace("MAPI");
        }
        private static void Inspector_Send(object Item, ref bool Cancel)
        {
            Console.WriteLine("OUTLOOK IS START as NEW MAIL SEND..................."+ Item.ToString());
            Console.WriteLine("OUTLOOK IS START as NEW MAIL SEND..................." + Cancel);
        }
        private static void Inspector_read(object Item)
        {
            Console.WriteLine("OUTLOOK IS START as MAIL read..................." + Item.ToString());
        }
        private static void WindowEventCallback(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            Outlook.ApplicationClass oApp = null;
            Outlook.NameSpace mapiNamespace = null;
            Console.WriteLine("Inside Windows Wvent mmmmmmmmmmmmmmmm");
            Custom_methods Custom_obj = new Custom_methods();
            try
            {
                Console.WriteLine("inside tryyyyyyyyyyyyy");
                oApp = new Outlook.ApplicationClass();
                mapiNamespace = oApp.GetNamespace("MAPI");
                mapiNamespace.Logon(null, null, false, false);
                //private Outlook.Folder myAppointmentsFolder = (Outlook.Folder)myApplication.Session.GetDefaultFolder();
                Console.WriteLine("After oooooooooooooouuuuu");
                oApp.NewMailEx += new Outlook.ApplicationEvents_11_NewMailExEventHandler(outLookApp_NewMailEx);
                //oApp.NewMailEx += new Outlook.ApplicationEvents_11_NewMailExEventHandler(outLookApp_send);
                oApp.ItemSend += new Outlook.ApplicationEvents_11_ItemSendEventHandler(Inspector_Send);
                // oApp.ItemLoad += new Outlook.ItemEvents_10_ReadEventHandler(Inspector_read);

                /*cal_app = new Outlook.AppointmentItemClass();
                Outlook.MeetingItem myAppointmentItems = Outlook.OlDefaultFolders.olFolderCalendar;
                myAppointmentItems.ItemAdd += new Outlook.ItemsEvents_ItemAddEventHandler(myAppointmentItems_Add);
                myAppointmentItems.ItemChange += new Outlook.ItemsEvents_ItemChangeEventHandler(myAppointmentItems_Change);
                myAppointmentItems.ItemRemove += new Outlook.ItemsEvents_ItemRemoveEventHandler(myAppointmentItems_Remove);*/
                Console.WriteLine("TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT");
                IntPtr fg = GetForegroundWindow();
                uint pid;
                GetWindowThreadProcessId(hwnd, out pid);
                Process p = Process.GetProcessById((int)pid);
                if (!String.IsNullOrEmpty(p.MainWindowTitle))
                {
                    Console.WriteLine("ppppppppppppppppppppppppppppppppppppppp");
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
                        string app_sd_query="SELECT id FROM application_session_details WHERE app_detail_id=(select id from application_details where  process_ID=" + p.Id + ") AND end_time is null;";
                        string old_process_id = Custom_obj.get_query_data(app_sd_query, " Get Application session detail ");
                        string app_dt_query="SELECT id FROM application_details WHERE process_ID=" + p.Id + " AND end_time is null;";
                        string active_process_id = Custom_obj.get_query_data(app_dt_query, " Get Application Detail ");
                        Boolean app_type = false;
                        if (active_process_id != null)
                        {
                            app_type = true;
                        }
                        if (!app_type)
                        {
                            string inst_query="INSERT INTO application_details(process_ID,process_name,Title,start_time,location) VALUES(" + p.Id + ",'" + p.ProcessName + "','" + p.MainWindowTitle + "','" + p.StartTime.ToString("yyyy-MM-dd HH:mm:ss") + "','" + file_path + "')";
                            Custom_obj.execute_query(inst_query," Insert application detail "," --- 2 ");
                            string ap_detail="SELECT id FROM application_details WHERE process_ID=" + p.Id + " AND end_time is null;";
                            active_process_id=Custom_obj.get_query_data(app_sd_query, " Get Application detail to update end time ");
                        }
                        if (app_type)
                        {
                            //active_process_id_temp = active_process_id["id"].ToString();
                            if (old_process_id != null)
                            {
                                String today_date = String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
                                string up_query="UPDATE application_session_details SET end_time='" + today_date + "' WHERE end_time is null;";
                                Custom_obj.execute_query(up_query, " Update application session detail ", " Update end type ");
                                string inst_query="INSERT INTO application_session_details(app_detail_id,start_time,Title) VALUES(" + active_process_id + ",'" + today_date + "','" + p.MainWindowTitle + "')";
                                Custom_obj.execute_query(up_query, " Insert application session detail ", " Insert record ");
                            }
                        }
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        Custom_obj.write_log_file("Get Windows event process id", " Error 1 ", ex.Message);          // call method to write error log
                    }
                    catch (Exception ex)
                    {
                        Custom_obj.write_log_file("Get Windows event process id", " Error 3 ", ex.Message);          // call method to write error log
                    }
                    finally
                    {
                        Console.WriteLine("pppppppppppppppppppppppppppppppppppppp");
                        Get_browser_Url((int)pid, (String)p.ProcessName, hwnd);
                        Get_outlook_info((int)pid, (String)p.ProcessName, hwnd);
                    }
                }
            }
            catch (System.InvalidOperationException ex)
            {
                Custom_obj.write_log_file(" Windows event ", " Error 1", ex.Message);          // call method to write error log
            }
            catch (Exception ex)
            {
                Custom_obj.write_log_file(" Windows event ", " Error 3", ex.Message);          // call method to write error log
            }
            finally
            {
                mapiNamespace = null;
                oApp = null;
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
            Custom_methods Custom_obj = new Custom_methods();
            try
            {
                Boolean browse_bool = false;
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
                    else if ((String)proce.ProcessName == "firefox" || (String)proce.ProcessName == "opera")
                    {
                        Url = GetFirefoxUrl(proce, (String)proce.ProcessName);
                    }
                    else if ((String)proce.ProcessName == "iexplore")
                    {
                        Url = GetInternetExplorerUrl(proce);
                    }
                    else
                    {
                       today_date = String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
                        string qury="UPDATE Url_Info SET end_time='" + today_date + "' WHERE end_time is null;";
                        Custom_obj.execute_query(qury,"Update Url End time","Url qury --2");
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
                        Console.WriteLine("iiiiiiiiiiisssssssssss");
                        today_date = String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
                        string  query="SELECT id FROM Url_Info WHERE url='" + domain_url + "' AND end_time is null;";
                        string active_process_id = Custom_obj.get_query_data(query," Get Url Info wher end time is null");
                        string up_query="UPDATE Url_Info SET end_time='" + today_date + "' WHERE end_time is null;";
                        Custom_obj.execute_query(up_query, "Update Url End time", "Url qury --2 ");
                        string inst_query="INSERT INTO Url_Info(url,start_time) VALUES('" + domain_url + "','" + today_date + "')";
                        Custom_obj.execute_query(inst_query, "Insert Url ", "Url qury --2 ");
                        Console.WriteLine("iiiiiiiiiieeeeeeeeeeeeeee");
                        temp_Url = Url;
                    }
                    System.Threading.Thread.Sleep(250);
                }
            }
            catch (System.InvalidOperationException ex)
            {
                Custom_obj.write_log_file("Get browse Url", "Error 1", ex.Message);          // call method to write error log
            }
            catch (Exception ex)
            {
                Custom_obj.write_log_file("Get browse Url", "Error 3", ex.Message);          // call method to write error log
            }
        }
        public static string GetChromeUrl(Process process)
        {
            Custom_methods Custom_obj = new Custom_methods();
            try
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
            }
            catch(Exception ex)
            {
                Custom_obj.write_log_file(" Error in Chrome Url"," Error ",ex.Message);
            }
            return null;
        }
        public static string GetFirefoxUrl(Process process,string browser)
        {
            DdeClient dde = new DdeClient(browser, "WWW_GetWindowInfo");
            dde.Connect();
            string url = dde.Request("URL", int.MaxValue);
            dde.Disconnect();
            if (url != null)
                return url.Split(',')[0];
            else return null;
        }

        public static string GetInternetExplorerUrl(Process process)
        {
            Custom_methods Custom_obj = new Custom_methods();
            try
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
            catch (System.InvalidOperationException ex)
            {
                Custom_obj.write_log_file("InternetExplorer Url", "Error 1", ex.Message);          // call method to write error log
            }
            catch (Exception ex)
            {
                Custom_obj.write_log_file("InternetExplorer Url", "Error 3", ex.Message);          // call method to write error log
            }
            return "";
        }

        // Method to Get Outlook Details
        public static void Get_outlook_info(int proc_id, String proc_name, IntPtr hwnd)
        {
           // Console.WriteLine("");
            Custom_methods Custom_obj = new Custom_methods();
            try
            {
                //Boolean outlook_bool = false;
                //if (proc_name == "OUTLOOK") { Boolean outlook_bool = true; }
                uint pid1;
                IntPtr fg = GetForegroundWindow();
                GetWindowThreadProcessId(fg, out pid1);
                Process proce = Process.GetProcessById((int)pid1);
                if ((String)proce.ProcessName == "OUTLOOK")
                {
                    GetAllCalendarItems();
                    Console.WriteLine("OOOOOOOOOOOUuuuuuuuuuuuutttttttttttttttt");
                    Outlook.Application app = new Outlook.Application();
                    Outlook._NameSpace ns = app.GetNamespace("MAPI");
                    try
                    {
                        Outlook._Folders oFolders;
                        Outlook.MAPIFolder oPublicFolder = (Outlook.MAPIFolder)ns.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox).Parent;
                        oFolders = oPublicFolder.Folders;
                        Outlook._NameSpace abc = (Outlook._NameSpace)oPublicFolder.Parent;
                        Outlook._Folders oFolders1 = abc.Folders;
                        foreach (Outlook.MAPIFolder Folder in oFolders1)
                        {
                            string foldername = Folder.Name;
                            string query="SELECT id FROM outlook WHERE folder='" + foldername + "';";
                            string  main_folder = Custom_obj.get_query_data(query," Get Outlook folder error");
                            if (main_folder != "0")
                            {
                                string inst_query="INSERT INTO outlook(folder) VALUES('" + foldername + "')";
                                Custom_obj.execute_query(inst_query,"insert folder name"," error 2");
                            }
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
                        Console.WriteLine("befre");
                    }
                    catch (System.InvalidOperationException ex)
                    {
                        Custom_obj.write_log_file("Get outlook process Info", " Error 1 ", ex.Message);          // call method to write error log
                    }
                    catch (Exception ex)
                    {
                        Custom_obj.write_log_file("Get outlook process Info", " Error 3 ", ex.Message);          // call method to write error log
                    }
                    finally
                    {
                        ns = null;
                        app = null;
                    }
                }
                System.Threading.Thread.Sleep(500);
            }
            catch (System.InvalidOperationException ex)
            {
                Custom_obj.write_log_file("Get Outlook Info", " Error --1 ", ex.Message);          // call method to write error log
            }
            catch (Exception ex)
            {
                Custom_obj.write_log_file("Get Outlook Info", " Error --3 ", ex.Message);          // call method to write error log
            }
        }

        // Method to get Outlook Folder Detail
        public static void get_folder_detail(Outlook.MAPIFolder folder)
        {
            Custom_methods Custom_obj = new Custom_methods();
            try
            {
                if (folder.Folders.Count == 0)
                {
                    foreach (object item1 in folder.Items)
                    {
                        Outlook.MailItem mailItem = item1 as Outlook.MailItem;
                        if (mailItem != null)
                        {
                            if (mailItem.Subject != null)
                            {
                                string query_ib="SELECT id,unread_bool FROM outlook_mail_inbox WHERE entry_id='" + mailItem.EntryID + "';";
                                List<string> main_folder = Custom_obj.get_query_data_list(query_ib,"Outlook Inbox");
                                if (main_folder == null)
                                {
                                    int main_id, parent_id;
                                   // main_folder.Close();
                                    Outlook.MAPIFolder parent_fname = (Outlook.MAPIFolder)folder.Parent;
                                    string query="SELECT id FROM outlook WHERE folder='" + parent_fname.Name + "';";
                                    string main_folder1 = Custom_obj.get_query_data(query_ib, "Outlook Folder ");
                                    if (main_folder1 != null)
                                    {
                                        main_id = Int32.Parse(main_folder1);
                                    }
                                    else main_id = 0;
                                   // main_folder1.Close();
                                    string query_mail="SELECT id FROM outlook_mail WHERE folder_name='" + folder.Name + "';";
                                    string main_folder2 = Custom_obj.get_query_data(query_ib, "Outlook Mail ");
                                    if (main_folder2 != null)
                                    {
                                        parent_id = Int32.Parse(main_folder2);
                                    }
                                    else parent_id = 0;
                                   // main_folder1.Close();
                                    int a = 1;
                                    if (mailItem.UnRead == false)
                                    {
                                        a = 0;
                                    }
                                    //mailItem.Recipients;
                                    string insrt_query_ib="INSERT INTO outlook_mail_inbox(mail_box_id, received_time, subject, outlook_mail_id, no_of_attachment,parent_folder_name,entry_id,unread_bool) VALUES(" + parent_id + ",'" + String.Format("{0:yyyy-MM-dd HH:mm:ss}", mailItem.ReceivedTime) + "','" + mailItem.Subject.Replace("'", "") + "','" + main_id + "','" + mailItem.Attachments.Count + "','" + folder.Name + "','" + mailItem.EntryID + "'," + a.ToString() + ")";
                                    Custom_obj.execute_query(insrt_query_ib, "Insert Outlook Mail Inbox"," Error --2");
                                }
                                else
                                {
                                    Boolean unread_bool = true ; //main_folder[0];
                                    int unread_mail_id = 0;//main_folder.GetInt32(0);
                                    Boolean mail_unread = mailItem.UnRead;
                                    //main_folder.Close();
                                    String today_date = String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);

                                    Console.WriteLine("UPDATE outlook_mail_inbox SET read_time='" + today_date + "' WHERE id=" + unread_mail_id + ")");
                                    if (mail_unread == false && unread_bool == true)
                                    {
                                        string up_query="UPDATE outlook_mail_inbox SET read_time='" + today_date + "', unread_bool=False WHERE id=" + unread_mail_id + ";";
                                        Custom_obj.execute_query(up_query,"Update read time for mail"," --- error");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (Outlook.MAPIFolder subFolder in folder.Folders)
                    {
                        String foldername = subFolder.Name;
                        string main_id;
                        string query_mail="SELECT id FROM outlook_mail WHERE folder_name='" + foldername + "';";
                        string main_folder = Custom_obj.get_query_data(query_mail, "Outlook Mail ");
                        if (main_folder == null)
                        {
                            string query="SELECT id FROM outlook WHERE folder='" + folder.Name + "';";
                            string main_folder1 = Custom_obj.get_query_data(query, "Outlook Folder");
                            if (main_folder1 != null)
                            {
                                main_id = main_folder1;
                            }
                            else main_id = "0";
                            string inst_query_mail="INSERT INTO outlook_mail(folder_name,outlook_id,parent_folder_name) VALUES('" + foldername + "'," + main_id + ",'" + folder.Name + "')";
                            Custom_obj.execute_query(inst_query_mail, "Insert outlook_mail", " --- error");
                        }
                        get_folder_detail(subFolder);
                    }
                }
            }
            catch (System.InvalidOperationException ex)
            {
                Custom_obj.write_log_file("Get outlook folder detail", " Error --1 ", ex.Message);          // call method to write error log
            }
            catch (Exception ex)
            {
                Custom_obj.write_log_file("Get outlook folder detail", " Error --3 ", ex.Message);          // call method to write error log
            }
        }
        public static void GetAllCalendarItems()
        {
            Custom_methods Custom_obj = new Custom_methods();
            try
            {
                Outlook.Application oApp = new Outlook.Application();
                Outlook._NameSpace oNS = oApp.GetNamespace("MAPI");
                Microsoft.Office.Interop.Outlook.MAPIFolder oCalenderFolder = oNS.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderCalendar);
                Microsoft.Office.Interop.Outlook.Items outlookCalendarItems = oCalenderFolder.Items;
                outlookCalendarItems.IncludeRecurrences = true;
                foreach (Microsoft.Office.Interop.Outlook.AppointmentItem item in outlookCalendarItems)
                {
                    string query_ib = "SELECT id FROM calender WHERE entry_id='" + item.EntryID.ToString() + "';";
                    string main_id = Custom_obj.get_query_data(query_ib, "calender");
                    if (main_id == "0")
                    {
                         String inst_cal = @"INSERT INTO calender(entry_id,subject,body,location,start_time,end_time,creation_date,IsRecurring,ModificationTime,MeetingStatus) 
                            VALUES('" + item.EntryID.ToString() + "','" +item.Subject.ToString()+"','"+item.Body.ToString()+"','"+item.Location.ToString()+
                            @"','"+String.Format("{0:yyyy-MM-dd HH:mm:ss}", item.Start)+"','"+String.Format("{0:yyyy-MM-dd HH:mm:ss}",item.End)+
                            @"','"+String.Format("{0:yyyy-MM-dd HH:mm:ss}", item.CreationTime)+"',"+item.IsRecurring.ToString()+ 
                            @",,'" + String.Format("{0:yyyy-MM-dd HH:mm:ss}", item.LastModificationTime) + "','"+item.MeetingStatus.ToString()+"'";
                        Custom_obj.execute_query(inst_cal, "Insert outlook_mail", " --- error");
                    }
                }
            }
            catch (Exception ex)
            {
                Custom_obj.write_log_file("Get outlook Calender detail", " Error --3 ", ex.Message);          // call method to write error log
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
}

