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
    public partial class project : Form
    {
        public project()
        {
            InitializeComponent();
            panel_project.Visible = true;
            panel_app.Visible = false;
            panel_meeting.Visible = false;
            panel_web.Visible = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void panel_project_Paint(object sender, PaintEventArgs e)
        {
            panel_project.Visible = true;
            panel_app.Visible = false;
            panel_meeting.Visible = false;
            panel_web.Visible = false;
        }

        private void btnapp_Click(object sender, EventArgs e)
        {
            panel_project.Visible = false;
            panel_app.Visible = true;
            panel_meeting.Visible = false;
            panel_web.Visible = false;
        }

        private void btnweb_Click(object sender, EventArgs e)
        {
            panel_project.Visible = false;
            panel_app.Visible = false;
            panel_meeting.Visible = false;
            panel_web.Visible = true;
        }

        private void btnmeeting_Click(object sender, EventArgs e)
        {
            panel_project.Visible = false;
            panel_app.Visible = false;
            panel_meeting.Visible = true;
            panel_web.Visible = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel_project.Visible = false;
            panel_app.Visible = false;
            panel_meeting.Visible = false;
            panel_web.Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel_project.Visible = false;
            panel_app.Visible = false;
            panel_meeting.Visible = false;
            panel_web.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel_project.Visible = false;
            panel_app.Visible = false;
            panel_meeting.Visible = false;
            panel_web.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel_project.Visible = false;
            panel_app.Visible = false;
            panel_meeting.Visible = false;
            panel_web.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel_project.Visible = false;
            panel_app.Visible = false;
            panel_meeting.Visible = false;
            panel_web.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel_project.Visible = false;
            panel_app.Visible = false;
            panel_meeting.Visible = false;
            panel_web.Visible = false;
        }
    }
}
