using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace SysInfo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Process[] process = Process.GetProcessesByName(Application.ProductName); //Prevent multiple instance           
            if (process.Length > 1)
            {
                MessageBox.Show("{Application Name}  is already running. This instance will now close.", "{Application Name}",MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
            else
            {
                Application.Run(new Feed());
            }
        }
    }
}
