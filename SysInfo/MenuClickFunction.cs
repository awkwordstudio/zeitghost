using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace SysInfo
{
    public partial class Main_Form
    {
        Bitmap bmp = new Bitmap(500, 500);
        //function to change menu color on click
        private void menu_color()
        {
            Color white = Custom_obj._get_color("white");
            Color gray = Custom_obj._get_color("grey");

            _ptr_home_page.BackColor    = Load_type == "home" ? white : gray;
            _ptr_mail_box.BackColor     = Load_type == "mail" ? white : gray;
            _ptr_calender.BackColor     = Load_type == "meeting" ? white : gray;
            _ptr_browser.BackColor      = Load_type == "browser" ? white : gray;
            _ptrProject.BackColor       = Load_type == "project" ? white : gray;
            _ptrUser.BackColor          = Load_type == "user" ? white : gray;
            _ptr_system.BackColor       = Load_type == "system" ? white : gray;
            _ptrDataArchive.BackColor   = Load_type == "dataarchive" ? white : gray;
            _ptrSetting.BackColor       = Load_type == "setting" ? white : gray;
        }

        // function call from menu click for change menu back color
        private void menu_button_Click()
        {
            menu_color();
            bool set_flag = Load_type == "mail" || Load_type == "meeting" || Load_type == "browser" || Load_type == "project" || Load_type == "user" || Load_type == "system" ? true : false;
            _pnlHome.Visible          = Load_type=="home" ? true : false;
            _pnlMain.Visible          = set_flag;
            _pnlGraph.Visible         = false;
            _pnlSettingMain.Visible   = Load_type == "setting" ? true : false;
            _pnlDataArchive.Visible   = false;
        }

        private void _ptr_home_page_Click(object sender, EventArgs e)           //HOME menu Click
        {
            Load_type = "home";
            menu_button_Click();
        }

        private void _ptr_mail_box_Click(object sender, EventArgs e)            //Mail menu Click
        {
            Load_type = "mail";
            menu_button_Click();
            Color mail_color        = Custom_obj._get_color("mail");
            _pnlHeader.BackColor    = mail_color;
            _lblMainTitle.Text      = "EMAIL DASHBOARD";
            _lblMainTitle.ForeColor = mail_color;
            _lbl_pnl_txt.Text       = "When You Spend Time on Email";
            _lbl_txt_H1.Text        = "email received";
            _lbl_txt_H2.Text        = "hour spent on email";
            _lbl_txt_H3.Text        = "email sent";
            main_page_chart_1.Titles["Title1"].Text = "When You Reply to Emails";
            main_page_chart_2.Titles["Title1"].Text = "When Others Reply to Your Emails";
            main_page_chart_1.Legends["Legend1"].Enabled = false;
            main_page_chart_2.Legends["Legend1"].Enabled = false;

            _graph_pnl_visible(false, true, false, true);                       // show main page gaphs
            main_page_chart_5.Titles["Title1"].Text = "How Many Emails You Send During Meetings";
            main_page_chart_6.Titles["Title1"].Text = "How Many Emails You Send Outside of Work";
            main_page_chart_5.Legends["Legend1"].Enabled = false;
            main_page_chart_6.Legends["Legend1"].Enabled = false;
            load_graphData("day", "mail");
            load_lbl_color(mail_color,true, false, false);          //set color for time period 
            _pnlSideView1.Visible = true;                           // donut graph panel visibility
            _pnlSideView2.Visible = false;                          // line graph panel visibility
            _lstData1.Visible = true;                               //list view visibility 
            _lstData2.Visible = true;                               //list view visibility 
        }

        private void _ptr_calender_Click(object sender, EventArgs e)            //Metting menu Click
        {
            Load_type = "meeting";
            menu_button_Click();
            Color cal_color         = Custom_obj._get_color("meeting");
            _pnlHeader.BackColor    = cal_color;
            _lblMainTitle.Text      = "MEETING DASHBOARD";
            _lblMainTitle.ForeColor = cal_color;
            _lbl_pnl_txt.Text       = "When You Spend Time in Meeting";
            _lbl_txt_H1.Text        = "meeting attended";
            _lbl_txt_H2.Text        = "hours spent in meeting";
            _lbl_txt_H3.Text        = "feture meeting scheduled";
            main_page_chart_1.Titles["Title1"].Text = "How Many Meeting are modified";
            main_page_chart_2.Titles["Title1"].Text = "How You Meet With Others";
            main_page_chart_1.Legends["Legend1"].Enabled = true;
            main_page_chart_2.Legends["Legend1"].Enabled = true;
            main_page_chart_4.Titles["Title1"].Text = "How Many Meetings Recur";
            main_page_chart_5.Titles["Title1"].Text = "How Much Time in Meetings You Spend on Other Activities";
            main_page_chart_6.Titles["Title1"].Text = "What Other Activities You Spend Time on in Meetings";
            main_page_chart_5.Legends["Legend1"].Enabled = false;
            main_page_chart_4.Legends["Legend1"].Enabled = false;
            main_page_chart_6.Legends["Legend1"].Enabled = false;
            _graph_pnl_visible(false, true, true, true);
            load_graphData("day", "calender");
            load_lbl_color(cal_color, true, false, false);
            _pnlSideView1.Visible = true;
            _pnlSideView2.Visible = false;
            _lstData1.Visible = true;                               //list view visibility 
            _lstData2.Visible = false;                               //list view visibility 
        }

        private void _ptr_browser_Click(object sender, EventArgs e)             //Browser menu Click
        {
            Load_type = "browser";
            menu_button_Click();
            Color brw_color         = Custom_obj._get_color("browser");
            _pnlHeader.BackColor    = brw_color;
            _lblMainTitle.Text      = "WEB DASHBOARD";
            _lblMainTitle.ForeColor = brw_color;
            _lbl_pnl_txt.Text       = "When You Spend Time on The Web";
            _lbl_txt_H1.Text        = "different website visited";
            _lbl_txt_H2.Text        = "hours spent on web";
            _lbl_txt_H3.Text        = "webpage visits";
            main_page_chart_1.Titles["Title1"].Text = "Type of Web Browsers";
            main_page_chart_2.Titles["Title1"].Text = "Which Types of Websites You Visit";
            main_page_chart_1.Legends["Legend1"].Enabled = true;
            main_page_chart_2.Legends["Legend1"].Enabled = true;
            _graph_pnl_visible(false, false, false, false);
            _grpMostCommonAtt.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
            _grpMostCommonAtt.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
            load_graphData("day","web");

            load_lbl_color(brw_color,true, false, false);
            _pnlSideView1.Visible = false;
            _pnlSideView2.Visible = true;
            _lstData1.Visible = false;                               //list view visibility 
            _lstData2.Visible = false;                               //list view visibility 
        }

        private void _ptrProject_Click(object sender, EventArgs e)              //Project and File menu Click
        {
            Load_type = "project";
            menu_button_Click();
            Color pr_color          = Custom_obj._get_color("project");
            _pnlHeader.BackColor    = pr_color;
            _lblMainTitle.Text      = "DELIVERABLE DASHBOARD";
            _lblMainTitle.ForeColor = pr_color;
            _lbl_pnl_txt.Text       = "When You Spend Time on Deliverables";
            _lbl_txt_H1.Text        = "project worked on";
            _lbl_txt_H2.Text        = "hours spent on deliverables";
            _lbl_txt_H3.Text        = "deliverables work on";
            main_page_chart_1.Titles["Title1"].Text = "All Time Breakdown by Project";
            main_page_chart_2.Titles["Title1"].Text = "Deliverable Breakdown by Project";
            _graph_pnl_visible(true, true, false, false);
            load_graphData("day", "project");
            load_lbl_color(pr_color, true, false, false);
            _pnlSideView1.Visible = false;
            _pnlSideView2.Visible = true;
            _lstData1.Visible = false;                               //list view visibility 
            _lstData2.Visible = false;                               //list view visibility 
        }

        private void _ptrUser_Click(object sender, EventArgs e)                 //User info menu Click
        {
            Load_type = "user";
            menu_button_Click();
            Color usr_color         = Custom_obj._get_color("user");
            _pnlHeader.BackColor    = usr_color;
            _lblMainTitle.Text      = "USER & SYSTEM DASHBOARD";
            _lblMainTitle.ForeColor = usr_color;
            _lbl_pnl_txt.Text       = "When You Spend Time on Work";
            _lbl_txt_H1.Text        = "hour spent in working";
            _lbl_txt_H2.Text        = "application used";
            _lbl_txt_H3.Text        = "hours idle time";
            main_page_chart_1.Titles["Title1"].Text = "How Much You Listen to music";
            main_page_chart_2.Titles["Title1"].Text = "How Many Open App You Use Hourly";
            main_page_chart_1.Legends["Legend1"].Enabled = false;
            main_page_chart_2.Legends["Legend1"].Enabled = false;

            _graph_pnl_visible(true, true, true, true);
            main_page_chart_3.Titles["Title1"].Text = "How Long Your Attention Span Is";
            main_page_chart_4.Titles["Title1"].Text = "How Much Time You Spend on Distractions";
            main_page_chart_5.Titles["Title1"].Text = "How Much Bandwidth You Typically Use";
            main_page_chart_6.Titles["Title1"].Text = "How Much Memory You Typically Use";
            main_page_chart_3.Legends["Legend1"].Enabled = false;
            main_page_chart_5.Legends["Legend1"].Enabled = false;
            main_page_chart_4.Legends["Legend1"].Enabled = false;
            main_page_chart_6.Legends["Legend1"].Enabled = false;
            load_graphData("day", "user");
            load_lbl_color(usr_color, true, false, false);
            _pnlSideView1.Visible = true;
            _pnlSideView2.Visible = false;
            _lstData1.Visible = false;                               //list view visibility 
            _lstData2.Visible = false;                               //list view visibility 
        }

        private void _ptr_system_Click(object sender, EventArgs e)              //System information menu Click
        {
            Load_type = "system";
            menu_button_Click();
            Color sys_color         = Custom_obj._get_color("system");
            _pnlHeader.BackColor    = sys_color;
            _lblMainTitle.Text      = "COMMUNICATION DASHBOARD";
            _lblMainTitle.ForeColor = sys_color;
            _lbl_pnl_txt.Text       = "When You Spend Time on Communication";
            _lbl_txt_H1.Text        = "hours spent in communication";
            _lbl_txt_H2.Text        = "communication app used";
            _lbl_txt_H3.Text        = "people spoken to";
            main_page_chart_1.Titles["Title1"].Text = "How Much You Communicate";
            main_page_chart_2.Titles["Title1"].Text = "How You Communicate Outside of Email";
            main_page_chart_3.Titles["Title1"].Text = "What App You Use to Communicate";
            main_page_chart_4.Titles["Title1"].Text = "Who You Communicate With";
            main_page_chart_5.Legends["Legend1"].Enabled = true;
            main_page_chart_6.Legends["Legend1"].Enabled = true;
            _graph_pnl_visible(true, false, true, false);
            load_graphData("day", "system");
            load_lbl_color(sys_color, true, false, false);
            _pnlSideView1.Visible = true;
            _pnlSideView2.Visible = false;
            _lstData1.Visible = false;                               //list view visibility 
            _lstData2.Visible = false;                               //list view visibility 
            //system_information();
        }


        private void _ptrDataArchive_Click(object sender, EventArgs e)          // Data archive menu click
        {
            Load_type = "dataarchive";
            menu_button_Click();
            _lblMainTitle.Text ="DATA ARCHIVE";
            _lblMainTitle.ForeColor = Color.FromArgb(91, 155, 213);
            _pnlSideView2.Visible = false;
            _pnlSideView1.Visible = false;

        }

        private void _ptrSetting_Click(object sender, EventArgs e)               //Setting menu Click
        {
            Load_type = "setting";
            menu_button_Click();
            _lblMainTitle.Text ="SETTING";
            _lblMainTitle.ForeColor = Color.FromArgb(91, 155, 213);
            _pnlSideView2.Visible = false;
            _pnlSideView1.Visible = false;
        }
        // function to set visability of panel
        private void _pnl_visible(bool flag1,bool flag2,bool flag3)
        {
            _pnlHome.Visible        = false;
            _pnlMain.Visible        = false;
            _pnlGraph.Visible       = flag1 ? true : false ;
            _pnlDataArchive.Visible = flag2 ? true : false;
            _pnlSettingMain.Visible = flag3 ? true : false;
        }
        private void _graph_pnl_visible(bool flag1, bool flag2, bool flag3,bool flag4)
        {
            main_page_chart_3.Visible = flag1 ? true : false;
            main_page_chart_4.Visible = flag3 ? true : false;
            main_page_chart_5.Visible = flag2 ? true : false;
            main_page_chart_6.Visible = flag4 ? true : false;
        }

        // Double click functionality for show graphs
        private void _ptr_mail_box_DoubleClick(object sender, EventArgs e)      //show mail graph
        {
            Load_type = "mail";
            _lblGrphHeader.Text = "EMAIL DASHBOARD";
            Color color = Custom_obj._get_color("mail");
            _lblGrphHeader.ForeColor = color;
            _pnl_visible(true, false, false);
           _Childgraphvisibility();
        }
        private void _ptr_calender_DoubleClick(object sender, EventArgs e)      // show Meeting graphs
        {
            Load_type = "meeting";
            _lblGrphHeader.Text = "MEETING DASHBOARD";
            Color color = Custom_obj._get_color("meeting");
            _lblGrphHeader.ForeColor = color;
            _pnl_visible(true, false, false);
            _Childgraphvisibility();        }
        private void _ptr_browser_DoubleClick(object sender, EventArgs e)       // show browser graphs
        {
            Load_type = "browser";
            _lblGrphHeader.Text = "WEB DASHBOARD";
            Color color = Custom_obj._get_color("browser");
            _lblGrphHeader.ForeColor = color;
            _pnl_visible(true, false, false);
            _Childgraphvisibility();
        }
        private void _ptrProject_DoubleClick(object sender, EventArgs e)        //Project graphs
        {
            Load_type = "project";
            _lblGrphHeader.Text = "DELIVERABLE DASHBOARD";
            Color color = Custom_obj._get_color("project");
            _lblGrphHeader.ForeColor = color;
            _pnl_visible(true, false, false);
            _Childgraphvisibility();
        }
        private void _ptrUser_DoubleClick(object sender, EventArgs e)           // show User graphs
        {
            Load_type = "user";
            _lblGrphHeader.Text = "USER & SYSTEM DASHBOARD";
            Color color = Custom_obj._get_color("user");
            _lblGrphHeader.ForeColor = color;
            _pnl_visible(true, false, false);
            _Childgraphvisibility();
        }
        private void _ptr_system_DoubleClick(object sender, EventArgs e)        // show System graphs
        {
            Load_type = "system";
            _lblGrphHeader.Text = "COMMUNICATION DASHBOARD";
            Color color = Custom_obj._get_color("system");
            _lblGrphHeader.ForeColor = color;
            _pnl_visible(true, false, false);
            _Childgraphvisibility();
        }
        private void _ptrDataArchive_DoubleClick(object sender, EventArgs e)    //show data archive graphs
        {
            Load_type = "dataarchive";
            _pnl_visible(false, true, false);
        }

        //For Email graphs
        private void _lbl_nameEmDs_Click(object sender, EventArgs e)
        {
            _pnlHome.Visible = false;
            _pnlGraph.Visible = true;
            _pnlMain.Visible = false;
            _pnlDataArchive.Visible = false;
            _pnlSettingMain.Visible = false;
        }
        // change label color
        private void load_lbl_color(Color col,bool flag1,bool flag2,bool flag3)         //set colour to time frame labels
        {
            _lblDay.ForeColor = flag1 ? col : Color.Black;
            _lblWeek.ForeColor = flag2 ? col : Color.Black;
            _lblMonth.ForeColor = flag3 ? col : Color.Black;
        }

        // check time frame (Day, week , month)
        private void TIME_CLICK(string Day,bool flag1,bool flag2,bool flag3)
        {
            Color white = Custom_obj._get_color("white");
            Color n_color = new Color();
            string load_type = "";
            if (_ptr_mail_box.BackColor == white)
            {
                n_color = Color.FromArgb(_pnlHeader.BackColor.R, _pnlHeader.BackColor.G, _pnlHeader.BackColor.B);
                load_type = "mail";
            }
            else if (_ptr_calender.BackColor == white)
            {
                n_color = Color.FromArgb(_pnlHeader.BackColor.R, _pnlHeader.BackColor.G, _pnlHeader.BackColor.B);
                load_type = "calender";
            }
            else if(_ptr_browser.BackColor == white)
            {
                n_color = Color.FromArgb(_pnlHeader.BackColor.R, _pnlHeader.BackColor.G, _pnlHeader.BackColor.B);
                load_type = "web";
            }
            else if(_ptrProject.BackColor == white)
            {
                n_color = Color.FromArgb(_pnlHeader.BackColor.R, _pnlHeader.BackColor.G, _pnlHeader.BackColor.B);
                load_type = "project";
            }
            else if(_ptrUser.BackColor == white)
            {
                n_color = Color.FromArgb(_pnlHeader.BackColor.R, _pnlHeader.BackColor.G, _pnlHeader.BackColor.B);
                load_type = "user";
            }
            else if(_ptr_system.BackColor == white)
            {
                n_color = Color.FromArgb(_pnlHeader.BackColor.R, _pnlHeader.BackColor.G, _pnlHeader.BackColor.B);
                load_type = "system";
            }
            load_lbl_color(n_color, flag1, flag2, flag3);
            load_graphData(Day, load_type);
        }
        // Day Click
        private void _lblDay_Click(object sender, EventArgs e)                 // Day click
        {
            TIME_CLICK("day", true, false, false);
        }
        // Week Click
        private void _lblWeek_Click(object sender, EventArgs e)                 //Week click
        {
            TIME_CLICK("Week", false, true, false);
        }
        // Month Click
        private void _lblMonth_Click(object sender, EventArgs e)                // Month Click
        {
            TIME_CLICK("Month", false, false, true);
        }

        private void setFormSize(int height,int width)
        {
            _pnlHeader.Width =30;
            _pnlHeader.Height = 20;
        }
    }
}
