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
    public partial class Setup : Form
    {
        public Setup()
        {
            InitializeComponent();
            panel_self.Visible = true;
            panel_team.Visible = false;
            panel_other.Visible = false;
            panel_client.Visible = false;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel_self.Visible = true;
            panel_team.Visible = false;
            panel_other.Visible = false;
            panel_client.Visible = false;
        }

        private void btnteam_Click(object sender, EventArgs e)
        {
            panel_self.Visible = false;
            panel_team.Visible = true;
            panel_other.Visible = false;
            panel_client.Visible = false;
        }

        private void btnclient_Click(object sender, EventArgs e)
        {
            panel_self.Visible = false;
            panel_team.Visible = false;
            panel_other.Visible = false;
            panel_client.Visible = true;
        }

        private void btnother_Click(object sender, EventArgs e)
        {
            panel_self.Visible = false;
            panel_team.Visible = false;
            panel_other.Visible = true;
            panel_client.Visible = false;
        }

        private void btntutorial_Click(object sender, EventArgs e)
        {
            panel_self.Visible = false;
            panel_team.Visible = false;
            panel_other.Visible = false;
            panel_client.Visible = false;
        }

        private void btnsignup_Click(object sender, EventArgs e)
        {
            panel_self.Visible = false;
            panel_team.Visible = false;
            panel_other.Visible = false;
            panel_client.Visible = false;
        }

        private void btnproject_Click(object sender, EventArgs e)
        {
            project p = new project();
            p.Show();
            this.Dispose();
        }
    }
}
