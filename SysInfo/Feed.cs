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
    public partial class Feed : Form
    {
        public Feed()
        {
            InitializeComponent();
            
            panel_breakdown.Visible = true;
            panel_goals.Visible = false;
            DateTime time = DateTime.Today;
            do
            {
                cb_getin.Items.Add(time.ToString("hh:mm tt"));
                cb_getout.Items.Add(time.ToString("hh:mm tt"));
                time = time.AddMinutes(5);
            } while (time.Day == DateTime.Today.Day);

            for (int i = 1; i <= 12; i++)
            {
                cbbreak.Items.Add(i + "Hours");
                cb_email.Items.Add(i + "Hours");
                cb_meeting.Items.Add(i + "Hours");
                cb_deliverable.Items.Add(i + "Hours");
                cb_comm.Items.Add(i + "Hours");
                cb_lunch.Items.Add(i + "Hours");
                cb_personal.Items.Add(i + "Hours");
                cb_distract.Items.Add(i + "Hours");
                cb_idle.Items.Add(i + "Hours");

            }

        }

        private void cb_workoutside_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((cb_getin.SelectedIndex == -1) || (cb_email.SelectedIndex == -1) || (cb_comm.SelectedIndex == -1) || (cbbreak.SelectedIndex == -1) || (cb_email.SelectedIndex == -1) || (cb_deliverable.SelectedIndex == -1)
                 && (cb_meeting.SelectedIndex == -1) || (cb_personal.SelectedIndex == -1) || (cb_lunch.SelectedIndex == -1) || (cb_idle.SelectedIndex == -1) || (cb_getout.SelectedIndex == -1) || (cb_workoutside.SelectedIndex == -1))
            {
                MessageBox.Show("Fill All Details");
            }
            else
            {
                panel_breakdown.Visible = false;
                panel_goals.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((checkedListBox1.CheckedItems.Count <= 0) || (txtgraph.Text.Equals("")))
            {
                MessageBox.Show("Fill all details");
            }
            else MessageBox.Show("Thanks For Registration");
            //this.Dispose();
            Main_Form mainForm = new Main_Form();
            mainForm.Show();
            this.Close();
        }
    }
}
