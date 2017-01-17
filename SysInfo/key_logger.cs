using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SysInfo
{
    public partial class key_logger : Form
    {
        public key_logger()
        {
            InitializeComponent();
        }
        private void form1_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine("ttttttttttttttttttttttttttttttttttttttttttttttttttt", e.KeyCode);
            if (e.KeyCode == Keys.F2)
                this.Close();
        }

        private void key_logger_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
        }
    }
}
