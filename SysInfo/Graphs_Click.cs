using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace SysInfo
{
    public partial class Main_Form
    {
        private void _move_graph(object sender)
        {
            List<Control> _glist = new List<Control>();
            List<Control> _gForm = new List<Control>();
            Control cur_ctr = (Control)sender;
            foreach (Control c in _pnlGraphView.Controls)       // get graph from View
            {
                if (c.Name == cur_ctr.Name)
                    _gForm.Add(c);
                else
                    _glist.Add(c);
            }
            _pnlGraphView.Controls.Clear();

            foreach (Control c in _pnlGraphList.Controls)       // get graph from list
            {
                if (c.Name == cur_ctr.Name)
                    _gForm.Add(c);
                else
                    _glist.Add(c);
            }

            _pnlGraphList.Controls.Clear();
            int Y = 10;
            foreach (Control control in _gForm)
            {
                _pnlGraphView.Controls.Add(control);        // add graph in View
                control.Location = new Point(20, Y);
                control.Size= new System.Drawing.Size(850, 500);
            }
            Y = 10;
            foreach (Control control in _glist)
            {
                _pnlGraphList.Controls.Add(control);       // add graph in List
                control.Location = new Point(10, Y);
                control.Size = new System.Drawing.Size(240, 280);
                Y += control.Height + 5;
            }
        }
        // On Click Graphs
        private void _grph_1_Click(object sender, EventArgs e)
        {
            string heading = "";
            if (Load_type == "mail")
                heading="EMAIL DASHBOARD | TIME ANALYSIS";
            else if (Load_type == "meeting")
                heading = "MEETING DASHBOARD | TIME ANALYSIS";
            else if (Load_type == "browser")
                heading = "WEB DASHBOARD | TIME ANALYSIS";
            else if (Load_type == "project")
                heading = "DELIVERABLE DASHBOARD | TIME ANALYSIS";
            else if (Load_type == "user")
                heading = "USER & SYSTEM DASHBOARD | TIME ANALYSIS";
            else if (Load_type == "system")
                heading = "COMMUNICATION DASHBOARD | TIME ANALYSIS";

            _lblGrphHeader.Text = heading;
            _grph_1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 6.0F);
            _move_graph(sender);
        }

        private void _grph_2_Click(object sender, EventArgs e)
        {
            
            string heading = "";
            if (Load_type == "mail")
                heading = "EMAIL DASHBOARD | RECIPITENT ANALYSIS";
            else if (Load_type == "meeting")
                heading = "MEETING DASHBOARD | SCHEDULING ANALYSIS";
            else if (Load_type == "browser")
                heading = "WEB DASHBOARD | WEBSITE ANALYSIS";
            else if (Load_type == "project")
                heading = "DELIVERABLE DASHBOARD | FILE TIME ANALYSIS";
            else if (Load_type == "user")
                heading = "USER & SYSTEM DASHBOARD | MUSIC ACTIVITY ANALYSIS";
            else if (Load_type == "system")
                heading = "COMMUNICATION DASHBOARD | PEOPLE ANALYSIS";

            _lblGrphHeader.Text = heading; 
            _grph_1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 6.0F);
            _move_graph(sender);
        }

        private void _grph_3_Click(object sender, EventArgs e)
        {
            string heading = "";
            if (Load_type == "mail")
                heading = "EMAIL DASHBOARD | INBOX ANALYSIS";
            else if (Load_type == "meeting")
                heading = "MEETING DASHBOARD | UPDATED MEETING ANALYSIS";
            else if (Load_type == "browser")
                heading = "WEB DASHBOARD | TAB ANALYSIS";
            else if (Load_type == "project")
                heading = "DELIVERABLE DASHBOARD | FILE ACTIVITY ANALYSIS";
            else if (Load_type == "user")
                heading = "USER & SYSTEM DASHBOARD | DISTRACTION ANALYSIS";
            else if (Load_type == "system")
                heading = "COMMUNICATION DASHBOARD | APP & ACTIVITY ANALYSIS";

            _lblGrphHeader.Text = heading;
            _grph_1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 7.5F);
            _move_graph(sender);
        }

        private void _grph_4_Click(object sender, EventArgs e)
        {
            string heading = "";
            if (Load_type == "mail")
                heading = "EMAIL DASHBOARD | OUTBOX ANALYSIS";
            else if (Load_type == "browser")
                heading = "WEB DASHBOARD | WEBSITE LINAKGE ANALYSIS";
            else if (Load_type == "project")
                heading = "DELIVERABLE DASHBOARD | FILE ACTIVITY USE  ANALYSIS";
            else if (Load_type == "user")
                heading = "USER & SYSTEM DASHBOARD | PRODUCTIVITY ANALYSIS";

            _lblGrphHeader.Text = heading;
            _move_graph(sender);
        }

        private void _grph_5_Click(object sender, EventArgs e)
        {
            string heading = "";
            if (Load_type == "mail")
                heading = "EMAIL DASHBOARD | RESPONCE TIME ANALYSIS";
            else if (Load_type == "project")
                heading = "DELIVERABLE DASHBOARD | PROJECT INCREMENT ANALYSIS";
            else if (Load_type == "user")
                heading = "USER & SYSTEM DASHBOARD | APP LINKAGE ANALYSIS";

            _lblGrphHeader.Text = heading;
            _move_graph(sender);
        }

        private void _grph_6_Click(object sender, EventArgs e)
        {
            string heading = "";
            if (Load_type == "mail")
                heading = "EMAIL DASHBOARD | SENDER ANALYSIS";
            else if (Load_type == "user")
                heading = "USER & SYSTEM DASHBOARD | APP USAGE ANALYSIS";


            _lblGrphHeader.Text = heading;
            _move_graph(sender);
        }
        private void _grph_7_Click(object sender, EventArgs e)
        {
            string heading = "";
            if (Load_type == "mail")
                heading = "EMAIL DASHBOARD | RECIPITENT ANALYSIS";
            else if (Load_type == "user")
                heading = "USER & SYSTEM DASHBOARD | BANDWIDTH ANALYSIS";

            _lblGrphHeader.Text = heading;
            _move_graph(sender);
        }
        private void _grph_8_Click(object sender, EventArgs e)
        {
            string heading = "";
            if (Load_type == "user")
                heading = "USER & SYSTEM DASHBOARD | MEMORY ANALYSIS";

            _lblGrphHeader.Text = heading;
            _move_graph(sender);
        }
        private void _grph_9_Click(object sender, EventArgs e)
        {
            string heading = "";
            if (Load_type == "mail")
                heading = "COMMUNICATION DASHBOARD | TIME ANALYSIS";

            _lblGrphHeader.Text = heading;
            _move_graph(sender);
        }
        private void _Childgraphvisibility()
        {
            _grph_1.Visible = Load_type == "mail" || Load_type == "meeting" || Load_type == "browser" || Load_type == "project" || Load_type == "user" || Load_type == "system" ? true : false;
            _grph_2.Visible = Load_type == "mail" || Load_type == "meeting" || Load_type == "browser" || Load_type == "project" || Load_type == "user" || Load_type == "system" ? true : false;
            _grph_3.Visible = Load_type == "mail" || Load_type == "meeting" || Load_type == "browser" || Load_type == "project" || Load_type == "user" || Load_type == "system" ? true : false;
            _grph_4.Visible = Load_type == "mail" || Load_type == "browser" || Load_type == "project" || Load_type == "user" ? true : false;
            _grph_5.Visible = Load_type == "mail" || Load_type == "project" || Load_type == "user" ? true : false;
            _grph_6.Visible = Load_type == "mail" || Load_type == "user" ? true : false;
            _grph_7.Visible = Load_type == "mail" || Load_type == "user" ? true : false;
            _grph_8.Visible = Load_type == "user" ? true : false;
            _grph_9.Visible = Load_type == "mail" ? true : false;
        }
    }
}
