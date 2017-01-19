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
            if ((txtsname.Text.Equals("")) && (txtcompany.Text.Equals("")) && (txtdept.Text.Equals("")) && (txtteam.Text.Equals("")) && (txtpos.Text.Equals("")))
            {
                MessageBox.Show("Please Fill All Details");
            }
        }

        private void btnteam_Click(object sender, EventArgs e)
        {
            if (txtsname.Text != "" && (txtcompany.Text) != "" && (txtdept.Text) != "" && (txtteam.Text) != "" && (txtpos.Text) != "")
            {
                 
                        panel_self.Visible = false;
                        panel_team.Visible = true;
                        panel_other.Visible = false;
                        panel_client.Visible = false;
                             
            }
            if ((txtmanager.Text.Equals("")) && (txtcolleague1.Text.Equals("")) && (txtcolleague2.Text.Equals("")) && (txtcolleague3.Text.Equals("")))
            {
                MessageBox.Show("Please Enter All Details");
            }
        }
        private void btnclient_Click(object sender, EventArgs e)
        {
            if (txtsname.Text != "" && (txtcompany.Text) != "" && (txtdept.Text) != "" && (txtteam.Text) != "" && (txtpos.Text) != "")
            {
                if ((txtmanager.Text) != "" && (txtcolleague1.Text) != "" && (txtcolleague2.Text) != "" && (txtcolleague3.Text) != "")
                {
                    
                        panel_self.Visible = false;
                        panel_team.Visible = false;
                        panel_other.Visible = false;
                        panel_client.Visible = true;
                    }
                
            }
            if ((txtclient1.Text.Equals("")) && (txtclient2.Text.Equals("")) && (txtclient3.Text.Equals(""))  && (txtvendor1.Text.Equals("")) && (txtvendor2.Text.Equals("")) && (txtvendor3.Text.Equals("")))
            {
                MessageBox.Show("Please Enter ll Details");
            }
        }

        private void btnother_Click(object sender, EventArgs e)
        {
            if (txtsname.Text != "" && (txtcompany.Text) != "" && (txtdept.Text) != "" && (txtteam.Text) != "" && (txtpos.Text) != "")
            {
                if ((txtmanager.Text) != "" && (txtcolleague1.Text) != "" && (txtcolleague2.Text) != "" && (txtcolleague3.Text) != "")
                {
                    if ((txtclient1.Text) != "" && (txtclient2.Text) != "" && (txtclient3.Text) != "" && (txtvendor1.Text) != "" && (txtvendor2.Text) != "" && (txtvendor3.Text) != "")
                    {
                        panel_self.Visible = false;
                        panel_team.Visible = false;
                        panel_other.Visible = true;
                        panel_client.Visible = false;
                    }
                }
            }
            if((txtinternal1.Text.Equals("")) && (txtinternal2.Text.Equals("")) && (txtinternal3.Text.Equals("")) && (txtfriend1.Text.Equals("")) && (txtfriend2.Text.Equals("")) && (txtfriend3.Text.Equals("")) && (txtimp1.Text.Equals("")) && (txtimp2.Text.Equals("")) && (txtimp3.Text.Equals("")))
            {
                MessageBox.Show("Please Enter All Details");
            }
        }

        private void btntutorial_Click(object sender, EventArgs e)
        {
            if (txtsname.Text != "" && (txtcompany.Text) != "" && (txtdept.Text) != "" && (txtteam.Text) != "" && (txtpos.Text) != "")
            {
                if ((txtmanager.Text) != "" && (txtcolleague1.Text) != "" && (txtcolleague2.Text) != "" && (txtcolleague3.Text) != "")
                {
                    if ((txtclient1.Text) != "" && (txtclient2.Text) != "" && (txtclient3.Text) != "" && (txtvendor1.Text) != "" && (txtvendor2.Text) != "" && (txtvendor3.Text) != "")
                    {
                        if ((txtinternal1.Text != "") && (txtinternal2.Text != "") && (txtinternal3.Text != "") && (txtfriend1.Text != "") && (txtfriend2.Text != "") && (txtfriend3.Text != "") && (txtimp1.Text != "") && (txtimp2.Text != "") && (txtimp3.Text != ""))
                        {

                            panel_self.Visible = false;
                            panel_team.Visible = false;
                            panel_other.Visible = false;
                            panel_client.Visible = false;
                        }
                    }
                }
            }
        }

        private void btnsignup_Click(object sender, EventArgs e)
        {
            //Main_Form f = new Main_Form();
            //f._pnlHome.Visible = true;
            panel_self.Visible = false;
            panel_team.Visible = false;
            panel_other.Visible = false;
            panel_client.Visible = false;
        }

        private void btnproject_Click(object sender, EventArgs e)
        {
            if (txtsname.Text != "" && (txtcompany.Text) != "" && (txtdept.Text) != "" && (txtteam.Text) != "" && (txtpos.Text) != "")
            {
                if ((txtmanager.Text) != "" && (txtcolleague1.Text) != "" && (txtcolleague2.Text) != "" && (txtcolleague3.Text) != "")
                {
                    if ((txtclient1.Text) != "" && (txtclient2.Text) != "" && (txtclient3.Text) != "" && (txtvendor1.Text) != "" && (txtvendor2.Text) != "" && (txtvendor3.Text) != "")
                    {
                        if ((txtinternal1.Text != "") && (txtinternal2.Text != "") && (txtinternal3.Text != "") && (txtfriend1.Text != "") && (txtfriend2.Text != "") && (txtfriend3.Text != "") && (txtimp1.Text != "") && (txtimp2.Text != "") && (txtimp3.Text != ""))
                        {

                            project p = new project();
                            p.Show();
                            this.Dispose();
                        }
                    }
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            panel_self.Visible = false;
            panel_team.Visible = false;
            panel_other.Visible = false;
            panel_client.Visible = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            panel_self.Visible = false;
            panel_team.Visible = false;
            panel_other.Visible = false;
            panel_client.Visible = false;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            panel_self.Visible = false;
            panel_team.Visible = false;
            panel_other.Visible = false;
            panel_client.Visible = false;
        }
    }
}
