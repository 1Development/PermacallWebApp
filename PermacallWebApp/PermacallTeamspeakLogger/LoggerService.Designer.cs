namespace PermacallTeamspeakLogger
{
    partial class LoggerService
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.logTimer = new System.Windows.Forms.Timer(this.components);
            // 
            // logTimer
            // 
            this.logTimer.Interval = 300000;
            this.logTimer.Tick += new System.EventHandler(this.logTimer_Tick);
            // 
            // LoggerService
            // 
            this.ServiceName = "LoggerService";

        }

        #endregion

        private System.Windows.Forms.Timer logTimer;
    }
}
