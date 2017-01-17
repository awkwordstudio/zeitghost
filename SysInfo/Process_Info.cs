using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using NDde.Client;
using System.IO;
using Microsoft.Win32;

namespace SysInfo
{
    public partial class Process_Info : Form
    {
        public Process_Info()
        {
            InitializeComponent();
        }
        public Process_Info(string process_id)
        {
            InitializeComponent();
            button1.Visible = false;
            dateTimePicker1.Visible = false;
            listView1.Visible = false;

            if (process_id == "1")
            {
                listView1.Visible = true;
                system_information(); 
            }
            if (process_id == "2")
            {
                listView1.Visible = true;
                running_process();
            }
            if (process_id == "3")
            {
                button1.Visible = true;
                dateTimePicker1.Visible = true;
                outlook_meeting_info();
            }
        }
        private void Process_Info_Load(object sender, EventArgs e)
        {
            
        }
        private void running_process()
        {

            listView1.Clear();
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("ID");
            listView1.Columns.Add("name");
            listView1.Columns.Add("Title");
            listView1.Columns.Add("Time");
            listView1.Columns.Add("Path");
            Process[] processlist = Process.GetProcesses();
            try
            {
                foreach (Process theprocess in processlist)
                {
                    if (string.IsNullOrEmpty(theprocess.MainWindowTitle) != true)
                    {
                        var p_id = theprocess.Id.ToString();
                        var name = theprocess.ProcessName;
                        var title = theprocess.MainWindowTitle;
                        DateTime dt = DateTime.Parse(theprocess.StartTime.ToString());
                        var path = theprocess.MainModule.FileName;
                        Get_paht();
                        listView1.Items.Add(new ListViewItem(new string[] { p_id, name, title, dt.ToString("HH:mm:ss"), path }));
                    }
                }
                listView1.Sorting = SortOrder.Descending;
            }
            catch { }
        }
        private void Get_paht()
        {
            string filename = "test.txt";
            //string filePath = AppDomain.CurrentDomain.BaseDirectory + filename;
            //string path = Path.GetFullPath(filename);
            string path = Path.GetDirectoryName(filename);
            Console.WriteLine(path);
        }
        private void system_information()
        {
            listView1.Clear();
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("Name");
            listView1.Columns.Add("Information");
            string[] arr = new string[2];
            ListViewItem item;

            try
            {
                // Get Computer Name
                ManagementClass mc1 = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc1 = mc1.GetInstances();
                String info = String.Empty;
                foreach (ManagementObject mo in moc1)
                {
                    info = (string)mo["Name"];
                }
                arr[0] = "Computer Name ";
                arr[1] = info;
                item = new ListViewItem(arr);
                listView1.Items.Add(item);

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
                arr[0] = "Active User Name ";
                arr[1] = User_Account_Name;
                item = new ListViewItem(arr);
                listView1.Items.Add(item);

                // Get Log in Time
                String log_in = String.Empty;
                ManagementScope ms = new ManagementScope("\\root\\cimv2");
                ObjectQuery oq = new ObjectQuery("Select * from Win32_Session");
                ManagementObjectSearcher query = new ManagementObjectSearcher(ms, oq);
                ManagementObjectCollection queryCollection = query.Get();
                foreach (ManagementObject mo in queryCollection)
                {
                    if (mo["LogonType"].ToString().Equals("2")) //  2 - for logged on User
                    {
                        log_in = mo["StartTime"].ToString();
                    }
                }

                arr[0] = "Log in Time ";
                arr[1] = log_in;
                item = new ListViewItem(arr);
                listView1.Items.Add(item);

                // Get Operating System Info
                String bios = String.Empty;
                ManagementObjectSearcher searcher1 = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
                foreach (ManagementObject wmi in searcher1.Get())
                {
                    try
                    {
                        bios = ((string)wmi["Caption"]).Trim() + ", " + (string)wmi["Version"] + ", " + (string)wmi["OSArchitecture"];
                    }
                    catch { }
                }
                arr[0] = "OS Information ";
                arr[1] = bios;
                item = new ListViewItem(arr);
                listView1.Items.Add(item);

                // Get Processor Info
                ManagementClass mc = new ManagementClass("win32_processor");
                ManagementObjectCollection moc = mc.GetInstances();
                String Id = String.Empty;
                String cpuMan = String.Empty;
                int cpuClockSpeed;
                double? GHz = null;
                String info1 = String.Empty;

                foreach (ManagementObject mo in moc)
                {
                    string name = (string)mo["Name"];
                    name = name.Replace("(TM)", "™").Replace("(tm)", "™").Replace("(R)", "®").Replace("(r)", "®").Replace("(C)", "©").Replace("(c)", "©").Replace("    ", " ").Replace("  ", " ");

                    info1 = name + ", " + (string)mo["Caption"] + ", " + (string)mo["SocketDesignation"];
                    arr[0] = "Processor Information ";
                    arr[1] = info1;
                    item = new ListViewItem(arr);
                    listView1.Items.Add(item);

                    Id = mo.Properties["processorID"].Value.ToString();              // get Processor ID 
                    arr[0] = "Processor ID  ";
                    arr[1] = Id;
                    item = new ListViewItem(arr);
                    listView1.Items.Add(item);

                    cpuMan = mo.Properties["Manufacturer"].Value.ToString();        // manufacturer of CPU
                    arr[0] = "CPU Manufacturer ";
                    arr[1] = cpuMan;
                    item = new ListViewItem(arr);
                    listView1.Items.Add(item);

                    cpuClockSpeed = Convert.ToInt32(mo.Properties["CurrentClockSpeed"].Value.ToString());   // CPU Status
                    arr[0] = "Current ClockSpeed ";
                    arr[1] = cpuClockSpeed.ToString();
                    item = new ListViewItem(arr);
                    listView1.Items.Add(item);

                    GHz = 0.001 * (UInt32)mo.Properties["CurrentClockSpeed"].Value;
                    arr[0] = "ClockSpeed in GHz ";
                    arr[1] = GHz.ToString() + " GHz";
                    item = new ListViewItem(arr);
                    listView1.Items.Add(item);

                    break;
                }

                // Get HDD info
                ManagementClass mangnmt = new ManagementClass("Win32_LogicalDisk");
                ManagementObjectCollection mcol = mangnmt.GetInstances();
                string result = "";
                foreach (ManagementObject strt in mcol)
                {
                    result += Convert.ToString(strt["VolumeSerialNumber"]);
                }
                arr[0] = "HDD serial No. ";
                arr[1] = result;
                item = new ListViewItem(arr);
                listView1.Items.Add(item);

                //get MAC Address
                ManagementClass mc4 = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc4 = mc4.GetInstances();
                string MACAddress = String.Empty;
                foreach (ManagementObject mo in moc4)
                {
                    if (MACAddress == String.Empty)
                    {
                        if ((bool)mo["IPEnabled"] == true)
                            MACAddress = mo["MacAddress"].ToString();
                    }
                    mo.Dispose();
                }
                MACAddress = MACAddress.Replace(":", "");

                arr[0] = "MAC Address ";
                arr[1] = MACAddress;
                item = new ListViewItem(arr);
                listView1.Items.Add(item);

                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
                String Board_Maker = String.Empty;
                String Product = String.Empty;
                foreach (ManagementObject wmi in searcher.Get())
                {
                    try
                    {
                        Board_Maker = wmi.GetPropertyValue("Manufacturer").ToString();      // get mother board maker
                        arr[0] = "Mother Board Maker ";
                        arr[1] = Board_Maker;
                        item = new ListViewItem(arr);
                        listView1.Items.Add(item);

                        Product = wmi.GetPropertyValue("Product").ToString();               // ge product
                        arr[0] = "Product ";
                        arr[1] = Product;
                        item = new ListViewItem(arr);
                        listView1.Items.Add(item);
                    }
                    catch { }
                }

                String CD_DVD_Drive_Path = String.Empty;
                ManagementObjectSearcher searcher7 = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_CDROMDrive");
                foreach (ManagementObject wmi in searcher7.Get())
                {
                    try
                    {
                        CD_DVD_Drive_Path = wmi.GetPropertyValue("Drive").ToString();
                        arr[0] = "CD-DVD Drive Path: ";
                        arr[1] = CD_DVD_Drive_Path;
                        item = new ListViewItem(arr);
                        listView1.Items.Add(item);
                    }
                    catch { }
                }

                //get BIOS info
                String BIOS_Maker = String.Empty;
                String BIOS_Serial_No = String.Empty;
                String caption = String.Empty;
                String current_lang = String.Empty;
                ManagementObjectSearcher searcher6 = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");
                foreach (ManagementObject wmi in searcher6.Get())
                {
                    try
                    {
                        BIOS_Maker = wmi.GetPropertyValue("Manufacturer").ToString();
                        arr[0] = "BIOS Maker: ";
                        arr[1] = BIOS_Maker;
                        item = new ListViewItem(arr);
                        listView1.Items.Add(item);

                        BIOS_Serial_No = wmi.GetPropertyValue("SerialNumber").ToString();
                        arr[0] = "BIOS Serial No: ";
                        arr[1] = BIOS_Serial_No;
                        item = new ListViewItem(arr);
                        listView1.Items.Add(item);

                        caption = wmi.GetPropertyValue("Caption").ToString();
                        arr[0] = "BIOS Caption: ";
                        arr[1] = caption;
                        item = new ListViewItem(arr);
                        listView1.Items.Add(item);

                        current_lang = wmi.GetPropertyValue("CurrentLanguage").ToString();
                        arr[0] = "Current Language: ";
                        arr[1] = current_lang;
                        item = new ListViewItem(arr);
                        listView1.Items.Add(item);
                    }
                    catch { }
                }

                //Get RAM info
                ManagementScope oMs = new ManagementScope();
                ObjectQuery oQuery = new ObjectQuery("SELECT * FROM Win32_PhysicalMemory");
                ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oMs, oQuery);
                ManagementObjectCollection oCollection = oSearcher.Get();

                long MemSize = 0;
                long mCap = 0;
                foreach (ManagementObject obj in oCollection)           // In case more than one Memory sticks are installed
                {
                    mCap = Convert.ToInt64(obj["Capacity"]);
                    MemSize += mCap;
                }
                MemSize = (MemSize / 1024) / 1024;
                arr[0] = "Memory Size: ";
                arr[1] = MemSize.ToString() + "MB \n";
                item = new ListViewItem(arr);
                listView1.Items.Add(item);

                int MemSlots = 0;
                ManagementScope oMs1 = new ManagementScope();
                ObjectQuery oQuery2 = new ObjectQuery("SELECT MemoryDevices FROM Win32_PhysicalMemoryArray");
                ManagementObjectSearcher oSearcher2 = new ManagementObjectSearcher(oMs1, oQuery2);
                ManagementObjectCollection oCollection2 = oSearcher2.Get();
                foreach (ManagementObject obj in oCollection2)
                {
                    MemSlots = Convert.ToInt32(obj["MemoryDevices"]);

                }
                arr[0] = "Memory Slots: ";
                arr[1] = MemSlots.ToString() + "MB \n";
                item = new ListViewItem(arr);
                listView1.Items.Add(item);

                //get Network info
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    foreach (UnicastIPAddressInformation ip in nic.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            arr[0] = "Default IP Gateway";
                            arr[1] = ip.Address.ToString();
                            item = new ListViewItem(arr);
                            listView1.Items.Add(item);
                        }
                    }
                }
            }
            catch { }
        
        }
        private void outlook_meeting_info()
        {
            listView1.Clear();
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("Header");
            listView1.Columns.Add("Values");
            String[] arr = new String[2];
            ListViewItem lm;

            //Outlook.Application appl=new Outlook.Application();
            Microsoft.Office.Interop.Outlook._Application oApp = null;
            Microsoft.Office.Interop.Outlook.NameSpace mapiNamespace = null;
            Microsoft.Office.Interop.Outlook.MAPIFolder CalendarFolder = null;
            //Microsoft.Office.Interop.Outlook.MAPIFolder Inbox = null;
            Microsoft.Office.Interop.Outlook.Items outlookCalendarItems = null;

            // Microsoft.Office.Interop.Outlook.PostItem item1 = null;
            //Microsoft.Office.Interop.Outlook.MAPIFolder inboxFolder = null;

            try
            {
                oApp = new Microsoft.Office.Interop.Outlook.Application();
                mapiNamespace = oApp.GetNamespace("MAPI");
                mapiNamespace.Logon("", "", true, true);

               // CalendarFolder = mapiNamespace.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderCalendar);
                CalendarFolder = oApp.Session.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderCalendar);
                DateTime startTime = DateTime.Now;
                DateTime endTime = startTime.AddDays(5);
                //string filter = "[Start] >= '"  + startTime.ToString("g")  + "' AND [End] <= '" + endTime.ToString("g") + "'";
                outlookCalendarItems = CalendarFolder.Items;
                // outlookCalendarItems.Restrict(filter);
                // outlookCalendarItems.Sort("Start");
                outlookCalendarItems.IncludeRecurrences = true;
                int Count = 0 ;
                foreach (Microsoft.Office.Interop.Outlook.AppointmentItem item in outlookCalendarItems)
                {
                    if (item.Subject != null)
                    {
                        DateTime dt; 

                        dt = dateTimePicker1.Value.Date;
                        string theDate = dateTimePicker1.Value.ToShortDateString();
                        if (item.Start >= dt ) //&& item.End >= dt)
                        {
                            Count += 1;
                            arr[0] = "Meeting " + Count.ToString();
                            arr[1] = "";
                            lm = new ListViewItem(arr);
                            listView1.Items.Add(lm); 

                            arr[0] = "Subject";
                            arr[1] = item.Subject;
                            lm = new ListViewItem(arr);
                            listView1.Items.Add(lm);

                            arr[0] = "Location";
                            arr[1] = item.Location;
                            lm = new ListViewItem(arr);
                            listView1.Items.Add(lm);

                            arr[0] = "Start Time";
                            arr[1] = item.Start.ToString();
                            lm = new ListViewItem(arr);
                            listView1.Items.Add(lm);

                            arr[0] = "End Time";
                            arr[1] = item.End.ToString();
                            lm = new ListViewItem(arr);
                            listView1.Items.Add(lm);

                            arr[0] = "Body";
                            arr[1] = item.Body;
                            lm = new ListViewItem(arr);
                            listView1.Items.Add(lm);
                        }
                        
                    }
                }
               // Microsoft.Office.Interop.Outlook.MAPIFolder inbox = this.Application.ActiveExplorer().Session.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderInbox);

                //Microsoft.Office.Interop.Outlook.Items unreadItems = inbox.Items.Restrict("[Unread]=true");

               // MessageBox.Show(string.Format("Unread items in Inbox = {0}", unreadItems.Count));
                
               // inboxFolder = mapiNamespace.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderInbox);
               // Console.WriteLine("Folders: {0}", inboxFolder1.Folders.Count);

               // inboxFolder = mapiNamespace.Folders["my-account@myserver.com"].Folders["Inbox"];

               /* foreach (Microsoft.Office.Interop.Outlook.MailItem mailItem in inboxFolder.Items)
                {
                    if (mailItem.UnRead) // I only process the mail if unread
                    {
                        Console.WriteLine("Accounts: {0}", mailItem.Body);
                    }
                }*/
            }
            catch
            {
                Console.WriteLine("HHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH");
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Visible = true;
            //outlook_meeting_info();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //gggg
        }
        private void refresh_1(string process_id)
        {
            listView1.Clear();
            listView1.View = View.Details;
            listView1.Columns.Add("ID");
            listView1.Columns.Add("name");
            listView1.Columns.Add("Title");
            listView1.Columns.Add("Time");
            listView1.Columns.Add("Path");
            listView1.Columns.Add("URL");
            //Process processlist = Process.GetProcessById(process_id);

            Console.Write("22222222222222222222222222222jjjj " + process_id + " 55555 ");
            Process[] localByName = Process.GetProcessesByName(process_id);
            foreach (Process theprocess in localByName)
            {
                string url = null;
                var p_id = theprocess.Id.ToString();
                var name = theprocess.ProcessName;
                var title = theprocess.MainWindowTitle;
                DateTime dt = DateTime.Parse(theprocess.StartTime.ToString());
                var path = theprocess.MainModule.FileName;
                if (theprocess.ProcessName == "firefox")
                    url = GetBrowserURL("firefox");
                listView1.Items.Add(new ListViewItem(new string[] { p_id, name, title, dt.ToString("HH:mm:ss"), path, url }));
                listView1.FullRowSelect = true;
            }
            listView1.Sorting = SortOrder.Descending;
        }
        private string GetBrowserURL(string browser)
        {
            try
            {
                DdeClient dde = new DdeClient(browser, "WWW_GetWindowInfo");
                dde.Connect();
                string url = dde.Request("URL", int.MaxValue);
                string[] text = url.Split(new string[] { "\",\"" }, StringSplitOptions.RemoveEmptyEntries);
                //string text = dde.Request("URL", int.MaxValue); 
                dde.Disconnect();
                return text[0].Substring(1);
            }
            catch
            {
                return null;
            }
        }
        private void outlook()
        {
            Microsoft.Office.Interop.Outlook.Application app = null;
            Microsoft.Office.Interop.Outlook._NameSpace ns = null;
            Microsoft.Office.Interop.Outlook.PostItem item = null;
            Microsoft.Office.Interop.Outlook.MAPIFolder inboxFolder = null;
            Microsoft.Office.Interop.Outlook.MAPIFolder subFolder = null;

            try
            {
                app = new Microsoft.Office.Interop.Outlook.Application();
                ns = app.GetNamespace("MAPI");
                ns.Logon(null, null, false, false);

                inboxFolder = ns.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderInbox);
                subFolder = inboxFolder.Folders["MySubFolderName"]; //folder.Folders[1]; also works
                Console.WriteLine("Folder Name: {0}, EntryId: {1}", subFolder.Name, subFolder.EntryID);
                Console.WriteLine("Num Items: {0}", subFolder.Items.Count.ToString());

                for (int i = 1; i <= subFolder.Items.Count; i++)
                {
                    item = (Microsoft.Office.Interop.Outlook.PostItem)subFolder.Items[i];
                    Console.WriteLine("Item: {0}", i.ToString());
                    Console.WriteLine("Subject: {0}", item.Subject);
                    //  Console.WriteLine("Sent: {0} {1}" item.SentOn.ToLongDateString(), item.SentOn.ToLongTimeString());
                    Console.WriteLine("Categories: {0}", item.Categories);
                    Console.WriteLine("Body: {0}", item.Body);
                    Console.WriteLine("HTMLBody: {0}", item.HTMLBody);
                }
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                ns = null;
                app = null;
                //inbox;
            }
        }
        public struct gridData
        {
            private string make;
            private int year;

            public gridData(string n, int y)
            {
                make = n;
                year = y;
            }

            public string Make
            {
                get { return make; }
                set { make = value; }
            }

            public int Year
            {
                get { return year; }
                set { year = value; }
            }
        }
        //const int WM_KEYDOWN = 0x100;
       // const int WM_SYSKEYDOWN = 0x104;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int WM_KEYDOWN = 0x100;
            const int WM_SYSKEYDOWN = 0x104;

            if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
            {
                switch (keyData)
                {
                    case Keys.Down:
                        this.Parent.Text = "Down Arrow Captured";
                        break;

                    case Keys.Up:
                        this.Parent.Text = "Up Arrow Captured";
                        break;

                    case Keys.Tab:
                        this.Parent.Text = "Tab Key Captured";
                        break;

                    case Keys.Control | Keys.M:
                        this.Parent.Text = "<CTRL> + M Captured";
                        break;

                    case Keys.Alt | Keys.Z:
                        this.Parent.Text = "<ALT> + Z Captured";
                        break;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void ResizeListViewColumns(ListView lv)
        {
            foreach (ColumnHeader column in lv.Columns)
            {
                column.Width = -2;
            }
        }
        
        private void SetStartup()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            /*if (chkStartUp.Checked)
                rk.SetValue(AppName, Application.ExecutablePath.ToString());
            else
                rk.DeleteValue(AppName,false);     */       

        }
    }
}
