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
            LogRepo.Log();
            if ((DateTime.Now - LastOrderedTime).TotalHours > 24 && DateTime.Now.Hour == 3)
            {
                LastOrderedTime = DateTime.Now;
                LogRepo.OrderGamingChannels();
            }
        }

        private void bntLogNow_Click(object sender, EventArgs e)
        {
            LogRepo.Log();
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            LogRepo.OrderGamingChannels();
        }
    }
}
