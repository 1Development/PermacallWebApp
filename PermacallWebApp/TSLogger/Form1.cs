using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TSLogger
{
    public partial class Form1 : Form
    {
        DateTime LastOrderedTime = new DateTime(0);
        public Form1()
        {
            InitializeComponent();
        }

        private void logTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                LogRepo.Log();
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Exception encountered when attempting to log the current users");
                sb.AppendLine(ex.Message);
                sb.AppendLine(ex.StackTrace);
                MailClass.SendNotificationMail(sb.ToString());
            }

            if ((DateTime.Now - LastOrderedTime).TotalHours > 24 && DateTime.Now.Hour == 3)
            {
                LastOrderedTime = DateTime.Now;
                try
                {
                    LogRepo.OrderGamingChannels();
                }
                catch (Exception ex)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Exception encountered when attempting to Order the gaming channels of the teamspeak server.");
                    sb.AppendLine(ex.Message);
                    sb.AppendLine(ex.StackTrace);
                    MailClass.SendNotificationMail(sb.ToString());
                }
            }
        }

        private void bntLogNow_Click(object sender, EventArgs e)
        {
            try
            {
                LogRepo.Log();
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Exception encountered when attempting to log the current users");
                sb.AppendLine(ex.Message);
                sb.AppendLine(ex.StackTrace);
                MailClass.SendNotificationMail(sb.ToString());
            }
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            try
            {
                LogRepo.OrderGamingChannels();
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Exception encountered when attempting to Order the gaming channels of the teamspeak server.");
                sb.AppendLine(ex.Message);
                sb.AppendLine(ex.StackTrace);
                MailClass.SendNotificationMail(sb.ToString());
            }
        }
    }
}
