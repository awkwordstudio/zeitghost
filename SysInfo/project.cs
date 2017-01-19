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
            //panel_app.Visible = false;
            //panel_meeting.Visible = false;
            //panel_web.Visible = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
           // if((txtprimary.Text.Equals("")) && (txtsecondary.Text.Equals("")) && (txtanother1.Text.Equals("")) && (txtanother2.Text.Equals("")) && (txtdomain.Text.Equals("")) && (txtposition.Text.Equals("")))
           // {
            //    MessageBox.Show("Please Fill All Details");
           // }
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
            if ((txtprimary.Text.Equals("")) || (txtsecondary.Text.Equals("")) || (txtanother1.Text.Equals("")) || (txtanother2.Text.Equals("")) || (txtposition.Text.Equals("")))
            {
                MessageBox.Show("Fill Your Project Details");
            }
            else
            {
                panel_project.Visible = false;
                panel_app.Visible = true;
                panel_meeting.Visible = false;
                panel_web.Visible = false;
            }
             
        }

        private void btnweb_Click(object sender, EventArgs e)
        {
            if ((txtprimary.Text != "") && (txtsecondary.Text != "") && (txtanother1.Text != "") && (txtanother2.Text != "")  && (txtposition.Text != ""))
            {
                if ((check_work.CheckedItems.Count != 0) && check_distract.CheckedItems.Count != 0)
                {
                    panel_project.Visible = false;
                    panel_app.Visible = false;
                    panel_meeting.Visible = false;
                    panel_web.Visible = true;
                }
                else MessageBox.Show("Please select Apps used for work and distraction");
            }else MessageBox.Show("fill all details");
        }

        private void btnmeeting_Click(object sender, EventArgs e)
        {
            if ((txtprimary.Text != "") && (txtsecondary.Text != "") && (txtanother1.Text != "") && (txtanother2.Text != "") && (txtdomain.Text != "") && (txtposition.Text != ""))
            {
                if ((check_work.CheckedItems.Count != 0) && check_distract.CheckedItems.Count != 0)
                {
                    if ((check_web_work.CheckedItems.Count != 0) && (check_web_distract.CheckedItems.Count != 0))
                    {
                        panel_project.Visible = false;
                        panel_app.Visible = false;
                        panel_meeting.Visible = true;
                        panel_web.Visible = false;
                       
                    }
                    else MessageBox.Show("Please select web used for work and distraction");
                    
                }
                
            }
            if (check_meeting.CheckedItems.Count !=0)
            {
                Feed f = new Feed();
                f.Show();
              
            }
            
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
