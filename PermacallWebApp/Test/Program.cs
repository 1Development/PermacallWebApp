﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Test.__test;

namespace Test
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
		/// dankdank
        [STAThread]
        static void Main()
        {

            

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
